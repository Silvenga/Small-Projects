using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SlightLibrary.UI.Models {
    /// <summary>
    /// Interaction logic for FileIconViewer.xaml
    /// </summary>
    public partial class FileIconViewer : UserControl {

        public FileIconViewer() {
            InitializeComponent();
        }

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(double), typeof(FileIconViewer), new PropertyMetadata(default(double), SizePropertyProperty));

        private static void SizePropertyProperty(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {

            FileIconViewer viewer = (FileIconViewer) dependencyObject;
            viewer.RefreshSize();
        }

        public double Size {
            get {
                return (double) GetValue(SizeProperty);
            }
            set {
                SetValue(SizeProperty, value);
            }
        }

        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register("FilePath", typeof(object), typeof(FileIconViewer), new PropertyMetadata(default(object), FilePathPropertyChanged));

        private static void FilePathPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {

            FileIconViewer viewer = (FileIconViewer) dependencyObject;
            viewer.RefreshFileIcon();
        }

        public object FilePath {
            get {
                return GetValue(FilePathProperty).ToString();
            }
            set {
                SetValue(FilePathProperty, value);
            }
        }

        public void RefreshSize() {

            Height = Size;
            Width = Size;
        }

        public void RefreshFileIcon() {

            Image.DataContext = FilePath;
        }

        public void Connect(int connectionId, object target) {
            throw new NotImplementedException();
        }
    }
}
