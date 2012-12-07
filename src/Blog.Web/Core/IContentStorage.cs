namespace Blog.Web.Core
{
	public interface IContentStorage
	{
		string GetContent(string filename);
	}
}