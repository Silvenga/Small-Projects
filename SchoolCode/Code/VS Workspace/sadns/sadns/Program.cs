using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace sadns {

    internal static class Program {

        private static Action _action;
        private static string _ip;
        private static string _domain;
        private static bool _isLooping = true;

        private static void Main(string[] args) {

            if(args.Length > 0)
                StartProgram(args);
            else
                ShowHelp();
        }

        private static void StartProgram(string[] args) {

            ParseOption(args);
            APIWorker worker = new APIWorker(_action, _isLooping, 1 * 60);
            worker.Run();

            if(!args.Contains("--no-ui"))
                Application.Run();
            else if(!args.Contains("--reset")) {
                while(true)
                    Thread.Sleep(500);
            }
        }

        private static void ShowHelp() {

            Console.WriteLine("Format: ");
            Console.WriteLine("sadns [options] <domain>");
            Console.WriteLine("Examples: ");
            Console.WriteLine("sadns web                       # Automaticly set up the web domain for dynamic DNS forwarding.");
            Console.WriteLine("sadns --no-ui web               # Runs the web domain with no gui, mainly used for running under *nix.");
            Console.WriteLine("sadns --reset ftp               # remove any dynamic dns settings from the ftp domain.");
            Console.WriteLine("sadns --custom=127.0.0.1 ftp    # Rather running the ftp domain using automatic detection, run using the ip 127.0.0.1.");
        }

        private static void ParseOption(IList<string> args) {

            if(!args.Contains("--no-ui")) {

                UI ui = new UI();
                ui.Show();
            }

            for(int i = 0; i < args.Count - 1; i++) {

                if(args[i].Contains("--reset")) {

                    _isLooping = false;

                    _action = (delegate {

                        string message;
                        if(!API.DoReset(_domain, out message))
                            Console.WriteLine(message);
                    });

                    break;
                }

                if(args[i].Contains("--custom=")) {

                    _isLooping = false;

                    _ip = args[i].Split('=')[1];
                    _action = (delegate {

                        string message;
                        if(!API.DoCustom(_domain, _ip, out message))
                            Console.WriteLine(message);
                    });

                    break;
                }
            }

            if(_action == null) {

                _action = delegate {

                    string message;
                    if(!API.DoFollow(_domain, out message))
                        Console.WriteLine(message);
                };
            }

            _domain = args.Last();
        }
    }
}
