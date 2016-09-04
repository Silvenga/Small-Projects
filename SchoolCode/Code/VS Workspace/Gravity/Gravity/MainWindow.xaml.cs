using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Gravity {


    public partial class MainWindow {

        public bool IsRunning {
            get;
            set;
        }

        public List<BigObject> Objects {
            get;
            set;
        }

        public MainWindow() {

            InitializeComponent();

            const int size = 50;
            var mass = 10 * Math.Pow(10, 11);

            Objects = new List<BigObject> {
                new BigObject {
                    Size = size,
                    Postion = new Vector(300, 300),
                    Color = Colors.Red,
                    Mass = mass ,
                    Vector = new Vector(0, 1),
                    Text = "one"
                },
                new BigObject {
                    Size = size,
                    Postion = new Vector(500, 500),
                    Color = Colors.Blue,
                    Mass = mass ,
                    Vector = new Vector(0, -1)
                    ,Text = "two"
                },
                new BigObject {
                    Size = size,
                    Postion = new Vector(400, 400),
                    Color = Colors.Green,
                    Mass = mass * 1000,
                    Vector = new Vector(0, 0),
                  Text  = "big"
                }
            };
        }

        public double G {
            get {
                return 6.67 * Math.Pow(10, -11);
            }
        }

        public double Force(double m1, double m2, double r) {

            if(r.Equals(0))
                throw new Exception("R must have value.");

            return G * ((m1 * m2) / (r * r));
        }

        public Vector Acceleration(double force, double mass, Vector direction) {

            direction /= direction.Length;

            return direction * (force / mass);
        }

        public Vector Velocity(Vector currentVector, Vector acceleration, double time) {

            return (acceleration * time) + currentVector;
        }

        public Vector Position(Vector currentPosition, Vector velocity, double time) {

            return (velocity * time) + currentPosition;
        }

        public async Task UpdateAsync() {

            await Dispatcher.InvokeAsync(Update);
        }

        public async Task DrawAsync() {

            await Dispatcher.InvokeAsync(Draw);
        }

        public async Task ClearAsync() {

            await Dispatcher.InvokeAsync(() => Canvas.Children.Clear());
        }

        public double Distance(BigObject one, BigObject two) {

            return Math.Sqrt(Math.Pow(one.Postion.X + two.Postion.X, 2) + Math.Pow(one.Postion.Y + two.Postion.Y, 2));
        }

        public Vector Angle(BigObject one, BigObject two) {

            var angle = two.Postion - one.Postion;

            return (angle) / angle.Length;
        }

        private void CalFromOthers(BigObject me, double time) {

            foreach(var ellipse in Objects) {

                if(!Equals(ellipse, me)) {

                    var f = Force(me.Mass, ellipse.Mass, Distance(me, ellipse));
                    var a = Acceleration(f, me.Mass, Angle(me, ellipse));
                    var v = Velocity(me.Vector, a, time);

                    me.Vector = v;
                }
            }
        }

        private void Update() {

            foreach(var ellipse in Objects) {

                CalFromOthers(ellipse, 1.0);
                ellipse.Update();
            }

            while(Objects.Any(x => x.Postion.X < 0)) {

                var offset = Objects.Select(x => x.Postion.X).Where(x => x < 0).OrderByDescending(x => x).First();

                MoveAll(new Vector(offset + 100, 0));
            }

            while(Objects.Any(x => x.Postion.Y < 0)) {

                var offset = Objects.Select(x => x.Postion.Y).Where(x => x < 0).OrderBy(x => x).First();

                MoveAll(new Vector(0, offset + 100));
            }
        }

        private void MoveAll(Vector vector) {

            foreach(var ellipse in Objects) {

                ellipse.Postion += vector;
            }
        }

        private void Draw() {

            foreach(var ellipse in Objects) {

                ellipse.Draw(Canvas);
            }
        }

        private async void Start(object sender, RoutedEventArgs e) {

            if(!IsRunning) {
                Button.Content = "Pause";
                IsRunning = true;
                await Loop();
            } else {
                Button.Content = "Resume";
                IsRunning = false;
            }
        }

        private async Task Loop() {

            do {
                await UpdateAsync();
                await DrawAsync();
                await Task.Delay(1);
                await ClearAsync();

            } while(IsRunning);

            await DrawAsync();
        }

    }
}
