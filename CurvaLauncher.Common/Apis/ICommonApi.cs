namespace CurvaLauncher.Apis;

public interface ICommonApi
{
    public void SetClipboardText(string text);

    public void Open(string name);
    public void OpenExecutable(string file);
}