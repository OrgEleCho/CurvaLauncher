using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CurvaLauncher.Services;
using CurvaLauncher.Utilities;
using CurvaLauncher.ViewModels;

namespace CurvaLauncher.Views.Pages
{
    /// <summary>
    /// Interaction logic for SettingsHotkeyPage.xaml
    /// </summary>
    public partial class SettingsHotkeyPage : Wpf.Ui.Controls.UiPage
    {
        public SettingsHotkeyPage(
            SettingsHotkeyViewModel viewModel,
            ConfigService configService,
            HotkeyService hotkeyService)
        {
            ViewModel = viewModel;
            ConfigService = configService;
            HotkeyService = hotkeyService;
            DataContext = this;

            InitializeComponent();
        }

        public SettingsHotkeyViewModel ViewModel { get; }
        public ConfigService ConfigService { get; }
        public HotkeyService HotkeyService { get; }


        [RelayCommand]
        public void Add()
        {
            ConfigService.Config.CustomQueryHotkeys.Add(new());
        }

        [RelayCommand]
        public void Delete()
        {
            int count = ConfigService.Config.CustomQueryHotkeys.Count;

            if (count > 0)
                ConfigService.Config.CustomQueryHotkeys.RemoveAt(count - 1);
        }

        [RelayCommand]
        public void ApplyHotkeys()
        {
            var customHotkeys = ConfigService.Config.CustomQueryHotkeys;
            for (int i = 0; i < customHotkeys.Count; i++)
            {
                var customHotkey = customHotkeys[i];

                if (string.IsNullOrWhiteSpace(customHotkey.Hotkey))
                {
                    customHotkeys.RemoveAt(i);
                    i--;
                }
            }

            HotkeyService.RegisterCustomHotkeys();
        }

        [RelayCommand]
        public void CheckHotkeys()
        {
            ViewModel.ErrorHotkeys.Clear();

            foreach (var customHotkey in ConfigService.Config.CustomQueryHotkeys)
            {
                if (!HotkeyUtils.TryParseHotkey(customHotkey.Hotkey, out var modifiers, out var key) ||
                    !HotkeyService.IsCustomHotkeyRegistered(new EleCho.GlobalHotkey.Hotkey(modifiers, key)))
                    ViewModel.ErrorHotkeys.Add(customHotkey.Hotkey);
            }
        }
    }
}
