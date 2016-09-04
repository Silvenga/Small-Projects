using System;
using System.Diagnostics;

namespace SlightLibrary.Exceptions {

    public class InputDataException : Exception {

        private readonly string _customMessage;

        public InputDataException(string message = "N/A") {

            _customMessage += ("Bad argument input of method: " + new StackFrame(1).GetMethod().Name);
            _customMessage += ("Method message follows: ");
            _customMessage += (message);
        }

        public override string Message {
            get {
                return _customMessage;
            }
        }
    }

}