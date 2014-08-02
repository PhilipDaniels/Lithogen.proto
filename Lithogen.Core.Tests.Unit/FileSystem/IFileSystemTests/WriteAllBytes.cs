using Lithogen.Interfaces.FileSystem;

namespace Lithogen.Core.Tests.Unit.FileSystem.IFileSystemTests
{
    public abstract class WriteAllBytes<T> : IFileSystemBase<T>
        where T : IFileSystem, new()
    {
    }
}
