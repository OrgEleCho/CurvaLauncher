using System;
using System.Collections.Generic;
using System.Windows.Interop;
using CurvaLauncher.Utilities;
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


    Hotkey _registeredLauncherHotkey;
    private readonly ConfigService _configService;
    private readonly HashSet<Hotkey> _registeredCustomHotkeys = new();

    public bool IsLauncherHotkeyRegistered { get; private set; }
    public string? RegisteredHotkey { get; private set; }

    public void RegisterLauncherHotkey()
    {
        string launcherHotkey = _configService.Config.LauncherHotkey;
        if (!HotkeyUtils.TryParseHotkey(launcherHotkey, out var modifiers, out var key))
        {
            IsLauncherHotkeyRegistered = false;
            return;
        }

        if (IsLauncherHotkeyRegistered)
        {
            if (_registeredLauncherHotkey.Modifier == modifiers &&
                _registeredLauncherHotkey.Key == key)
                return;

            _globalHotkeyManager.Unregister(modifiers, key);
        }

        try
        {
            Hotkey hotkey = new Hotkey(modifiers, key);
            _globalHotkeyManager.Register(hotkey, hotkey => App.ShowLauncher());

            _registeredLauncherHotkey = hotkey;

            RegisteredHotkey = launcherHotkey;
            IsLauncherHotkeyRegistered = true;
        }
        catch
        {
            IsLauncherHotkeyRegistered = false;
        }
    }

    public void RegisterCustomHotkeys()
    {
        foreach (var hotkey in _registeredCustomHotkeys)
            _globalHotkeyManager.Unregister(hotkey);

        foreach (var queryHotkey in _configService.Config.CustomQueryHotkeys)
        {
            if (!HotkeyUtils.TryParseHotkey(queryHotkey.Hotkey, out var modifiers, out var key))
                continue;

            try
            {
                var hotkey = new Hotkey(modifiers, key);
                _globalHotkeyManager.Register(hotkey, hotkey => App.ShowLauncherWithQuery(queryHotkey.QueryText));
                _registeredCustomHotkeys.Add(hotkey);
            }
            catch { }
        }
    }

    public bool IsCustomHotkeyRegistered(Hotkey hotkey)
    {
        return _registeredCustomHotkeys.Contains(hotkey);
    }

    public void Register()
    {
        RegisterLauncherHotkey();
        RegisterCustomHotkeys();
    }
}
