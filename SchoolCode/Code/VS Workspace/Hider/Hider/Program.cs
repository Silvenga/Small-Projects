using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hider {
    static class Program {

        private const int SW_HIDE = 0x00;
        private const int SW_SHOW = 0x05;
        private const int WS_EX_APPWINDOW = 0x0040000;
        private const int GWL_EXSTYLE = -0x14;
        private const int GWL_STYLE = -16;
        private const int WS_EX_TOOLWINDOW = 0x80;
        private const long WS_POPUP = 0x80000000;
        private const int GWL_HWNDPARENT = -8;

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr zeroOnly, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex) {
            if(IntPtr.Size == 8)
                return GetWindowLongPtr64(hWnd, nIndex);
            else
                return GetWindowLongPtr32(hWnd, nIndex);
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, IntPtr dwNewLong);

        public static IntPtr SetWindowLongPtr(HandleRef hWnd, int nIndex, IntPtr dwNewLong) {
            if(IntPtr.Size == 8)
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            else
                return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
        }

        static void Main(string[] args) {

            // Hide();
            HideWithDialog();
            Console.ReadLine();

            UnHideWithDialog();

            //Application.Exit();

        }

        public static void Hide() {

            var thread = new Thread(HideWindow);

            thread.Start();
        }

        public static void HideWindow() {

            var parrent = new Form();

            var parrentHWnd = new HandleRef(null, parrent.Handle);

            var ptr = FindWindowByCaption(IntPtr.Zero, "Hangouts");
            var targetHWnd = new HandleRef(null, ptr);

            ptr.ToInt32().ToHex().Print();
            parrent.Handle.ToInt32().ToHex().Print();

            unchecked {
                SetWindowLongPtr(parrentHWnd, GWL_STYLE, (IntPtr) (GetWindowLongPtr(parrentHWnd.Handle, GWL_STYLE).ToInt64() | (int) WS_POPUP));
                SetWindowLongPtr(parrentHWnd, GWL_EXSTYLE, (IntPtr) (GetWindowLongPtr(parrentHWnd.Handle, GWL_EXSTYLE).ToInt32() | WS_EX_TOOLWINDOW));

                SetWindowLongPtr(targetHWnd, GWL_HWNDPARENT, parrentHWnd.Handle);
                SetWindowLongPtr(targetHWnd, GWL_EXSTYLE, (IntPtr) (GetWindowLongPtr(ptr, GWL_EXSTYLE).ToInt32() | ~WS_EX_APPWINDOW));
            }

            Application.Run();
        }

        public static void HideWithDialog() {


            var ptr = FindWindowByCaption(IntPtr.Zero, "Hangouts");
            var targetHWnd = new HandleRef(null, ptr);

            ptr.PrintHex();


            ShowWindow(ptr, SW_HIDE);


            var a = GetWindowLongPtr(ptr, GWL_EXSTYLE);
            Console.WriteLine(a);


            SetWindowLongPtr(targetHWnd, GWL_EXSTYLE, (IntPtr) (GetWindowLongPtr(ptr, GWL_EXSTYLE).ToInt32() | WS_EX_TOOLWINDOW));

            a = GetWindowLongPtr(ptr, GWL_EXSTYLE);
            Console.WriteLine(a);

            ShowWindow(ptr, SW_SHOW);
        }

        public static void PrintHex(this object a) {

            Console.WriteLine("{0:X}", a);
        }

        public static void UnHideWithDialog() {


            var ptr = FindWindowByCaption(IntPtr.Zero, "Hangouts");
            var targetHWnd = new HandleRef(null, ptr);

            ShowWindow(ptr, SW_HIDE);
            SetWindowLongPtr(targetHWnd, GWL_EXSTYLE, (IntPtr) (GetWindowLongPtr(ptr, GWL_EXSTYLE).ToInt32() | ~WS_EX_TOOLWINDOW));
            ShowWindow(ptr, SW_SHOW);
        }

        public static string ToHex(this int value) {

            return value.ToString("X");
        }

        public static void Print(this object obj) {
            Console.WriteLine(obj);
        }
    }
}
