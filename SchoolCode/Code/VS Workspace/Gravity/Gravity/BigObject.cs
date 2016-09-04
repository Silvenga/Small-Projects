using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Gravity {

    public class BigObject {

        public Color Color {
            get;
            set;
        }

        public double Size {
            get;
            set;
        }

        public double Mass {
            get;
            set;
        }

        public Vector Vector {
            get;
            set;
        }

        public Vector Postion {
            get;
            set;
        }

        public string Text {
            get;
            set;
        }

        public Vector Last {
            get;
            set;
        }

        public void Draw(Grid canvas) {

            var text = new Label {
                Content = "" + (Vector - Last)
            };

            Last = Vector;

            var shape = new Canvas {
                Width = Size,
                Height = Size,
                Background = new SolidColorBrush(Color)
            };

            text.Margin = new Thickness(Postion.X - (Size / 2), Postion.Y - (Size / 2), 0, 0);
            shape.Margin = new Thickness(Postion.X - (Size / 2), Postion.Y - (Size / 2), 0, 0);

            //  canvas.Children.Add(shape);

            canvas.Children.Add(text);

        }

        private Vector CalOut(Vector position, Canvas plane) {

            var x = position.X;
            var y = position.Y;

            var planeWidth = plane.ActualWidth;
            var planeHeight = plane.ActualHeight;

            Console.WriteLine(planeHeight);

            if(x < 1) {

                x = planeWidth - 2;

            } else if(x > planeWidth) {

                x = 2;
            }

            if(y < 0) {

                y = planeHeight - 2;

            } else if(y > planeHeight) {

                y = 2;
            }

            return new Vector(x, y);
        }

        public void Update() {

            Postion += Vector;
        }

        public Canvas Backend {
            get {
                return new Canvas {
                    Width = Size,
                    Height = Size,
                    Background = new SolidColorBrush(Color)
                };
            }
        }

        protected bool Equals(BigObject other) {
            return Color.Equals(other.Color) && Mass.Equals(other.Mass) && Size.Equals(other.Size);
        }

        public override bool Equals(object obj) {
            if(ReferenceEquals(null, obj)) {
                return false;
            }
            if(ReferenceEquals(this, obj)) {
                return true;
            }
            if(obj.GetType() != GetType()) {
                return false;
            }
            return Equals((BigObject) obj);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = Color.GetHashCode();
                hashCode = (hashCode * 397) ^ Mass.GetHashCode();
                hashCode = (hashCode * 397) ^ Size.GetHashCode();
                return hashCode;
            }
        }

    }
}
