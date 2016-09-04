using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using SlightNetRepairer.Actions;
using SlightNetRepairer.ModernUI;
using Timer = System.Timers.Timer;

namespace SlightNetRepairer {

    public partial class MainWindow {

        private static readonly Stopwatch Stopwatch = new Stopwatch();

        private int ActionIndex {
            get;
            set;
        }

        private List<Action> ReadActions {
            get;
            set;
        }

        private string CurrentStatus {
            get {
                return CurrentStatusLabel.Content.ToString();
            }
            set {
                CurrentStatusLabel.Content = value;
                CurrentStatusLabel.IsEnabled = !CurrentStatusLabel.IsEnabled;
                CurrentStatusLabel.IsEnabled = !CurrentStatusLabel.IsEnabled;
            }
        }

        private string PastStatus {
            set {
                PastStatusLabel.Content = value;
                PastStatusLabel.IsEnabled = !PastStatusLabel.IsEnabled;
                PastStatusLabel.IsEnabled = !PastStatusLabel.IsEnabled;
            }
        }

        public MainWindow() {

            InitializeComponent();
            DwmHelper.DropShadowToWindow(this);
        }

        private void Drag(object sender, MouseButtonEventArgs e) {

            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e) {

            Stopwatch.Start();
            ReadActions = ActionHelper.ReadActions(Assembly.GetExecutingAssembly().GetManifestResourceStream("SlightNetRepairer." + "Actions.txt"));
            ProgressBar.Maximum = ReadActions.Count;

            ExtractFiles();

            TaskWorker taskWorker = new TaskWorker("");
            taskWorker.Finished += TaskWorkerOnFinished;
            taskWorker.Start();
        }

        private void ExtractFiles() {

            WriteFile("devcon.exe", Properties.Resources.devcon);
        }

        private static void WriteFile(string name, byte[] resource) {

            string path = Path.Combine("C:/Windows/Temp", name);
            File.WriteAllBytes(path, resource);
        }

        private void TaskWorkerOnFinished(int errorCode) {

            Thread.Sleep((int) ((Stopwatch.ElapsedMilliseconds > 1000) ? 0 : 1000 - Stopwatch.ElapsedMilliseconds));
            Stopwatch.Restart();

            Dispatcher.BeginInvoke(new System.Action(() => {

                if (ActionIndex < ReadActions.Count) {

                    Action action = ReadActions[ActionIndex];
                    PastStatus = CurrentStatus + " - " + ((errorCode == 0) ? "Successful." : "Failed.");
                    CurrentStatus = action.Text;
                    ProgressBar.Value = ActionIndex;

                    TaskWorker taskWorker = new TaskWorker(action.Program);
                    taskWorker.Finished += TaskWorkerOnFinished;
                    taskWorker.Start();

                    ActionIndex++;

                } else {

                    ProgressBar.Value = ActionIndex;

                    PastStatusLabel.Content = CurrentStatus + " - " + ((errorCode == 0) ? "Successful." : "Failed.");
                    CurrentStatus = "Completed Actions";

                    Timer timer = new Timer(1500);
                    timer.Elapsed += Close;
                    timer.Start();
                }
            }));
        }

        private void Close(object sender, ElapsedEventArgs elapsedEventArgs) {

            Dispatcher.BeginInvoke(new System.Action(() => Application.Current.Shutdown(0)));
        }
    }
}
