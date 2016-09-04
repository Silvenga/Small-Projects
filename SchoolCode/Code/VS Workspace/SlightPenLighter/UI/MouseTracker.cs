

using SlightPenLighter.Hook;
using SlightPenLighter.Models;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlightPenLighter.UI {

    public class MouseTracker {

        public const int MaxEvents = 5;

        public delegate void MouseMovement(Position position);

        public event MouseMovement MouseMove;

        public MouseTracker() {

            EventQueue = new BlockingCollection<Position>(MaxEvents);
            Track();
        }

        public BlockingCollection<Position> EventQueue {
            get;
            set;
        }

        protected virtual void OnMouseMove(Position position) {

            var handler = MouseMove;
            if(handler != null)
                handler(position);
        }

        public void Track() {

            HookManager.MouseMove += HookManagerOnMouseMove;
            PushEvents();
        }

        private void HookManagerOnMouseMove(object sender, MouseEventArgs mouseEventArgs) {

            var next = new Position {
                X = mouseEventArgs.X,
                Y = mouseEventArgs.Y
            };

            var success = EventQueue.TryAdd(next);

            if(!success) {

                Position pos;
                EventQueue.TryTake(out pos);
            }
        }

        private void PushEvents() {

            Task.Run(delegate {

                while(!EventQueue.IsAddingCompleted) {

                    OnMouseMove(EventQueue.Take());
                }
            });
        }
    }
}
