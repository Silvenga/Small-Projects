using System;
using System.Windows.Controls;
using SlightLibrary.UI.UIModels;

namespace SlightLibrary.UI {

    public class LayoutNG : UserControl {

        private SlightUIElement _layoutParent = new CardLayoutNG();
        private readonly Grid _grid = new Grid();

        public SlightUIElement LayoutParent {
            get {
                return _layoutParent;
            }
            set {

                _layoutParent = value;
                Refresh();
            }
        }

        public Object Children {
            get;
            set;
        }

        protected override void OnInitialized(EventArgs e) {

            base.OnInitialized(e);
            Refresh();
        }

        public void Refresh() {

            ContentPresenter content = new ContentPresenter {
                Content = Children
            };
            LayoutParent.SChildren.Clear();
            LayoutParent.SChildren.Add(content);
            try {
                _grid.Children.Clear();
                _grid.Children.Add(LayoutParent);
            } catch (Exception e) {

                Console.WriteLine(e.Message);
                throw;
            }

            Content = _grid;
        }
    }
}
