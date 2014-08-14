
namespace Lithogen.Core
{
    public interface IBuildStep
    {
        string Name { get; set; }
        bool Execute();
    }
}
