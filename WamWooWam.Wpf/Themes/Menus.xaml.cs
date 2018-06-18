using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

namespace WamWooWam.Wpf.Theme
{
    partial class Menus : ResourceDictionary
    {
        public Menus()
        {
            InitializeComponent();
        }

        private void ContextMenu_Loaded(object sender, RoutedEventArgs e)
        {
            if (Misc.IsWindows10)
            {
                var s = (sender as ContextMenu);
                var res = s.FindResource("SystemChromeLowBrush") as SolidColorBrush;
                var c = res.Clone();
                c.Opacity = 0.66;

                s.Background = c;

                HwndSource source = (HwndSource)HwndSource.FromVisual(s);
                Accent.SetAccentState(source.Handle, AccentState.EnableBlurBehind);
            }
        }

        private void MenuItem_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
