using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using SlightLibrary.Entities;
using SlightLibrary.Extensions;

namespace SlightLibrary.UI {

    public partial class Toast : UserControl {

        public Toast() {

            InitializeComponent();
        }

        public static readonly DependencyProperty IsToastingProperty =
            DependencyProperty.Register("IsToasting", typeof(bool), typeof(Toast), new PropertyMetadata(default(bool), IsToastingChanged));

        private static void IsToastingChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {

            Toast toast = (Toast) dependencyObject;
            DoubleAnimation animation = (toast.IsToasting) ? new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(.6))) : new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(.6)));
            toast.DoAnimation(animation, OpacityProperty);
        }

        public bool IsToasting {
            get {
                return (bool) GetValue(IsToastingProperty);
            }
            private set {
                SetValue(IsToastingProperty, value);
            }
        }

        public void Show(string message, double time = 2000) {

            MessageLabel.Content = message;
            IsToasting = true;
            ParallelTask task = new ParallelTask(time);
            task.AddTask(SafelyHide);
            task.Start();
        }

        private void SafelyHide() {

            Dispatcher.Invoke(() => {
                IsToasting = false;
            });
        }

        public void Connect(int connectionId, object target) {
            throw new NotImplementedException();
        }
    }
}
