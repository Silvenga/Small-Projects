using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace SlightLibrary.Extensions {

    public static class UIExtensions {

        public static void InvokeNotify<T>(this PropertyChangedEventHandler handler, T invokingObject, string propertyName) where T : INotifyPropertyChanged {

            if (handler != null)
                handler(invokingObject, new PropertyChangedEventArgs(propertyName));
        }

        public static void BindTo(this FrameworkElement control, string dataContextProperty, DependencyProperty property, BindingMode bindingMode = BindingMode.TwoWay) {

            Binding binding = new Binding(dataContextProperty) {
                Mode = bindingMode
            };
            control.SetBinding(property, binding);
        }

        public static void BindToWithConverter(this FrameworkElement control, string dataContextProperty, DependencyProperty property, IValueConverter converter, BindingMode bindingMode = BindingMode.TwoWay) {

            Binding binding = new Binding(dataContextProperty) {

                Mode = bindingMode,
                Converter = converter
            };

            control.SetBinding(property, binding);
        }

        public static T GetStaticResource<T>(this Application app, string resourceKey) {

            return (T) app.TryFindResource(resourceKey);
        }

        public static BitmapSource ToSourceBitmap(this Bitmap bitMap) {

            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bitMap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        public static void DoAnimation(this FrameworkElement targetElement, AnimationTimeline animation, DependencyProperty targetProperty) {

            Storyboard storyboard = new Storyboard();
            Storyboard.SetTarget(animation, targetElement);
            Storyboard.SetTargetProperty(animation, new PropertyPath(targetProperty));
            storyboard.Children.Add(animation);
            storyboard.Begin();
        }

        public static T FindChild<T>(this DependencyObject parentControl, string childName) where T : DependencyObject {

            T foundChild = null;

            if (parentControl != null && !string.IsNullOrEmpty(childName)) {

                int childrenCount = VisualTreeHelper.GetChildrenCount(parentControl);
                for (int childIndex = 0; childIndex < childrenCount; childIndex++) {

                    DependencyObject possibleChild = VisualTreeHelper.GetChild(parentControl, childIndex);

                    T childType = possibleChild as T;
                    if (childType == null) {

                        foundChild = FindChild<T>(possibleChild, childName);
                        if (foundChild != null)
                            break;

                        FrameworkElement frameworkElement = possibleChild as FrameworkElement;

                        if (frameworkElement != null && frameworkElement.Name == childName) {

                            // ReSharper disable once PossibleInvalidCastException
                            foundChild = (T) possibleChild;
                            break;
                        }
                    } else {

                        foundChild = (T) possibleChild;
                        break;
                    }
                }
            }

            return foundChild;
        }

        public static System.Windows.Media.Color FromColor(this System.Drawing.Color otherColor) {

            return new System.Windows.Media.Color {
                A = otherColor.A,
                B = otherColor.B,
                G = otherColor.G,
                R = otherColor.R
            };
        }

        public static System.Drawing.Color FromColor(this System.Windows.Media.Color otherColor) {

            return System.Drawing.Color.FromArgb(otherColor.A, otherColor.R, otherColor.G, otherColor.B);
        }

        public static SolidColorBrush ToBrush(this System.Windows.Media.Color color) {

            return new SolidColorBrush(color);
        }
    }
}