
namespace Lithogen.Interfaces
{
    public interface IBuildStep
    {
        string Name { get; set; }
        bool Execute(IBuildContext buildContext);
    }
}
