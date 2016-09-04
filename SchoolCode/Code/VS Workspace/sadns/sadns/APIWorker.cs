using System;
using System.Threading;

namespace sadns {
    class APIWorker {

        public bool Loop {
            get;
            private set;
        }

        private readonly Action _action;

        public bool IsRunning {
            get;
            private set;
        }

        public int SecondsToWait {
            get;
            set;
        }

        public APIWorker(Action action, bool loop, int secondsToWait) {

            Loop = loop;
            SecondsToWait = secondsToWait;
            _action = action;
        }

        public void Run() {

            Thread thread = new Thread(Start) {
                IsBackground = false
            };
            thread.Start();
        }

        public void Stop() {

            IsRunning = false;
        }

        private void Start() {

            IsRunning = true;

            string lastIP = "";

            do {

                string currentIP = API.ClientIP();

                Console.WriteLine("Current IP is: " + currentIP);
                Console.WriteLine("Last IP was: " + lastIP);

                if(!currentIP.Equals(lastIP)) {
                    Console.WriteLine("Detected change. Updating DNS records.");
                    _action.Invoke();
                    lastIP = currentIP;
                    Console.WriteLine("Records updated. Next poll in 1 minute.");
                } else {
                    Console.WriteLine("No change detected. Next poll in 1 minute.");
                }

                if(IsRunning && Loop)
                    Thread.Sleep(SecondsToWait * 1000);

            } while(IsRunning && Loop);
        }
    }
}
