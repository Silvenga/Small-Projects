using System.Windows.Controls;

namespace SlightLibrary.UI.UIModels {

    public abstract class SlightUIElement : UserControl {

        protected readonly Grid SlightContent = new Grid();

        public UIElementCollection SChildren {

            get {
                return SlightContent.Children;
            }

        }
    }
}
