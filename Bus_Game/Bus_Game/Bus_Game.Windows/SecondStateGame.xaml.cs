using Bus_Game.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class SecondStateGame : Page
    {
        private int _AmountOfRows = 4;
        private StackPanel[] _centerStackPanels = null;
        private Tuple<bool, int> _GameInformation = null;
        private Card[] _PlayerDown_Card = null, _PlayerTop_Card = null, _PlayerLeft_Card = null, _PlayerRight_Card = null;
        private Image[] _PlayerDown_Images = null, _PlayerTop_Images = null, _PlayerLeft_Images = null, _PlayerRight_Images = null;
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public SecondStateGame()
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
            InitCenterGrid();
            InitImages();
            InitPanels();
        }
        #endregion
        #region Init Center Grid
        private void InitCenterGrid()
        {
            ColumnDefinition cd = null;
            this._centerStackPanels = new StackPanel[_AmountOfRows];
            for (int i = 0; i < _AmountOfRows; i++)
            {
                cd = new ColumnDefinition();
                cd.Width = new GridLength(150);
                _Center_Grid.ColumnDefinitions.Add(cd);
            }
            for (int i = 0; i < _AmountOfRows; i++)
            {
                _centerStackPanels[i] = new StackPanel();
                _centerStackPanels[i].Orientation = Orientation.Vertical;
                _centerStackPanels[i].HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                _centerStackPanels[i].VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
                _Center_Grid.Children.Add(_centerStackPanels[i]);
            }
            InitCenterCards();
        }
        private void InitCenterCards()
        {
            _centerStackPanels[0].Children.Add(CreateCard());
            _centerStackPanels[1].Children.Add(CreateCard());
            _centerStackPanels[1].Children.Add(CreateCard());
        }
        private Image CreateCard()
        {
            Image img = new Image();
            img.Width = 100;
            img.Height = 100;
            img.Stretch = Stretch.Fill;
            img.Source = new BitmapImage(new Uri("ms-appx:Resources/Logo.scale-100.png"));
            return img;
        }
        #endregion
        #region Init Images
        private void InitImages()
        {
            this._PlayerDown_Images = new Image[4];
            this._PlayerLeft_Images = new Image[4];
            this._PlayerTop_Images = new Image[4];
            this._PlayerRight_Images = new Image[4];
            InitImageArray(_PlayerDown_Card, _PlayerDown_Images);
            InitImageArray(_PlayerLeft_Card, _PlayerLeft_Images);
            InitImageArray(_PlayerTop_Card, _PlayerTop_Images);
            InitImageArray(_PlayerRight_Card, _PlayerRight_Images);
        }
        private void InitImageArray(Card[] cards, Image[] images)
        {
            if (cards != null)
            {
                for (int i = 0; i < cards.Length; i++)
                {
                    Debug.WriteLine("Loading " + cards[i].Index + ".png");
                    images[i] = new Image();
                    images[i].Width = 150;
                    images[i].Height = 150;
                    images[i].Margin = new Thickness(10, 10, 10, 10);
                    images[i].Stretch = Stretch.Fill;
                    images[i].Source = LoadImageIndex(cards[i].Index);
                }
            }
        }
        private BitmapImage LoadImageIndex(int index)
        { return LoadImageString(index.ToString()); }
        private BitmapImage LoadImageString(string name)
        { return new BitmapImage(new Uri("ms-appx:Resources/" + name + ".png")); }
        #endregion
        #region InitPanels
        private void InitPanels()
        {
            InitPanel(_PlayerDown_Panel, _PlayerDown_Images);
            InitPanel(_PlayerLeft_Panel, _PlayerLeft_Images);
            InitPanel(_PlayerTop_Panel, _PlayerTop_Images);
            InitPanel(_PlayerRight_Panel, _PlayerRight_Images);
        }
        private void InitPanel(StackPanel sp, Image[] images)
        {
            if (images[0] != null)
                foreach (Image img in images)
                { sp.Children.Add(img); }
        }
        #endregion
        #endregion

        #region NavigationHelper registration
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        { Init(); }
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        { }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            #region Init Second State
            Tuple<Tuple<Tuple<Card[], Card[]>, Tuple<Card[], Card[]>>, Tuple<bool, int>> information = e.Parameter as Tuple<Tuple<Tuple<Card[], Card[]>, Tuple<Card[], Card[]>>, Tuple<bool, int>>;
            _GameInformation = information.Item2;
            #region 2 Players
            if (_GameInformation.Item2 == 1)
            {
                _PlayerDown_Card = information.Item1.Item1.Item1;
                _PlayerTop_Card = information.Item1.Item1.Item2;
            }
            #endregion
            #region 3 Players
            if (_GameInformation.Item2 == 2)
            {
                _PlayerDown_Card = information.Item1.Item1.Item1;
                _PlayerLeft_Card = information.Item1.Item1.Item2;
                _PlayerTop_Card = information.Item1.Item2.Item1;
            }
            #endregion
            #region 4 Players
            if (_GameInformation.Item2 == 3)
            {
                _PlayerDown_Card = information.Item1.Item1.Item1;
                _PlayerLeft_Card = information.Item1.Item1.Item2;
                _PlayerTop_Card = information.Item1.Item2.Item1;
                _PlayerRight_Card = information.Item1.Item2.Item2;
            }
            #endregion
            #endregion
            navigationHelper.OnNavigatedTo(e);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this._GameInformation = (Tuple<bool, int>)(e.Parameter);
            navigationHelper.OnNavigatedFrom(e);
        }
        public ObservableDictionary DefaultViewModel
        { get { return this.defaultViewModel; } }
        public NavigationHelper NavigationHelper
        { get { return this.navigationHelper; } }
        #endregion
    }
}
