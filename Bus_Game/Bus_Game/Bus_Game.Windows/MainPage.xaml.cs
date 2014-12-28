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
    public sealed partial class MainPage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public MainPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            Init();
        }
        private void Init()
        {
            this._Bots_rb.IsChecked = true;
            this._Username_tx.Text = "Admin";
            this._AmountOfPlayers_cb.Items.Add("1");
            this._AmountOfPlayers_cb.Items.Add("2");
            this._AmountOfPlayers_cb.Items.Add("3");
            this._AmountOfPlayers_cb.SelectedIndex = 0;
            EnabledSettings();
        }
        #region Enabled
        private void EnabledSettings()
        {
            this._Username_tx.IsEnabled = false;
            this._Players_rb.IsEnabled = false;
            this._AmountOfPlayers_cb.IsEnabled = false;
        }
        #endregion

        #region NavigationHelper registration
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        { navigationHelper.OnNavigatedTo(e); }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        { navigationHelper.OnNavigatedFrom(e); }
        public ObservableDictionary DefaultViewModel
        { get { return this.defaultViewModel; } }
        public NavigationHelper NavigationHelper
        { get { return this.navigationHelper; } }
        #endregion

        private void _Start_bn_Click(object sender, RoutedEventArgs e)
        {
            bool bots = (bool)_Bots_rb.IsChecked;
            int amount = (int)_AmountOfPlayers_cb.SelectedIndex;
            Tuple<bool, int> info = new Tuple<bool, int>(bots, amount + 1);
            this.Frame.Navigate(typeof(FirstStateGame), info);
        }
    }
}
