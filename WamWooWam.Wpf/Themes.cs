using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WamWooWam.Wpf.Tools;

namespace WamWooWam.Wpf
{
    public static class Themes
    {
        public static ThemeConfiguration CurrentConfiguration { get; private set; }

        public static void SetTheme(ThemeConfiguration config)
        {
            LoadTheme(config, Application.Current.Resources);
        }

        public static void SetTheme(FrameworkElement element, ThemeConfiguration config)
        {
            LoadTheme(config, element.Resources);
        }

        /// <summary>
        /// Loads and applies my default theme.
        /// </summary>
        /// <param name="light">Chooses between light or dark themes, leave as <see langword="null"/> to use the current Windows setting (if available).</param>
        /// <param name="accentColour">The accent colour the app should use, leave as <see langword="null"/> to use the Windows accent color.</param>
        public static void SetTheme(bool? light = null, Color? accentColour = null)
        {
            LoadTheme(new ThemeConfiguration(light, accentColour), Application.Current.Resources);
        }

        /// <summary>
        /// Loads and applies my default theme to a specified element.
        /// </summary>
        /// <param name="element">The element to load to. Preferably a <see cref="Window"/> or a <see cref="Page"/></param>
        /// <param name="light">Chooses between light or dark themes, leave as <see langword="null"/> to use the current Windows setting (if available).</param>
        /// <param name="accentColour">The accent colour the app should use, leave as <see langword="null"/> to use the Windows accent color.</param>
        public static void SetTheme(FrameworkElement element, bool? light = null, Color? accentColour = null)
        {
            LoadTheme(new ThemeConfiguration(light, accentColour), element.Resources);
        }

        private static void LoadTheme(ThemeConfiguration config, ResourceDictionary current)
        {
            CurrentConfiguration = config;

            current["SystemFontFamily"] = config.FontFamily;
            current["SystemFontSize"] = config.FontSize;

            current["SystemMonospaceFontFamily"] = config.MonospaceFontFamily;
            current["SystemMonospaceFontSize"] = config.MonospaceFontSize;

            if (config.GetColourMode() == ThemeColourMode.Light)
            {
                if (Misc.IsWindows10)
                {
                    foreach (var thing in AccentColorSet.ActiveSet.GetAllColorNames().Where(c => c.StartsWith("Light")))
                    {
                        SetColourResource(current, thing, thing.Substring(5));
                    }
                }
                else
                {
                    current.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/WamWooWam.Wpf;component/Themes/LightColours.xaml", UriKind.Absolute) });
                }
            }
            else
            {
                if (Misc.IsWindows10)
                {
                    foreach (var thing in AccentColorSet.ActiveSet.GetAllColorNames().Where(c => c.StartsWith("Dark")))
                    {
                        SetColourResource(current, thing, thing.Substring(4));
                    }
                }
                else
                {
                    current.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/WamWooWam.Wpf;component/Themes/DarkColours.xaml", UriKind.Absolute) });
                }
            }

            var accent = config.GetAccentColour();
            var accentLightness = 0.299 * ((double)accent.R / 255) + 0.587 * ((double)accent.G / 255) + 0.114 * ((double)accent.B / 255);

            current["SystemAccentColor"] = accent;

            current["SystemAccentLighten3Brush"] = new SolidColorBrush(accent.Lighten(0.6f));
            current["SystemAccentLighten2Brush"] = new SolidColorBrush(accent.Lighten(0.4f));
            current["SystemAccentLighten1Brush"] = new SolidColorBrush(accent.Lighten(0.2f));
            current["SystemAccentBrush"] = new SolidColorBrush(accent);
            current["SystemAccentDarken1Brush"] = new SolidColorBrush(accent.Darken(0.2f));
            current["SystemAccentDarken2Brush"] = new SolidColorBrush(accent.Darken(0.4f));
            current["SystemAccentDarken3Brush"] = new SolidColorBrush(accent.Darken(0.6f));

            current["SystemAccentForegroundBrush"] = new SolidColorBrush(accentLightness > 0.5 ? Colors.Black : Colors.White);
            current["SystemAccentBackgroundBrush"] = new SolidColorBrush(accent) { Opacity = 0.5 };

            current.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/WamWooWam.Wpf;component/Themes/All.xaml", UriKind.Absolute) });
        }

        private static void SetColourResource(ResourceDictionary current, string colour, string trimmed)
        {
            var col = AccentColorSet.ActiveSet[colour];
            var brush = new SolidColorBrush(col);
            var colStr = $"System{trimmed}Color";
            var brushStr = $"System{trimmed}Brush";

            brush.Freeze();
            current[colStr] = col;
            current[brushStr] = brush;
        }
    }

    public class ThemeConfiguration
    {
        public ThemeConfiguration()
        {
            ColourMode = ThemeColourMode.Automatic;
            AccentColour = null;
            FontFamily = new FontFamily("Default");
            MonospaceFontFamily = new FontFamily("Fira Code, Consolas, Lucida Sans Typewriter, Courier New");
            FontSize = (double)(new FontSizeConverter().ConvertFromString("10pt"));
            MonospaceFontSize = (double)(new FontSizeConverter().ConvertFromString("9.5pt"));
        }

        public ThemeConfiguration(bool? light = null, Color? accentColour = null) : this()
        {
            if (light.HasValue)
            {
                ColourMode = light.Value ? ThemeColourMode.Light : ThemeColourMode.Dark;
            }
            else
            {
                ColourMode = ThemeColourMode.Automatic;
            }

            AccentColour = accentColour;
        }

        public ThemeColourMode ColourMode { get; set; }
        public Color? AccentColour { get; set; }
        public FontFamily FontFamily { get; set; }
        public FontFamily MonospaceFontFamily { get; set; }
        public double FontSize { get; set; }
        public double MonospaceFontSize { get; set; }

        internal Color GetAccentColour()
        {
            if (AccentColour == null)
            {
                if (Misc.IsWindows10)
                {
                    try
                    {
                        AccentColour = AccentColorSet.ActiveSet["SystemAccent"];
                    }
                    catch
                    {
                        AccentColour = Color.FromRgb(0x00, 0x78, 0xD7); // default blue
                    }
                }
                else if (Misc.IsWindows8 || Misc.IsWindows7)
                {
                    AccentColour = SystemParameters.WindowGlassColor;
                }
                else
                {
                    AccentColour = Color.FromRgb(0x00, 0x78, 0xD7); // default blue
                }
            }

            return AccentColour.Value;
        }

        internal ThemeColourMode GetColourMode()
        {
            if (ColourMode == ThemeColourMode.Automatic)
            {
                if (Misc.IsWindows10)
                {
                    try
                    {
                        var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
                        var pkey = int.Parse(key.GetValue("AppsUseLightTheme", "1").ToString());
                        ColourMode = pkey != 0 ? ThemeColourMode.Light : ThemeColourMode.Dark;
                    }
                    catch
                    {
                        ColourMode = ThemeColourMode.Light;
                    }
                }
                else
                {
                    ColourMode = ThemeColourMode.Light;
                }
            }

            return ColourMode;
        }
    }

    public enum ThemeColourMode
    {
        Automatic, Light, Dark
    }
}