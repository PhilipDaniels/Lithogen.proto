
namespace Lithogen.Core
{
    /// <summary>
    /// Determines whether files should be skipped from processing.
    /// </summary>
    public interface IFileFilter
    {
        bool ShouldProcessFile(string file);
    }
}
