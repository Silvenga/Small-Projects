using System;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace SlightNetRepairer.ModernUI {

    static class DwmHelper {

        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        private static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins pMarInset);

        public static void DropShadowToWindow(Window window) {

            if (!DropShadow(window))
                window.SourceInitialized += window_SourceInitialized;
        }

        private static void window_SourceInitialized(object sender, EventArgs e) {

            Window window = (Window) sender;
            DropShadow(window);
            window.SourceInitialized -= window_SourceInitialized;
        }

        private static bool DropShadow(Window window) {

            try {

                WindowInteropHelper helper = new WindowInteropHelper(window);
                int val = 2;
                int ret1 = DwmSetWindowAttribute(helper.Handle, 2, ref val, 4);

                if (ret1 != 0)
                    return false;

                Margins m = new Margins {
                    Bottom = 0,
                    Left = 0,
                    Right = 0,
                    Top = 0
                };

                return DwmExtendFrameIntoClientArea(helper.Handle, ref m) == 0;

            } catch {

                return false;
            }
        }
    }
}
