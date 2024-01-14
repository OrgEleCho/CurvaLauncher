using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace CurvaLauncher.Plugins;

public abstract class Plugin: IPlugin, INotifyPropertyChanging, INotifyPropertyChanged
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract ImageSource Icon { get; }


    public CurvaLauncherContext HostContext { get; }

    public Plugin(CurvaLauncherContext context)
    {
        HostContext = context;
    }


    protected void OnPropertyChanging([CallerMemberName] string? propertyName = null)
        => PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) 
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


    public event PropertyChangedEventHandler? PropertyChanged;
    public event PropertyChangingEventHandler? PropertyChanging;
}
