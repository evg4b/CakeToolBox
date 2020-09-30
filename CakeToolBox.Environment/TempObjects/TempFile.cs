using Cake.Core.IO;

namespace CakeToolBox.Environment.TempObjects
{
    public class TempFile : ITempObject<FilePath>
    {
        private readonly IFileSystem _fileSystem;

        public TempFile(FilePath path, IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            Value = path;
        }
        
        public void Dispose()
        {
            var file = _fileSystem.GetFile(Value.FullPath);
            if (file.Exists)
            {
                file.Delete();
            }
        }

        public FilePath Value { get; }
    }
}