using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using SlightLibrary.Entities;
using Point = System.Drawing.Point;

namespace SlightLibrary.Helpers {

    public static class UIHelper {

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

                Margins m = new Margins(0, 0, 0, 0);

                return DwmExtendFrameIntoClientArea(helper.Handle, ref m) == 0;

            } catch {

                return false;
            }
        }

        public static Taskbar GetTaskbar() {

            return new Taskbar();
        }

        /// <summary>
        /// Get the absolute mouse position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void GetAbsoluteMouseLocation(out int x, out int y) {

            x = Control.MousePosition.X;
            y = Control.MousePosition.Y;
        }

        /// <summary>
        /// Get the absolute mouse position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static Point GetAbsoluteMouseLocation() {

            return Control.MousePosition;
        }

        public static Screen GetScreen(Window window) {

            return Screen.FromHandle(new WindowInteropHelper(window).Handle);
        }

    }
}