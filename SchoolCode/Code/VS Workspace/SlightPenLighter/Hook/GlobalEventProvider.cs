using System.ComponentModel;
using System.Windows.Forms;

namespace SlightPenLighter.Hook {
    /// <summary>
    /// This component monitors all mouse activities globally (also outside of the application) 
    /// and provides appropriate events.
    /// </summary>
    public class GlobalEventProvider : Component {
        /// <summary>
        /// This component raises events. The value is always true.
        /// </summary>
        protected override bool CanRaiseEvents {
            get {
                return true;
            }
        }

        private event MouseEventHandler _mouseMove;

        /// <summary>
        /// Occurs when the mouse pointer is moved. 
        /// </summary>
        public event MouseEventHandler MouseMove {
            add {
                if(_mouseMove == null) {
                    HookManager.MouseMove += HookManager_MouseMove;
                }
                _mouseMove += value;
            }

            remove {
                _mouseMove -= value;
                if(_mouseMove == null) {
                    HookManager.MouseMove -= HookManager_MouseMove;
                }
            }
        }

        void HookManager_MouseMove(object sender, MouseEventArgs e) {
            if(_mouseMove != null) {
                _mouseMove.Invoke(this, e);
            }
        }
    }
}
