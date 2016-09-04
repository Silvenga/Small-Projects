using System;
using System.Drawing;
using System.Windows.Forms;

namespace sadns {
    class UI {

        private readonly NotifyIcon _icon;

        public UI() {

            ContextMenu contextMenu = CreateMenuItems();

            _icon = new NotifyIcon {
                ContextMenu = contextMenu,
                Icon = SystemIcons.Information,
                Text = "Dyn DNS",
            };
        }

        public void Show() {

            _icon.Visible = true;
        }

        private static ContextMenu CreateMenuItems() {

            ContextMenu menu = new ContextMenu();

            menu.MenuItems.Add(new MenuItem("Exit", Exit));

            return menu;
        }

        private static void Exit(object sender, EventArgs eventArgs) {

            Environment.Exit(0);
        }
    }
}
