using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using SlightLibrary.Bases;
using SlightLibrary.Entities;

namespace SlightLibrary.UI.UIModels {

    /// <summary>
    /// Interaction logic for LayoutParent.xaml
    /// </summary>
    public sealed partial class CardLayoutNG : SlightUIElement {

        private bool _isHidden;
        public bool IsHidden {
            get {
                return _isHidden;
            }
            set {
                _isHidden = value;
                OnHidingChanged(_isHidden);
            }
        }

        public CardLayoutNG() {

            InitializeComponent();
            SlideMinimum = 20;
            PeakMinimum = 40;
            SlideDirection = Direction.Left;
            HideLogic = Trinary.Auto;
            Header = "";
            _transition = TransitionMargin;
            TopLevelContent.Children.Add(SlightContent);
        }

        public event Delegates.SimpleEvent HidingChanged;

        private void OnHidingChanged(object arguments) {

            Delegates.SimpleEvent handler = HidingChanged;
            if (handler != null)
                handler(this, arguments);
        }

        public Brush BackgroundControl {

            get {
                return TopLevelContent.Background;
            }
            set {
                TopLevelContent.Background = value;
            }
        }

        public bool UIClipToBounds {
            get {
                return DrawingCanvas.ClipToBounds;
            }
            set {
                DrawingCanvas.ClipToBounds = value;
            }
        }

        public string Header {
            get {
                return TitleLabel.Content.ToString();
            }
            set {
                TitleLabel.Height = (string.IsNullOrEmpty(value)) ? 0 : 26;
                TitleLabel.Content = value;
            }
        }

        public ItemCollection Menu {

            get {
                return MenuControl.Items;
            }
        }

        public bool MenuVisible {

            get {
                return SidePanel.Visibility != Visibility.Collapsed;
            }
            set {
                SidePanel.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Direction SlideDirection {
            get;
            set;
        }

        private ParallelTask _hideParallelTask;

        public void Show(double duration = 0) {

            Dispatcher.Invoke((() => {
                TransitionMargin(Border.Margin, new Thickness(0), Border.Name);
                IsHidden = false;

                TopLevelContent.IsEnabled = true;
                if (duration > 0) {

                    if (_hideParallelTask != null)
                        _hideParallelTask.RequestStop();
                    _hideParallelTask = new ParallelTask(duration);
                    _hideParallelTask.AddTask(InvokeHide);
                    _hideParallelTask.Start();
                }
            }));
        }

        private delegate void Transition(Thickness fromThickness, Thickness toThickness, string controlName);

        readonly Transition _transition;

        public void Peak(double duration = 0, double peakMin = 0) {



            Dispatcher.Invoke((() => {
                if (peakMin > 0)
                    PeakMinimum = peakMin;
                TransitionMargin(Border.Margin, GetPeakMargin(), Border.Name);
                IsHidden = true;

                TopLevelContent.IsEnabled = false;
                if (duration > 0) {

                    if (_hideParallelTask != null)
                        _hideParallelTask.RequestStop();
                    _hideParallelTask = new ParallelTask(duration);
                    _hideParallelTask.AddTask(InvokeHide);
                    _hideParallelTask.Start();
                }

            }));
        }

        public void Hide() {

            Hide(false);
        }

        public void Hide(bool force) {

            if (!IsMouseOver || force) {

                TopLevelContent.IsEnabled = false;

                TransitionMargin(Border.Margin, GetHideMargin(), Border.Name);
                IsHidden = true;
            }
        }

        public void InvokeHide() {

            SafelyInvoke(Hide);
        }

        public void SafelyInvoke(Delegates.SimpleTask taskToInvoke) {

            Delegates.SimpleTask task = taskToInvoke;
            Dispatcher.BeginInvoke(task, null);
        }

        private Trinary _hideLogic;
        public Trinary HideLogic {
            get {
                return _hideLogic;
            }
            set {
                _hideLogic = value;
                DoHideLogic();
            }
        }



        public double SlideMinimum {
            get;
            set;
        }

        public double PeakMinimum {
            get;
            set;
        }

        private Thickness GetHideMargin() {

            switch (SlideDirection) {
                case Direction.Down:
                    return new Thickness(0, Border.ActualHeight - SlideMinimum, 0, 0);
                case Direction.Up:
                    return new Thickness(0, (Border.ActualHeight * -1) + SlideMinimum, 0, 0);
                case Direction.Left:
                    return new Thickness((Border.ActualWidth * -1) + SlideMinimum, 0, 0, 0);
                case Direction.Right:
                    return new Thickness(Border.ActualWidth - SlideMinimum, 0, 0, 0);
            }
            return Border.Margin;
        }

        private Thickness GetPeakMargin() {

            switch (SlideDirection) {
                case Direction.Down:
                    return new Thickness(0, Border.ActualHeight - PeakMinimum, 0, 0);
                case Direction.Up:
                    return new Thickness(0, (Border.ActualHeight * -1) + PeakMinimum, 0, 0);
                case Direction.Left:
                    return new Thickness((Border.ActualWidth * -1) + PeakMinimum, 0, 0, 0);
                case Direction.Right:
                    return new Thickness(Border.ActualWidth - PeakMinimum, 0, 0, 0);
            }
            return Border.Margin;
        }

        public void Connect(int connectionId, object target) {

            throw new NotImplementedException();
        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e) {

            IsHidden = false;
            DoHideLogic();
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e) {

            IsHidden = true;
            DoHideLogic();
        }

        public void DoHideLogic() {

            switch (HideLogic) {
                case Trinary.Auto:
                    if (IsHidden)
                        Show();
                    else
                        Hide();
                    break;
                case Trinary.False:
                    Hide();
                    break;
                case Trinary.True:
                    Show();
                    break;
            }
        }

        public void TransitionMargin(Thickness fromThickness, Thickness toThickness, string controlName) {

            Storyboard storyboard = new Storyboard();
            ThicknessAnimation thicknessAnimation = new ThicknessAnimation {
                From = fromThickness,
                To = toThickness,
                Duration = new Duration(TimeSpan.FromSeconds(.4)),
                AutoReverse = false
            };
            Storyboard.SetTargetName(thicknessAnimation, controlName);
            Storyboard.SetTargetProperty(thicknessAnimation, new PropertyPath(MarginProperty));
            storyboard.Children.Add(thicknessAnimation);
            storyboard.Begin(this);

        }

        private void CardLayout_OnLoaded(object sender, RoutedEventArgs e) {

            DoHideLogic();
        }
    }
}
