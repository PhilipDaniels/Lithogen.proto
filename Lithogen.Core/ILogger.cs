
namespace Lithogen.Core
{
    public interface ILogger
    {
        string Prefix { get; }
        void PushPrefix(string prefix);
        void PopPrefix();

        void Msg(string msg, params object[] args);
        void Error(string origin, string code, string msg, params object[] args);

        void Write(string msg);
        void WriteLine(string msg);
    }
}
