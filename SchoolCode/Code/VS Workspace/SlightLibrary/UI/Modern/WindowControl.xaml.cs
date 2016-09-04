using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SlightLibrary.Extensions;
using SlightLibrary.Helpers;

namespace SlightLibrary.UI.Modern {


    public partial class WindowControl : UserControl {

        public WindowControl() {

            InitializeComponent();
        }

        private bool _canMinimize;
        public bool CanMinimize {
            get {
                return _canMinimize;
            }
            set {
                _canMinimize = value;
                MinLabel.Visibility = (CanMinimize) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(WindowControl), new PropertyMetadata(""));
        public string Header {
            get {
                return (string) GetValue(HeaderProperty);
            }
            set {
                SetValue(HeaderProperty, value);
            }
        }

        public static readonly DependencyProperty ParentWindowProperty =
            DependencyProperty.Register("ParentWindow", typeof(Window), typeof(WindowControl), new PropertyMetadata(default(Window), ParentWindowChange));

        private static void ParentWindowChange(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {

            WindowControl windowControl = (WindowControl) dependencyObject;
            windowControl.HookWindow();
        }

        private void HookWindow() {

            ParentWindow.StateChanged += ParentWindowOnStateChanged;
            ParentWindow.BorderBrush = Colors.Black.ToBrush();
            ParentWindow.BorderThickness = new Thickness(1);
            UIHelper.DropShadowToWindow(ParentWindow);
            ParentWindow.Activated += ParentWindowOnActivatedChange;
            ParentWindow.Deactivated += ParentWindowOnActivatedChange;
        }

        private void ParentWindowOnActivatedChange(object sender, EventArgs eventArgs) {

            ParentWindow.BorderBrush = ((ParentWindow.IsActive) ? Colors.Black : Colors.DarkGray).ToBrush();
        }

        private void ParentWindowOnStateChanged(object sender, EventArgs eventArgs) {

            switch (ParentWindow.WindowState) {
                case WindowState.Normal:
                    MaxLabel.Content = "1";
                    break;
                case WindowState.Maximized:
                    MaxLabel.Content = "2";
                    break;
            }
        }

        public Window ParentWindow {
            get {
                return (Window) GetValue(ParentWindowProperty);
            }
            set {
                SetValue(ParentWindowProperty, value);
            }
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e) {

            if (ParentWindow != null)
                ParentWindow.DragMove();
        }

        private void Close(object sender, MouseButtonEventArgs e) {

            if (ParentWindow != null && e.LeftButton == MouseButtonState.Pressed)
                ParentWindow.Close();
        }

        private void Min(object sender, MouseButtonEventArgs e) {

            if (ParentWindow != null && e.LeftButton == MouseButtonState.Pressed)
                ParentWindow.WindowState = WindowState.Minimized;
        }

        private void Max(object sender, MouseButtonEventArgs e) {

            switch (ParentWindow.WindowState) {
                case WindowState.Normal:
                    ParentWindow.WindowState = WindowState.Maximized;
                    break;
                case WindowState.Maximized:
                    ParentWindow.WindowState = WindowState.Normal;
                    break;
            }
        }

        public void Connect(int connectionId, object target) {

            throw new System.NotImplementedException();
        }
    }
}
