using System;
using System.Collections.Generic;
using System.IO;

namespace SlightNetRepairer.Actions {

    public static class ActionHelper {

        private static readonly char[] DelimiterChars = { ';' };

        public static List<Action> ReadActions(string filePath) {

            List<Action> actions = new List<Action>();

            using (StreamReader file = new StreamReader(filePath)) {

                string line;
                while ((line = file.ReadLine()) != null) {

                    Action action = ParseLine(line);
                    if (action != null)
                        actions.Add(action);
                }
            }

            return actions;
        }

        public static List<Action> ReadActions(Stream fileStream) {

            List<Action> actions = new List<Action>();

            using (StreamReader file = new StreamReader(fileStream)) {

                string line;
                while ((line = file.ReadLine()) != null) {

                    Action action = ParseLine(line);
                    if (action != null)
                        actions.Add(action);
                }
            }

            return actions;
        }

        public static Action ParseLine(string str) {

            if (str.StartsWith("#"))
                return null;

            string[] action = str.Split(DelimiterChars, StringSplitOptions.RemoveEmptyEntries);

            if (action.Length != 2)
                return null;

            return new Action {
                Program = action[0].Trim(),
                Text = action[1].Trim()
            };
        }
    }
}
