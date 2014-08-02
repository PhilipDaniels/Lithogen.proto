using Lithogen.Interfaces.FileSystem;

namespace Lithogen.Core.Tests.Unit.FileSystem
{
    public abstract class IFileSystem_WriteAllBytes<T> : IFileSystemBase<T>
        where T : IFileSystem, new()
    {
    }
}
