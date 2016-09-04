using System.Diagnostics;
using System.Threading;

namespace SlightNetRepairer.Actions {

    public sealed class TaskWorker {

        private readonly string _cmdProgram;
        public delegate void TaskComplete(int errorCode);

        /// <summary>
        /// Exit code given by the given program.
        /// "-5992" is given when the program has not finished.
        /// </summary>
        public int ExitCode {
            get;
            private set;
        }

        public TaskWorker(string cmdProgram) {

            _cmdProgram = cmdProgram;
            ExitCode = -5992;
        }

        public event TaskComplete Finished;

        private void OnFinished(int errorcode) {

            TaskComplete handler = Finished;
            if (handler != null)
                handler(errorcode);
        }

        public void Start() {

            ThreadStart workerObject = Worker;
            Thread workerThread = new Thread(workerObject);
            workerThread.Start();
        }

        private void Worker() {

            Process process = new Process {
                StartInfo = {
                    FileName = "cmd.exe",
                    Arguments = "/C " + _cmdProgram,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true
                }
            };

            process.Start();
            process.WaitForExit();

            ExitCode = process.ExitCode;
            OnFinished(ExitCode);
        }
    }
}
