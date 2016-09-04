using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using SlightLibrary.Entities;
using SlightLibrary.Extensions;

namespace SlightLibrary.UI {
    /// <summary>
    /// Interaction logic for Toast.xaml
    /// </summary>
    public partial class Toast : UserControl {

        public Toast() {
            InitializeComponent();
        }

        public void Show(string message, double time = 5000) {

            UserControl.Visibility = Visibility.Visible;
            MessageLabel.Content = message.ToTitleCase();
            ShowToast(time);
        }

        private void ShowToast(double time) {

            Storyboard storyboard = new Storyboard();
            DoubleAnimation doubleAnimation = new DoubleAnimation {
                From = 0.0,
                To = 1.0,
                Duration = new Duration(TimeSpan.FromSeconds(.4)),
                AutoReverse = false
            };
            Storyboard.SetTargetName(doubleAnimation, Grid.Name);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(OpacityProperty));
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin(this);

            ParallelTask task = new ParallelTask(time);
            task.AddTask(SafelyHide);
            task.Start();
        }

        private void SafelyHide() {

            Dispatcher.Invoke(Hide);
        }

        private void Hide() {

            Storyboard storyboard = new Storyboard();
            DoubleAnimation doubleAnimation = new DoubleAnimation {
                From = 1.0,
                To = 0.0,
                Duration = new Duration(TimeSpan.FromSeconds(.4)),
                AutoReverse = false,
            };
            doubleAnimation.Completed += HideComplete;
            Storyboard.SetTargetName(doubleAnimation, Grid.Name);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(OpacityProperty));
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin(this);
        }

        private void HideComplete(object sender, EventArgs eventArgs) {

            UserControl.Visibility = Visibility.Hidden;
        }

        public void Connect(int connectionId, object target) {
            throw new NotImplementedException();
        }
    }
}
