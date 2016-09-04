using System.Windows.Forms;

namespace SlightPenLighter.Hook {

    /// <summary>
    /// This class monitors all mouse activities globally (also outside of the application) 
    /// and provides appropriate events.
    /// </summary>
    public static partial class HookManager {

        private static event MouseEventHandler s_MouseMove;

        /// <summary>
        /// Occurs when the mouse pointer is moved. 
        /// </summary>
        public static event MouseEventHandler MouseMove {
            add {
                EnsureSubscribedToGlobalMouseEvents();
                s_MouseMove += value;
            }

            remove {
                s_MouseMove -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }
    }
}
