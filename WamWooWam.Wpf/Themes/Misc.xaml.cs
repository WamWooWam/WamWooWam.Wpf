using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Media;
using WamWooWam.Wpf.Interop;

namespace WamWooWam.Wpf.Theme
{
    partial class MiscResources : ResourceDictionary
    {
        public MiscResources()
        {
            InitializeComponent();
        }

        private void ToolTip_Loaded(object sender, RoutedEventArgs e)
        {
            if (Misc.IsWindows10)
            {
                var s = (sender as ToolTip);
                var res = s.FindResource("SystemChromeLowBrush") as SolidColorBrush;
                var c = res.Clone();
                c.Opacity = 0.66;

                s.Background = c;
                
                HwndSource source = (HwndSource)HwndSource.FromVisual(s);
                Accent.SetAccentState(source.Handle, AccentState.EnableBlurBehind);
            }
        }
    }
}
