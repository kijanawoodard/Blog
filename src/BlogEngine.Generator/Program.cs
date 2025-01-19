using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Syntax;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace BlogEngine.Generator;

public record BlogPost(
    string Slug,
    string Title,
    DateTime Published,
    string[] Tags,
    string Content,
    string SourcePath
);

// BlogEngine.Generator/BlogGenerator.cs


[Generator(LanguageNames.CSharp)]
public class BlogGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var markdownFiles = context.AdditionalTextsProvider
            .Where(f => Path.GetExtension(f.Path).Equals(".md", StringComparison.OrdinalIgnoreCase));

        IncrementalValueProvider<(Compilation, ImmutableArray<AdditionalText>)> sources =
            context.CompilationProvider.Combine(markdownFiles.Collect());

        context.RegisterSourceOutput(sources, GenerateFiles);
    }

    private void GenerateFiles(SourceProductionContext context, 
        (Compilation Compilation, ImmutableArray<AdditionalText> Files) input)
    {
        var posts = new List<BlogPost>();
        var pipeline = new MarkdownPipelineBuilder()
            .UseYamlFrontMatter()
            .UseAdvancedExtensions()
            .Build();

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        foreach (var file in input.Files)
        {
            if (context.CancellationToken.IsCancellationRequested)
                return;

            try
            {
                var content = file.GetText(context.CancellationToken)?.ToString() ?? "";
                
                // Extract front matter
                var document = Markdown.Parse(content, pipeline);
                var frontMatterBlock = document
                    .Descendants<YamlFrontMatterBlock>()
                    .FirstOrDefault();

                if (frontMatterBlock is null)
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        new DiagnosticDescriptor(
                            "BLOG002",
                            "Missing front matter",
                            "No front matter found in {0}",
                            "Generation",
                            DiagnosticSeverity.Warning,
                            isEnabledByDefault: true),
                        Location.None,
                        file.Path));
                    continue;
                }

                var yaml = string.Join("\n", 
                    frontMatterBlock.Lines.Lines.Select(l => l.ToString()));
                var frontMatter = deserializer.Deserialize<Dictionary<string, object>>(yaml);

                // Parse metadata
                var title = frontMatter["title"]?.ToString() ?? 
                    Path.GetFileNameWithoutExtension(file.Path);
                var published = DateTime.Parse(frontMatter["published"]?.ToString() ?? 
                    DateTime.Now.ToString("d"));
                var tags = (frontMatter["tags"] as List<object>)?
                    .Select(t => t?.ToString() ?? "")
                    .Where(t => !string.IsNullOrEmpty(t))
                    .ToArray() ?? Array.Empty<string>();

                // Remove front matter and convert to HTML
                content = content.Replace(yaml, "").TrimStart('-', '\r', '\n');
                var html = Markdown.ToHtml(content, pipeline);
                
                var slug = Path.GetFileNameWithoutExtension(file.Path).ToLowerInvariant();
                
                posts.Add(new BlogPost(slug, title, published, tags, html, file.Path));

                // Generate blog post HTML
                context.AddSource($"{slug}.html",
                    SourceText.From(WrapBlogPost(title, published, tags, html), Encoding.UTF8));
            }
            catch (Exception ex)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    new DiagnosticDescriptor(
                        "BLOG001",
                        "Failed to process markdown file",
                        "Error processing {0}: {1}",
                        "Generation",
                        DiagnosticSeverity.Error,
                        isEnabledByDefault: true),
                    Location.None,
                    file.Path,
                    ex.Message));
            }
        }

        // Generate index.html
        context.AddSource("index.html", 
            SourceText.From(GenerateIndex(posts), Encoding.UTF8));

        // Generate sitemap.xml
        context.AddSource("sitemap.xml",
            SourceText.From(GenerateSitemap(posts), Encoding.UTF8));
    }

    private string WrapBlogPost(string title, DateTime published, string[] tags, string content) => 
$@"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""utf-8"">
    <title>{title}</title>
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
</head>
<body>
    <article>
        <header>
            <h1>{title}</h1>
            <time datetime=""{published:yyyy-MM-dd}"">{published:MMMM dd, yyyy}</time>
            {(tags.Length > 0 ? $@"
            <div class=""tags"">
                {string.Join(", ", tags.Select(t => $"<span class=\"tag\">{t}</span>"))}
            </div>" : "")}
        </header>
        {content}
    </article>
</body>
</html>";

    private string GenerateIndex(List<BlogPost> posts) =>
$@"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""utf-8"">
    <title>Blog</title>
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
</head>
<body>
    <h1>Blog Posts</h1>
    <ul>
        {string.Join("\n        ", posts
            .OrderByDescending(p => p.Published)
            .Select(p => $@"<li>
            <time datetime=""{p.Published:yyyy-MM-dd}"">{p.Published:MMMM dd, yyyy}</time>
            <a href=""{p.Slug}.html"">{p.Title}</a>
            {(p.Tags.Length > 0 ? $@"<div class=""tags"">{string.Join(", ", p.Tags)}</div>" : "")}
        </li>"))}
    </ul>
</body>
</html>";

    private string GenerateSitemap(List<BlogPost> posts) =>
$@"<?xml version=""1.0"" encoding=""UTF-8""?>
<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">
    <url>
        <loc>/index.html</loc>
        <lastmod>{DateTime.Now:yyyy-MM-dd}</lastmod>
    </url>
    {string.Join("\n    ", posts.Select(p => $@"<url>
        <loc>/{p.Slug}.html</loc>
        <lastmod>{p.Published:yyyy-MM-dd}</lastmod>
    </url>"))}
</urlset>";
}