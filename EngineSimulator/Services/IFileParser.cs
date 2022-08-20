namespace EngineSimulator.Services
{
    public interface IFileParser
    {
        Dictionary<string, object> Parse();
        void AddParams(params string[] names);
    }
}
