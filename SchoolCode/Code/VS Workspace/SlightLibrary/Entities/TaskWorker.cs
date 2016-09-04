using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SlightLibrary.Bases;
using SlightLibrary.Extensions;

namespace SlightLibrary.Entities {

    public class TaskWorker : IWorker {

        private Thread _workerThread;

        /// <summary>
        /// List of tasks to run
        /// Will wait until startup tasks are completed
        /// </summary>
        public List<Delegates.SimpleTask> TaskList {
            get;
            private set;
        }

        /// <summary>
        /// List of tasks to run once and with high priority
        /// </summary>
        public List<Delegates.SimpleTask> StartupTaskList {
            get;
            private set;
        }

        public event Delegates.SimpleEvent Paused;
        protected virtual void OnPaused(object arguments) {
            Delegates.SimpleEvent handler = Paused;
            if (handler != null)
                handler(this, arguments);
        }

        public event Delegates.SimpleEvent Resumed;
        protected virtual void OnResumed(object arguments) {
            Delegates.SimpleEvent handler = Resumed;
            if (handler != null)
                handler(this, arguments);
        }

        public event Delegates.SimpleEvent PausedChanged;
        protected virtual void OnPausedChanged(object arguments) {
            Delegates.SimpleEvent handler = PausedChanged;
            if (handler != null)
                handler(this, arguments);
        }

        public event Delegates.SimpleEvent Pausing;
        protected virtual void OnPausing(object arguments) {
            Delegates.SimpleEvent handler = Pausing;
            if (handler != null)
                handler(this, arguments);
        }

        public bool IsLooping {
            get;
            private set;
        }

        public bool IsPaused {
            get;
            private set;
        }

        public bool IsPausing {
            get;
            private set;
        }

        public bool IsRunning {
            get;
            private set;
        }

        public int Responsiveness {
            get;
            private set;
        }

        public TaskWorker() {

            StartupTaskList = new List<Delegates.SimpleTask>();
            TaskList = new List<Delegates.SimpleTask>();
        }

        private void RunTasks() {

            foreach (var task in TaskList.Where(task => IsRunning)) {

                CheckForPause();
                if (task != null)
                    task.Invoke();
            }
        }

        private void RunStartupTasks() {

            foreach (var task in StartupTaskList.Where(task => IsRunning)) {

                if (task != null)
                    task.Invoke();
            }
        }

        private void CheckForPause() {

            if (IsPausing) {
                OnPaused(null);
                OnPausedChanged(null);
                IsPaused = true;
                IsPausing = false;
                "Thread pause called, sleeping...".ToConsole();
            }
            while (IsPaused && IsRunning) {
                Thread.Sleep(Responsiveness);
                Console.Write(".");
            }
            IsPaused = false;
            OnResumed(null);
            OnPausedChanged(null);
        }

        private void Run() {

            IsRunning = true;
            IsPaused = false;

            RunStartupTasks();

            do {

                RunTasks();
            } while (IsLooping && IsRunning);

            IsRunning = false;
        }

        /// <summary>
        /// Thread will end safely at the next possible moment
        /// </summary>
        public void RequestStop() {

            IsRunning = false;
        }

        /// <summary>
        /// Worker will pause at the next possible moment
        /// </summary>
        /// <param name="responsiveness"></param>
        public void RequestPause(int responsiveness = 500) {

            Responsiveness = Responsiveness > 50 ? responsiveness : 500;
            IsPausing = true;
            OnPausing(null);
        }

        /// <summary>
        /// Worker will unpuase under the responsiveness time 
        /// </summary>
        public void RequestResume() {

            IsPaused = false;
        }

        public void RequestPauseToggle() {

            if (IsPaused)
                RequestResume();
            else
                RequestPause();
        }

        /// <summary>
        /// Wrapper for the generic thread.join function
        /// </summary>
        public void Join() {

            _workerThread.Join();
        }

        /// <summary>
        /// Start the thread with the given tasks
        /// </summary>
        /// <param name="loop">Loop Worker until stop is called</param>
        public void Start(bool loop = false) {

            IsLooping = loop;
            ThreadStart workerObject = Run;
            _workerThread = new Thread(workerObject);
            _workerThread.Start();
        }
    }
}