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
    public sealed partial class FirstStateGame : Page
    {
        private Tuple<bool, int> _GameInformation = null;
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private Image[] _PlayerDown_Images, _PlayerLeft_Images, _PlayerTop_Images, _PlayerRight_Images, _CenterImages;
        public FirstStateGame()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }
        private void Init()
        {
            InitImageArrays();
            _Down_Panel.Orientation = Orientation.Horizontal;
            _Top_Panel.Orientation = Orientation.Horizontal;
            _Center_Panel.Orientation = Orientation.Horizontal;
            initPanel(_Down_Panel, _PlayerDown_Images);
            if (_GameInformation.Item2 >= 1)
                initPanel(_Top_Panel, _PlayerTop_Images);
            if (_GameInformation.Item2 >= 2)
                initPanel(_Left_Panel, _PlayerLeft_Images);
            if (_GameInformation.Item2 >= 3)
                initPanel(_Right_Panel, _PlayerRight_Images);
            initPanel(_Center_Panel, _CenterImages);
        }
        #region Initialisation panel and images
        private void InitImageArrays()
        {
            _PlayerDown_Images = new Image[4];
            _PlayerTop_Images = new Image[4];
            _PlayerLeft_Images = new Image[4];
            _PlayerRight_Images = new Image[4];
            _CenterImages = new Image[1];
            _CenterImages[0] = new Image();
            InitImage(_CenterImages[0]);
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
        private void InitDownPanel()
        {
            this._Down_Panel.Orientation = Orientation.Horizontal;
            this._Down_Panel.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            this._Down_Panel.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            foreach (Image img in _PlayerDown_Images)
                _Down_Panel.Children.Add(img);
        }
        private void InitTopPanel()
        {
            this._Top_Panel.Orientation = Orientation.Horizontal;
            this._Top_Panel.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            this._Top_Panel.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            foreach (Image img in _PlayerTop_Images)
                _Top_Panel.Children.Add(img);
        }
        private void initPanel(StackPanel panel, Image[] images)
        {
            panel.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            panel.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            foreach (Image img in images)
                panel.Children.Add(img);

        }
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
    }
}
