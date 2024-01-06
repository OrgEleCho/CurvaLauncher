using System;
using System.Windows.Interop;
using EleCho.GlobalHotkey;
using EleCho.GlobalHotkey.Windows;

namespace CurvaLauncher.Services;

public class HotkeyService
{
    GlobalHotkeyManager _globalHotkeyManager;

    public HotkeyService(
        ConfigService configService)
    {
        HwndSource hwndSource = new HwndSource(
            new HwndSourceParameters()
            {
                HwndSourceHook = Hook,
                ParentWindow = (IntPtr)(-3)
            });

        _globalHotkeyManager = new GlobalHotkeyManager(hwndSource.Handle);
        _configService = configService;
    }

    private IntPtr Hook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (_globalHotkeyManager != null &&
            _globalHotkeyManager.Process(hwnd, msg, wParam, lParam))
        {
            handled = true;
            return (IntPtr)1;
        }

        handled = false;
        return IntPtr.Zero;
    }


    Hotkey _registeredHotkey;
    private readonly ConfigService _configService;

    public bool Registered { get; private set; }


    public void Register()
    {
        ModifierKeys modifier = ModifierKeys.None;
        Key key = Key.None;

        string[] keyStrs = _configService.Config.LauncherHotkey.Split('+');
        foreach (var keyStr in keyStrs)
        {
            if (Enum.TryParse<ModifierKeys>(keyStr, out var _modifierKey))
                modifier |= _modifierKey;
            else if (Enum.TryParse<Key>(keyStr, out var _key))
                key |= _key;
        }

        if (Registered &&
            _registeredHotkey.Modifier == modifier &&
            _registeredHotkey.Key == key)
            return;

        try
        {
            Hotkey hotkey = new Hotkey(modifier, key);
            _globalHotkeyManager.UnregisterAll();
            _globalHotkeyManager.Register(hotkey, hotkey => App.ShowLauncher());

            _registeredHotkey = hotkey;
            Registered = true;
        }
        catch
        {
            Registered = false;
        }
    }
}
