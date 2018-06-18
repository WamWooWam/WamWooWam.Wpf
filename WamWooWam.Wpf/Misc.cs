using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WamWooWam.Wpf
{
    internal static class Misc
    {
        internal static Lazy<Version> _osVersionLazy = new Lazy<Version>(() => Environment.OSVersion.Version);

        public static bool IsWindows10 => _osVersionLazy.Value.Major == 10;
        public static bool IsWindows8 => _osVersionLazy.Value.Major == 6 && (_osVersionLazy.Value.Minor == 2 || _osVersionLazy.Value.Minor == 3);
        public static bool IsWindows7 => _osVersionLazy.Value.Major == 6 && _osVersionLazy.Value.Minor == 1; 
    }
}
