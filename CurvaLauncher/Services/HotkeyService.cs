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

    public string? RegisteredHotkey { get; private set; }

    private bool TryParseHotkey(string hotkey, out ModifierKeys modifiers, out Key key)
    {
        modifiers = ModifierKeys.None;
        key = Key.None;

        string[] keyStrs = _configService.Config.LauncherHotkey.Split('+');
        foreach (var keyStr in keyStrs)
        {
            var _keyStr = keyStr;

            if (_keyStr == "Ctrl")
                _keyStr = "Control";

            if (Enum.TryParse<ModifierKeys>(_keyStr, out var _modifierKey))
                modifiers |= _modifierKey;
            else if (Enum.TryParse<Key>(_keyStr, out var _key))
                key |= _key;
            else
                return false;
        }

        return true;
    }

    public bool IsValidHotkey()
    {
        return TryParseHotkey(_configService.Config.LauncherHotkey, out _, out _);
    }


    public void Register()
    {
        string launcherHotkey = _configService.Config.LauncherHotkey;
        if (!TryParseHotkey(launcherHotkey, out var modifiers, out var key))
        {
            Registered = false;
            return;
        }

        if (Registered &&
            _registeredHotkey.Modifier == modifiers &&
            _registeredHotkey.Key == key)
            return;

        try
        {
            Hotkey hotkey = new Hotkey(modifiers, key);
            _globalHotkeyManager.UnregisterAll();
            _globalHotkeyManager.Register(hotkey, hotkey => App.ShowLauncher());

            _registeredHotkey = hotkey;

            RegisteredHotkey = launcherHotkey;
            Registered = true;
        }
        catch
        {
            Registered = false;
        }
    }
}
