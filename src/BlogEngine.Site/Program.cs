using System.Security.Cryptography;
using System.Text;
using BlazorStatic;
using BlogEngine.Site.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents();
builder.WebHost.UseStaticWebAssets();
builder.Services.AddBlazorStaticService(opt => {
        //opt.IgnoredPathsOnContentCopy.Add("app.css");//pre-build version for tailwind
        opt.HotReloadEnabled = true;
        opt.ShouldGenerateSitemap = true;
        opt.SiteUrl = "https://kijanawoodard.com";
    }
).AddBlazorStaticContentService<PostFrontMatter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>();

Console.WriteLine($"BlogEngine.Site started in {app.Environment.EnvironmentName} mode");
app.UseBlazorStaticGenerator(shutdownApp: !app.Environment.IsDevelopment());

app.Run();

public static class WebsiteKeys
{
    public const string GitHubRepo = "https://github.com/kijanawoodard/Blog";
    public const string X = "https://x.com/";
    public const string Title = "Kijana Woodard's Blog";
    public const string BlogPostStorageAddress = $"{GitHubRepo}/tree/static/src/BlogEngine.Site/Content/Blog";
    public const string BlogLead = "Personal blog of Kijana Woodard";
}

public class PostFrontMatter : BlogFrontMatter
{
    public PostComment[] Comments { get; set; } = [];
}

// ReSharper disable once ClassNeverInstantiated.Global
// These are the properties that are used in the front matter of the blog posts
public class PostComment
{
    public string Email { get; set; } = "";
    public string Name { get; set; } = "";
    public string Message { get; set; } = "";
    public DateTime When { get; set; } = DateTime.MinValue;
}

public static class StringExtensions
{
    public static string ToMd5(this string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return "*************";
        var asciiBytes = Encoding.ASCII.GetBytes(input);
        var hashedBytes = MD5.Create().ComputeHash(asciiBytes);
        var hashedString = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        return hashedString;
    }
}

public static class AssemblyExtensions
    {
        public static AssemblyInfo GetAssemblyInfo()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var fileInfo = new FileInfo(assembly.Location);
            
            return new AssemblyInfo
            {
                Version = assembly.GetName().Version ?? new Version(0, 0),
                LastModified = fileInfo.LastWriteTime
            };
        }

        public class AssemblyInfo
        {
            public Version Version { get; init; } = new(0, 0);
            public DateTime LastModified { get; init; } = DateTime.MinValue;
        }
    }