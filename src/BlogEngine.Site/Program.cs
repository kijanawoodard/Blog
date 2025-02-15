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

app.UseBlazorStaticGenerator(shutdownApp: !app.Environment.IsDevelopment());

app.Run();

public static class WebsiteKeys
{
    public const string GitHubRepo = "https://github.com/BlazorStatic/BlazorStaticMinimalBlog";
    public const string X = "https://x.com/";
    public const string Title = "BlazorStatic Minimal Blog";
    public const string BlogPostStorageAddress = $"{GitHubRepo}/tree/main/Content/Blog";
    public const string BlogLead = "Sample blog created with BlazorStatic and TailwindCSS";
}

public class PostFrontMatter : BlogFrontMatter
{
    public PostComment[] Comments { get; set; } = [];
}

public class PostComment
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string Message { get; set; }
    public DateTime When { get; set; }
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