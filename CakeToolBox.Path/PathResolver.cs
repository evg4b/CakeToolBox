namespace CakeToolBox.Path
{
    using Cake.Common.IO.Paths;
    using Cake.Core;
    using Cake.Core.Annotations;
    using System.IO;
    using System.Linq;

    using CakeIO = Cake.Core.IO;

    public static class PathResolver
    {
        [CakeMethodAlias]
        public static string ResolvePath(this ICakeContext context, params string[] paths)
        {
            return Path.Combine(paths);
        }

        [CakeMethodAlias]
        public static string ResolvePath(this ICakeContext context, params CakeIO.DirectoryPath[] paths)
        {
            return ResolvePath(context, paths.Select(p => p.FullPath).ToArray());
        }

        [CakeMethodAlias]
        public static string ResolvePath(this ICakeContext context, params ConvertableDirectoryPath[] paths)
        {
            return ResolvePath(context, paths.Select(p => p.Path).ToArray());
        }

        [CakeMethodAlias]
        public static string ResolveFullPath(this ICakeContext context, params string[] paths)
        {
            return Path.GetFullPath(ResolvePath(context, paths));
        }

        [CakeMethodAlias]
        public static string ResolveFullPath(this ICakeContext context, params CakeIO.DirectoryPath[] paths)
        {
            return Path.GetFullPath(ResolvePath(context, paths));
        }

        [CakeMethodAlias]
        public static string ResolveFullPath(this ICakeContext context, params ConvertableDirectoryPath[] paths)
        {
            return Path.GetFullPath(ResolvePath(context, paths));
        }
    }
}
