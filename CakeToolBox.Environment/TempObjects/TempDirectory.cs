using Cake.Core.IO;

namespace CakeToolBox.Environment.TempObjects
{
    public class TempDirectory : ITempObject<DirectoryPath>
    {
        private readonly DirectoryPath _path;
        private readonly IFileSystem _fileSystem;

        public TempDirectory(DirectoryPath path, IFileSystem fileSystem)
        {
            _path = path;
            _fileSystem = fileSystem;
        }
        
        public void Dispose()
        {
            var directory = _fileSystem.GetDirectory(_path.FullPath);
            if (directory.Exists)
            {
                directory.Delete(true);
            }
        }

        public DirectoryPath Value { get; }
    }
}