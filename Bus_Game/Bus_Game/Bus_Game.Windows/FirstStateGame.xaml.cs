using Bus_Game.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Bus_Game
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class FirstStateGame : Page
    {
        #region Fields

        private SolidColorBrush _DefaultBackground_bn, _DefaultForeground_bn, _PressedBackground_bn, _PressedForeground_bn;
        private DispatcherTimer _Timer, _LastRoundTimer;
        private Tuple<bool, int> _GameInformation = null;
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private Image[] _PlayerDown_Images, _PlayerLeft_Images, _PlayerTop_Images, _PlayerRight_Images, _Center_Images;
        private Card[] _PlayerDown_Cards, _PlayerLeft_Cards, _PlayerTop_Cards, _PlayerRight_Cards, _Center_Cards;
        private Button next_bn, button1_bn, button2_bn;
        private TextBlock button1_2_tx;
        private int round = 1, playingPlayer = 0, buttonPressed = 0, timerTime;
        #endregion

        public FirstStateGame()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }
        #region Initialisation
        #region Init
        private void Init()
        {
            InitImageArrays();
            InitButtons();
            InitColors();
            _Down_Panel.Orientation = Orientation.Horizontal;
            _Top_Panel.Orientation = Orientation.Horizontal;
            Init_Center_Panel();
            initPanel(_Down_Panel, _PlayerDown_Images);
            if (_GameInformation.Item2 >= 1)
                initPanel(_Top_Panel, _PlayerTop_Images);
            if (_GameInformation.Item2 >= 2)
                initPanel(_Left_Panel, _PlayerLeft_Images);
            if (_GameInformation.Item2 >= 3)
                initPanel(_Right_Panel, _PlayerRight_Images);
            Check();
            this._Wrong_Panel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            if (Deck.Instance._cards.Length != 52)
                throw new NotSupportedException();
        }
        #endregion
        #region Buttons
        private void InitButtons()
        {
            next_bn = new Button();
            button1_bn = new Button();
            button2_bn = new Button();
            button1_2_tx = new TextBlock();
            button1_2_tx.Text = "of";
            button1_2_tx.Margin = new Thickness(20, 0, 20, 0);
            button1_2_tx.FontSize = 32;
            MethodToButton(next_bn, "Next", Next_bn_Click);
            MethodToButton(button1_bn, "Button1", Button1_bn_Click);
            MethodToButton(button2_bn, "Button2", Button2_bn_Click);
        }
        private void InitDefaultButtonColor()
        {
            button1_bn.Background = _DefaultBackground_bn;
            button2_bn.Background = _DefaultBackground_bn;
            button1_bn.Foreground = _DefaultForeground_bn;
            button2_bn.Foreground = _DefaultForeground_bn;
        }
        private void MethodToButton(Button button, string text, Action<object, RoutedEventArgs> Method)
        {
            button.Content = text;
            button.Click += new RoutedEventHandler(Method);
        }
        private void InitColors()
        {
            this._PressedBackground_bn = new SolidColorBrush(Colors.Black);
            this._DefaultForeground_bn = new SolidColorBrush(Colors.Gray);
            this._PressedBackground_bn = new SolidColorBrush(Colors.Green);
            this._PressedForeground_bn = new SolidColorBrush(Colors.Yellow);
        }
        #endregion
        #region Images
        private void InitImageArrays()
        {
            _PlayerDown_Images = new Image[4];
            _PlayerDown_Cards = new Card[4];
            _PlayerTop_Images = new Image[4];
            _PlayerTop_Cards = new Card[4];
            _PlayerLeft_Images = new Image[4];
            _PlayerLeft_Cards = new Card[4];
            _PlayerRight_Images = new Image[4];
            _PlayerRight_Cards = new Card[4];
            _Center_Images = new Image[1];
            _Center_Cards = new Card[1];
            _Center_Images[0] = new Image();
            InitImage(_Center_Images[0]);
            for (int i = 0; i < 4; i++)
            {
                _PlayerDown_Images[i] = new Image();
                InitImage(_PlayerDown_Images[i]);

                if (_GameInformation.Item2 >= 1)
                {
                    Debug.WriteLine("Index >= 1");
                    _PlayerTop_Images[i] = new Image();
                    InitImage(_PlayerTop_Images[i]);
                }
                if (_GameInformation.Item2 >= 2)
                {
                    Debug.WriteLine("Index >= 2");
                    _PlayerLeft_Images[i] = new Image();
                    InitImage(_PlayerLeft_Images[i]);
                }
                if (_GameInformation.Item2 >= 3)
                {
                    Debug.WriteLine("Index >= 3");
                    _PlayerRight_Images[i] = new Image();
                    InitImage(_PlayerRight_Images[i]);
                }
            }
        }
        private void InitImage(Image image)
        {
            BitmapImage img = new BitmapImage();
            img.UriSource = new Uri("ms-appx:Resources/Logo.scale-100.png", UriKind.RelativeOrAbsolute);
            image.Width = 150;
            image.Height = 150;
            image.Margin = new Thickness(10, 0, 10, 0);
            image.Source = img;
            image.Stretch = Stretch.Fill;
        }
        #endregion
        #region Panels
        private void InitDefaultPanelColor()
        {
            SolidColorBrush color = new SolidColorBrush(Colors.Gray);
            this._Down_Panel.Background = color;
            this._Left_Panel.Background = color;
            this._Top_Panel.Background = color;
            this._Right_Panel.Background = color;
        }
        private void Init_Center_Panel()
        {
            StackPanel topPanel = new StackPanel();
            StackPanel bottemPanel = new StackPanel();
            topPanel.Orientation = Orientation.Horizontal;
            bottemPanel.Orientation = Orientation.Horizontal;
            initPanel(topPanel, null);
            initPanel(bottemPanel, _Center_Images);

            topPanel.Children.Add(button1_bn);
            topPanel.Children.Add(button1_2_tx);
            topPanel.Children.Add(button2_bn);
            bottemPanel.Children.Add(next_bn);
            _Center_Panel.Children.Add(topPanel);
            _Center_Panel.Children.Add(bottemPanel);
            NextCenterCard();
        }
        private void initPanel(StackPanel panel, Image[] images)
        {
            if (panel != null)
            {
                panel.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                panel.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            }
            if (images != null)
                foreach (Image img in images)
                    panel.Children.Add(img);

        }
        #endregion
        #endregion
        #region NavigationHelper registration
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
            this._GameInformation = (Tuple<bool, int>)(e.Parameter);
            Debug.WriteLine("Info: " + _GameInformation.Item1 + ", " + _GameInformation.Item2);
            Init();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }
        #endregion
        #region GameLogic
        #region Check Functions
        private void Check()
        {
            CheckButtonsState();
            CheckPanels();
            AddImage();
            this._Wrong_Panel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }
        #region ButtonPressed
        private void ButtonPressed()
        {
            if (!RightButtonPressed())
            { Wrong(); }
            else
            { this.timerTime = 2; }
        }
        #endregion
        #region RightButtonPressed
        private bool RightButtonPressed()
        {
            bool result = false;
            #region Round 1
            if (round == 1)
            { result = Round1(); }
            #endregion
            #region Round 2
            else if (round == 2)
            { result = Round2(); }
            #endregion
            #region Round 3
            else if (round == 3)
            { result = Round3(); }
            #endregion
            #region Round 4
            else if (round == 4)
            { result = Round4(); }
            #endregion
            else { return false; }
            return result;
        }
        #endregion
        #region Wrong
        private void Wrong()
        {
            this.timerTime = 2;
            this._Wrong_Panel.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }
        #endregion
        #region CheckButtonsState
        private void CheckButtonsState()
        {
            if (round == 1)
            {
                button1_bn.Content = "Rood";
                button2_bn.Content = "Zwart";
            }
            else if (round == 2)
            {
                button1_bn.Content = "Hoger";
                button2_bn.Content = "Lager";
            }
            else if (round == 3)
            {
                button1_bn.Content = "Binnen";
                button2_bn.Content = "Buiten";
            }
            else if (round == 4)
            {
                button1_bn.Content = "Heb hem al";
                button2_bn.Content = "Heb hem nog niet";
            }
            else
            {
                LastRound();
            }
            InitDefaultButtonColor();
        }
        #endregion
        #region CheckPanels
        private void CheckPanels()
        {
            InitDefaultPanelColor();
            SolidColorBrush color = new SolidColorBrush(Colors.Red);
            if (_GameInformation.Item2 == 1)
            {
                if (playingPlayer == 0)
                    _Down_Panel.Background = color;
                else if (playingPlayer == 1)
                    _Top_Panel.Background = color;
            }
            else if (_GameInformation.Item2 == 2)
            {
                if (playingPlayer == 0)
                    _Down_Panel.Background = color;
                else if (playingPlayer == 1)
                    _Left_Panel.Background = color;
                else if (playingPlayer == 2)
                    _Top_Panel.Background = color;
            }
            else if (_GameInformation.Item2 == 3)
            {
                if (playingPlayer == 0)
                    _Down_Panel.Background = color;
                else if (playingPlayer == 1)
                    _Left_Panel.Background = color;
                else if (playingPlayer == 2)
                    _Top_Panel.Background = color;
                else if (playingPlayer == 3)
                    _Right_Panel.Background = color;
            }
        }
        #endregion
        #region AddImage
        private void AddImage()
        {
            if (_Center_Cards != null)
            {
                #region 2 Players
                if (_GameInformation.Item2 == 1)
                {
                    if (playingPlayer == 1)
                    {
                        if (round == 1) { _PlayerDown_Cards[0] = _Center_Cards[0]; }
                        else if (round == 2) { _PlayerDown_Cards[1] = _Center_Cards[0]; }
                        else if (round == 3) { _PlayerDown_Cards[2] = _Center_Cards[0]; }
                        else if (round == 4) { _PlayerDown_Cards[3] = _Center_Cards[0]; }
                    }
                    else if (playingPlayer == 0)
                    {
                        if (round == 2) { _PlayerTop_Cards[0] = _Center_Cards[0]; }
                        else if (round == 3) { _PlayerTop_Cards[1] = _Center_Cards[0]; }
                        else if (round == 4) { _PlayerTop_Cards[2] = _Center_Cards[0]; }
                        else if (round == 5) { _PlayerTop_Cards[3] = _Center_Cards[0]; }
                    }
                }
                #endregion
                #region 3 Players
                else if (_GameInformation.Item2 == 2)
                {
                    if (playingPlayer == 1)
                    {
                        if (round == 1) { _PlayerDown_Cards[0] = _Center_Cards[0]; }
                        else if (round == 2) { _PlayerDown_Cards[1] = _Center_Cards[0]; }
                        else if (round == 3) { _PlayerDown_Cards[2] = _Center_Cards[0]; }
                        else if (round == 4) { _PlayerDown_Cards[3] = _Center_Cards[0]; }
                    }
                    else if (playingPlayer == 2)
                    {
                        if (round == 1) { _PlayerLeft_Cards[0] = _Center_Cards[0]; }
                        else if (round == 2) { _PlayerLeft_Cards[1] = _Center_Cards[0]; }
                        else if (round == 3) { _PlayerLeft_Cards[2] = _Center_Cards[0]; }
                        else if (round == 4) { _PlayerLeft_Cards[3] = _Center_Cards[0]; }
                    }
                    else if (playingPlayer == 0)
                    {
                        if (round == 2) { _PlayerTop_Cards[0] = _Center_Cards[0]; }
                        else if (round == 3) { _PlayerTop_Cards[1] = _Center_Cards[0]; }
                        else if (round == 4) { _PlayerTop_Cards[2] = _Center_Cards[0]; }
                        else if (round == 5) { _PlayerTop_Cards[3] = _Center_Cards[0]; }
                    }
                }
                #endregion
                #region 4 Players
                else if (_GameInformation.Item2 == 3)
                {
                    if (playingPlayer == 1)
                    {
                        if (round == 1) { _PlayerDown_Cards[0] = _Center_Cards[0]; }
                        else if (round == 2) { _PlayerDown_Cards[1] = _Center_Cards[0]; }
                        else if (round == 3) { _PlayerDown_Cards[2] = _Center_Cards[0]; }
                        else if (round == 4) { _PlayerDown_Cards[3] = _Center_Cards[0]; }
                    }
                    else if (playingPlayer == 2)
                    {
                        if (round == 1) { _PlayerLeft_Cards[0] = _Center_Cards[0]; }
                        else if (round == 2) { _PlayerLeft_Cards[1] = _Center_Cards[0]; }
                        else if (round == 3) { _PlayerLeft_Cards[2] = _Center_Cards[0]; }
                        else if (round == 4) { _PlayerLeft_Cards[3] = _Center_Cards[0]; }
                    }
                    else if (playingPlayer == 3)
                    {
                        if (round == 1) { _PlayerTop_Cards[0] = _Center_Cards[0]; }
                        else if (round == 2) { _PlayerTop_Cards[1] = _Center_Cards[0]; }
                        else if (round == 3) { _PlayerTop_Cards[2] = _Center_Cards[0]; }
                        else if (round == 4) { _PlayerTop_Cards[3] = _Center_Cards[0]; }
                    }
                    else if (playingPlayer == 0)
                    {
                        if (round == 2) { _PlayerRight_Cards[0] = _Center_Cards[0]; }
                        else if (round == 3) { _PlayerRight_Cards[1] = _Center_Cards[0]; }
                        else if (round == 4) { _PlayerRight_Cards[2] = _Center_Cards[0]; }
                        else if (round == 5) { _PlayerRight_Cards[3] = _Center_Cards[0]; }
                    }
                }
                #endregion
                CheckImages();
                _Center_Images[0].Source = LoadImageString("Logo.scale-100");
            }
        }
        #endregion
        #region CheckImages
        private void CheckImages()
        {
            for (int i = 0; i < _PlayerDown_Cards.Length; i++)
                if (_PlayerDown_Cards[i] != null)
                    _PlayerDown_Images[i].Source = LoadImageIndex(_PlayerDown_Cards[i].Index);
            for (int i = 0; i < _PlayerLeft_Cards.Length; i++)
                if (_PlayerTop_Cards[i] != null)
                    _PlayerTop_Images[i].Source = LoadImageIndex(_PlayerTop_Cards[i].Index);
            for (int i = 0; i < _PlayerDown_Cards.Length; i++)
                if (_PlayerLeft_Cards[i] != null)
                    _PlayerLeft_Images[i].Source = LoadImageIndex(_PlayerLeft_Cards[i].Index);
            for (int i = 0; i < _PlayerLeft_Cards.Length; i++)
                if (_PlayerRight_Cards[i] != null)
                    _PlayerRight_Images[i].Source = LoadImageIndex(_PlayerRight_Cards[i].Index);
            for (int i = 0; i < _Center_Cards.Length; i++)
                if (_Center_Cards[i] != null)
                    _Center_Images[i].Source = LoadImageIndex(_Center_Cards[i].Index);
        }
        private BitmapImage LoadImageString(string name)
        { return new BitmapImage(new Uri("ms-appx:Resources/" + name + ".png")); }
        private BitmapImage LoadImageIndex(int index)
        { return LoadImageString(index.ToString()); }
        #endregion
        #endregion
        #region Button functions
        private void Next_bn_Click(object sender, RoutedEventArgs e)
        { NextCenterCard(); }
        private void Button1_bn_Click(object sender, RoutedEventArgs e)
        {
            this.buttonPressed = 1;
            StartTimer();
            this.button1_bn.Background = _PressedBackground_bn;
            this.button1_bn.Foreground = _PressedForeground_bn;
        }
        private void Button2_bn_Click(object sender, RoutedEventArgs e)
        {
            this.buttonPressed = 2;
            StartTimer();
            this.button2_bn.Background = _PressedBackground_bn;
            this.button2_bn.Foreground = _PressedForeground_bn;
        }
        #endregion
        #region Timers + Action
        private void StartTimer()
        {
            if (_Timer == null)
            {
                TimerActionCount = 0;
                this._Timer = new DispatcherTimer();
                this._Timer.Tick += TimerAction;
                this._Timer.Interval = new TimeSpan(0, 0, timerTime);
                this._Timer.Start();
            }
        }
        private void LastRound()
        {
            if (_LastRoundTimer == null)
            {
                this._LastRoundTimer = new DispatcherTimer();
                this._LastRoundTimer.Tick += LastRoundAction;
                this._LastRoundTimer.Interval = new TimeSpan(0, 0, 2);
                this._LastRoundTimer.Start();
            }
        }
        private int TimerActionCount = 0;
        private void TimerAction(object sender, object e)
        {
            if (TimerActionCount == 0)
            {
                ButtonPressed();
                NextCenterCard();
                CheckImages();
            }
            else if (TimerActionCount == 1)
            {
                if (playingPlayer == _GameInformation.Item2)
                {
                    playingPlayer = 0;
                    round++;
                }
                else
                {
                    playingPlayer++;
                }
                Check();
                this._Timer.Stop();
                this._Timer = null;
            }
            TimerActionCount++;
        }
        private void LastRoundAction(object sender, object e)
        {
            this.Frame.Navigate(typeof(SecondStateGame), e);
            this._LastRoundTimer.Stop();
            this._LastRoundTimer = null;
        }
        #endregion
        #region Logic
        #region Round 1
        private bool Round1()
        {
            bool red = (_Center_Cards[0].Index <= 26 ? true : false);
            Debug.WriteLine("Button Pressed: " + buttonPressed + ", Red: " + red);
            if (buttonPressed == 1 && red) { return true; }
            else if (buttonPressed == 2 && !red) { return true; }
            return false;
        }
        #endregion
        #region Round 2
        private bool Round2()
        {
            bool lower = false;
            if (_GameInformation.Item2 == 1)
            {
                if (playingPlayer == 0) { lower = (_PlayerDown_Cards[0].Value < _Center_Cards[0].Value ? true : false); }
                else if (playingPlayer == 1) { lower = (_PlayerTop_Cards[0].Value < _Center_Cards[0].Value ? true : false); }
            }
            Debug.WriteLine("Button Pressed: " + buttonPressed + ", Lower: " + lower);
            if (buttonPressed == 1 && lower) { return true; }
            else if (buttonPressed == 2 && !lower) { return true; }
            return false;
        }
        #endregion
        #region Round 3
        private bool Round3()
        {
            bool inside = false;
            if (_GameInformation.Item2 == 1)
            {
                if (playingPlayer == 0) { inside = InBetween_Down(); }
                else if (playingPlayer == 1) { inside = InBetween_Top(); }
            }
            else if (_GameInformation.Item2 == 2)
            {
                if (playingPlayer == 0) { inside = InBetween_Down(); }
                else if (playingPlayer == 1) { inside = InBetween_Left(); }
                else if (playingPlayer == 2) { inside = InBetween_Top(); }
            }
            else if (_GameInformation.Item2 == 3)
            {
                if (playingPlayer == 0) { inside = InBetween_Down(); }
                else if (playingPlayer == 1) { inside = InBetween_Left(); }
                else if (playingPlayer == 2) { inside = InBetween_Top(); }
                else if (playingPlayer == 3) { inside = InBetween_Right(); }
            }
            Debug.WriteLine("Button Pressed: " + buttonPressed + ", Inside: " + inside);
            if (buttonPressed == 1 && inside) { return true; }
            else if (buttonPressed == 2 && !inside) { return true; }
            return false;
        }
        private bool InBetween_Down() { return InBetween_Round3(_PlayerDown_Cards[0].Value, _PlayerDown_Cards[1].Value, _Center_Cards[0].Value); }
        private bool InBetween_Left() { return InBetween_Round3(_PlayerLeft_Cards[0].Value, _PlayerLeft_Cards[1].Value, _Center_Cards[0].Value); }
        private bool InBetween_Top() { return InBetween_Round3(_PlayerTop_Cards[0].Value, _PlayerTop_Cards[1].Value, _Center_Cards[0].Value); }
        private bool InBetween_Right() { return InBetween_Round3(_PlayerRight_Cards[0].Value, _PlayerRight_Cards[1].Value, _Center_Cards[0].Value); }
        private bool InBetween_Round3(int value1, int value2, int other)
        {
            Debug.WriteLine(value1 + " < " + other + " < " + value2 + " | OR | " + value2 + " < " + other + " < " + value1);
            if (value1 < other && other < value2)
                return true;
            if (value2 < other && other < value1)
                return true;
            return false;
        }
        #endregion
        #region Round 4
        private bool Round4()
        {
            bool inPossession = false;

            Debug.WriteLine("Button Pressed: " + buttonPressed + ", In Possession: " + inPossession);
            if (buttonPressed == 1 && inPossession) { return true; }
            else if (buttonPressed == 2 && !inPossession) { return true; }
            return false;
        }
        #endregion
        #region NextCenterCard
        private void NextCenterCard()
        {
            Random random = new Random();
            //int index = random.Next(0, 104);
            bool succes = false;
            int index = 0, count = 0;
            while (!succes)
            {
                index = random.Next(1, 29);
                if (!Deck.Instance._cards[index].Taken())
                    succes = !Deck.Instance._cards[index].Taken();
                count++;
                if (!succes && count > Deck.Instance._cards.Length)
                {
                    succes = true;
                    index = -1;
                }
            }
            index = (index > 52 ? index - 52 : index);
            if (index != -1)
            { Deck.Instance._cards[index].Taken(true); }
            SetCenterCard(index);
        }
        #endregion
        #region SetCenterCard(int index)
        private void SetCenterCard(int index)
        {
            BitmapImage img = null;
            Card card = null;
            if (index == -1)
            {
                img = new BitmapImage(new Uri("ms-appx:Resources/Logo.scale-100.png", UriKind.RelativeOrAbsolute));
            }
            else
            {
                img = new BitmapImage(new Uri("ms-appx:Resources/" + (index + 1) + ".png", UriKind.RelativeOrAbsolute));
                card = Deck.Instance._cards[index];
            }
            _Center_Cards[0] = card;
            _Center_Images[0].Source = img;
        }
        #endregion
        #endregion
        #endregion
    }
}
