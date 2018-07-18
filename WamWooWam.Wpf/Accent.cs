using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using WamWooWam.Wpf.Interop;

namespace WamWooWam.Wpf
{ 
    public static class Accent
    {
        [DllImport("user32.dll")]
        private static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        [DllImport("dwmapi.dll")]
        private static extern void DwmEnableBlurBehindWindow(IntPtr hwnd, ref DWM_BLURBEHIND blurBehind);

        public static void SetAccentState(IntPtr wind, AccentState state)
        {
            var accent = new AccentPolicy { AccentState = state };
            var accentStructSize = Marshal.SizeOf(accent);
            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData
            {
                Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };

            SetWindowCompositionAttribute(wind, ref data);

            Marshal.FreeHGlobal(accentPtr);
        }

        public static void SetDWMBlurBehind(IntPtr wind, bool enable)
        {
            var dwmbb = new DWM_BLURBEHIND(enable);
            DwmEnableBlurBehindWindow(wind, ref dwmbb);
        }
    }
}
