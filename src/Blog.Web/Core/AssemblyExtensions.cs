using System;

namespace Blog.Web.Core
{
    public static class AssemblyExtensions
    {
        public static AssemblyInfo GetAssemblyInfo()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var fileInfo = new System.IO.FileInfo(assembly.Location);
            
            return new AssemblyInfo
            {
                Version = assembly.GetName().Version,
                LastModified = fileInfo.LastWriteTime
            };
        }

        public class AssemblyInfo
        {
            public Version Version { get; set; }
            public DateTime LastModified { get; set; }
        }
    }
}