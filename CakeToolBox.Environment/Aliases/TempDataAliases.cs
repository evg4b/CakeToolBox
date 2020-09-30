using CakeToolBox.Environment.TempObjects;

namespace CakeToolBox.Environment.Aliases
{
    using Cake.Core;
    using Cake.Core.IO;
    using Cake.Core.Annotations;
    using System.IO;
    using Path = System.IO.Path;

    public static class TempDataAliases
    {
        [CakeMethodAlias]
        public static ITempObject<DirectoryPath> GetTempDir(this ICakeContext context, bool create = true)
        {
            var path = Path.GetTempPath();
            var file = context.FileSystem.GetDirectory(path);
            if (create)
            {
                file.Create();
            }
            
            return new TempDirectory(new DirectoryPath(path), context.FileSystem);
        }
        
        [CakeMethodAlias]
        public static ITempObject<FilePath> GetTempFile(this ICakeContext context, bool create = true)
        {
            var path = Path.GetTempFileName();
            var file = context.FileSystem.GetFile(path);
            if (create)
            {
                file.Open(FileMode.CreateNew).Close();
            }
            
            return new TempFile(new FilePath(path), context.FileSystem);
        }
    }
}