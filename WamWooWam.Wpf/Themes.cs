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
        /// <summary>
        /// Loads and applies my default theme.
        /// </summary>
        /// <param name="light">Chooses between light or dark themes, leave as <see langword="null"/> to use the current Windows setting (if available).</param>
        /// <param name="accentColour">The accent colour the app should use, leave as <see langword="null"/> to use the Windows accent color.</param>
        public static void SetTheme(bool? light = null, Color? accentColour = null)
        {
            EnsureLight(ref light);
            EnsureAccentColour(ref accentColour);

            LoadTheme(light.Value, accentColour.Value, Application.Current.Resources);
        }

        /// <summary>
        /// Loads and applies my default theme to a specified element.
        /// </summary>
        /// <param name="element">The element to load to. Preferably a <see cref="Window"/> or a <see cref="Page"/></param>
        /// <param name="light">Chooses between light or dark themes, leave as <see langword="null"/> to use the current Windows setting (if available).</param>
        /// <param name="accentColour">The accent colour the app should use, leave as <see langword="null"/> to use the Windows accent color.</param>
        public static void SetTheme(FrameworkElement element, bool? light = null, Color? accentColour = null)
        {
            EnsureLight(ref light);
            EnsureAccentColour(ref accentColour);

            LoadTheme(light.Value, accentColour.Value, element.Resources);
        }

        private static void LoadTheme(bool light, Color accentColour, ResourceDictionary current)
        {
            if (light)
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

            var accentLightness = 0.299 * ((double)accentColour.R / 255) + 0.587 * ((double)accentColour.G / 255) + 0.114 * ((double)accentColour.B / 255);

            current["SystemAccentColor"] = accentColour;

            current["SystemAccentLighten3Brush"] =  new SolidColorBrush(accentColour.Lighten(0.6f));
            current["SystemAccentLighten2Brush"] =  new SolidColorBrush(accentColour.Lighten(0.4f));
            current["SystemAccentLighten1Brush"] =  new SolidColorBrush(accentColour.Lighten(0.2f));
            current["SystemAccentBrush"]         =  new SolidColorBrush(accentColour);
            current["SystemAccentDarken1Brush"]  =  new SolidColorBrush(accentColour.Darken(0.2f));
            current["SystemAccentDarken2Brush"]  =  new SolidColorBrush(accentColour.Darken(0.4f));
            current["SystemAccentDarken3Brush"]  =  new SolidColorBrush(accentColour.Darken(0.6f));

            current["SystemAccentForegroundBrush"] = new SolidColorBrush(accentLightness > 0.5 ? Colors.Black : Colors.White);
            current["SystemAccentBackgroundBrush"] = new SolidColorBrush(accentColour) { Opacity = 0.5 };

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

        private static void EnsureLight(ref bool? light)
        {
            if (light == null)
            {
                if (Misc.IsWindows10)
                {
                    try
                    {
                        var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
                        var l = int.Parse(key.GetValue("AppsUseLightTheme", "1").ToString());
                        light = l != 0;
                    }
                    catch
                    {
                        light = true;
                    }
                }
                else
                {
                    light = true;
                }
            }
        }

        private static void EnsureAccentColour(ref Color? accentColour)
        {
            if (accentColour == null)
            {
                if (Misc.IsWindows10)
                {
                    try
                    {
                        accentColour = AccentColorSet.ActiveSet["SystemAccent"];
                    }
                    catch
                    {
                        accentColour = Color.FromRgb(0x00, 0x78, 0xD7); // default blue
                    }
                }
                else if (Misc.IsWindows8 || Misc.IsWindows7)
                {
                    accentColour = SystemParameters.WindowGlassColor;
                }
                else
                {
                    accentColour = Color.FromRgb(0x00, 0x78, 0xD7); // default blue
                }
            }
        }
    }
}
