public interface IFloppyWebExtension
{
    string Name { get; }
    string Version { get; }
    void Initialize(IServiceProvider services);
    void OnPageLoad(string url);
    void OnBeforeNavigate(string url);
} 