using System.Collections.Generic;

using ChatterBot.Models.SM;

namespace ChatterBot.Actors {
    public class Chatter {

        public Dictionary<string, dynamic> Inputs {
            get;
            set;
        }

        public void Say(string input) {

            var output = HiStateMachine.Start(input);

            Inputs.Add(input, output);
        }
    }
}
