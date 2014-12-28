using Bus_Game.Common;
using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Bus_Game
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class SecondStateGame : Page
    {
        private Tuple<bool, int> _GameInformation = null;
        private Card[] _PlayerDown_Card = null, _PlayerTop_Card = null, _PlayerLeft_Card = null, _PlayerRight_Card = null;
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

        }
        #endregion
        #endregion

        #region NavigationHelper registration
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        { }
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        { }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
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
