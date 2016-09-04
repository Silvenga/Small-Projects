using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using SlightLibrary.Bases;

namespace SlightLibrary.Entities {

    class ParallelTask : IWorker {

        private readonly List<Delegates.SimpleTask> _tasks = new List<Delegates.SimpleTask>();
        private Timer _timer;

        public bool IsRunning {
            get {
                return _timer.Enabled;
            }
        }

        /// <summary>
        /// Durration in milliseconds
        /// </summary>
        public double Duration {
            get;
            private set;
        }

        public void AddTask(Delegates.SimpleTask task) {

            _tasks.Add(task);
        }

        public ParallelTask(double duration) {

            Duration = duration;
        }

        public void Start(bool loop = false) {

            _timer = new Timer(Duration) {
                AutoReset = loop,
            };
            _timer.Elapsed += TimerOnElapsed;
            _timer.Start();
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs) {

            foreach (var simpleTask in _tasks) {
                simpleTask.Invoke();
            }
        }

        public void RequestStop() {

            _timer.Close();
        }

        public void Join() {
        }

        public static void QuickStart(double duration, Delegates.SimpleTask task) {

            if (duration > 0 && task != null) {

                ParallelTask parallelTask = new ParallelTask(duration);
                parallelTask.AddTask(task);
                parallelTask.Start();
            }
        }
    }
}
