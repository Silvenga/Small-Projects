
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Media;

using SlightPenLighter.Models;
using System;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Threading;
using Application = System.Windows.Application;
using Point = System.Windows.Point;

namespace SlightPenLighter.UI {

    public partial class PenHighlighter {

        private static double _widthCenter;
        private static double _heightCenter;

        public MouseTracker MouseTracker {
            get;
            set;
        }

        public PenHighlighter() {

            InitializeComponent();

            CreateIcon();
            MouseTracker = new MouseTracker();

            Top = 0;
            Left = 0;

            _widthCenter = Width / 2;
            _heightCenter = Height / 2;
        }

        public OptionWindow OptionWindow {
            get;
            set;
        }

        private void MainWindow_OnSourceInitialized(object sender, EventArgs e) {

            Dispatcher.Invoke(new Action(Target), DispatcherPriority.ContextIdle, null);
        }

        private void Target() {

            var windowHandler = new WindowInteropHelper(this).Handle;
            DwmHelper.SetWindowExTransparent(windowHandler);

            OptionWindow = new OptionWindow(this);

            MouseTracker.MouseMove += MouseTrackerOnMouseMove;
        }

        private void CreateIcon() {

            var menu = new ContextMenu();

            var optionsItem = new MenuItem("Show Options");
            optionsItem.Click += OpenOptions;
            menu.MenuItems.Add(optionsItem);

            var item = new MenuItem("Show/Hide Highlighter");
            item.Click += HideShow;
            menu.MenuItems.Add(item);

            var exitItem = new MenuItem("Exit");
            exitItem.Click += Exit;
            menu.MenuItems.Add(exitItem);

            new NotifyIcon {
                ContextMenu = menu,
                Icon = Properties.Resources.icon,
                Text = @"Pen Highlighter",
                Visible = true
            };
        }

        private void OpenOptions(object sender, EventArgs eventArgs) {

            OptionWindow.Show();
        }
        private static void Exit(object sender, EventArgs eventArgs) {

            Application.Current.Shutdown(0);
        }

        private void HideShow(object sender, EventArgs eventArgs) {

            if(IsVisible)
                Hide();
            else
                Show();
        }

        private void MouseTrackerOnMouseMove(Position position) {

            var realpoint = DwmHelper.PixelsToPoints(position.X, position.Y);

            var x = realpoint.X - _widthCenter;
            var y = realpoint.Y - _heightCenter;

            Dispatcher.Invoke(delegate {

                Left = x;
                Top = y;

            }, DispatcherPriority.Render);

        }
    }

}
