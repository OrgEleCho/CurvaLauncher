using System;
using EleCho.GlobalHotkey;

namespace CurvaLauncher.Utilities;

public static class HotkeyUtils
{
    public static bool TryParseHotkey(string hotkey, out ModifierKeys modifiers, out Key key)
    {
        modifiers = ModifierKeys.None;
        key = Key.None;

        string[] keyStrs = hotkey.Split('+');
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

        return modifiers != ModifierKeys.None && key != Key.None;
    }

    public static bool IsValidHotkey(string hotkey) => TryParseHotkey(hotkey, out _, out _);
}
