using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SlightNetRepairer.ModernUI {
    /// <summary>
    /// Interaction logic for WindowControl.xaml
    /// </summary>
    public partial class WindowControl : UserControl {

        public WindowControl() {

            InitializeComponent();
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
            DependencyProperty.Register("ParentWindow", typeof(Window), typeof(WindowControl), new PropertyMetadata(default(Window)));

        public Window ParentWindow {
            get {
                return (Window) GetValue(ParentWindowProperty);
            }
            set {
                SetValue(ParentWindowProperty, value);
            }
        }

        //        public Window ParentWindow {
        //            get;
        //            set;
        //        }

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
    }
}
