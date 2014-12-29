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
            this._AmountOfPlayers_cb.IsEnabled = true;
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
        #region Skip
        private void _Skip_To_Second_bn_Click(object sender, RoutedEventArgs e)
        {
            Tuple<Card[], Card[]> players1and2 = null;
            Tuple<Card[], Card[]> players3and4 = null;
            #region 2 Players
            if (_AmountOfPlayers_cb.SelectedIndex + 1 == 1)
            {
                players1and2 = new Tuple<Card[], Card[]>(GenerateCardArray(), GenerateCardArray());
                players3and4 = new Tuple<Card[], Card[]>(null, null);
            }
            #endregion
            #region 3 Players
            if (_AmountOfPlayers_cb.SelectedIndex + 1 == 2)
            {
                players1and2 = new Tuple<Card[], Card[]>(GenerateCardArray(), GenerateCardArray());
                players3and4 = new Tuple<Card[], Card[]>(GenerateCardArray(), null);
            }
            #endregion
            #region 4 Players
            if (_AmountOfPlayers_cb.SelectedIndex + 1 == 3)
            {
                players1and2 = new Tuple<Card[], Card[]>(GenerateCardArray(), GenerateCardArray());
                players3and4 = new Tuple<Card[], Card[]>(GenerateCardArray(), GenerateCardArray());
            }
            #endregion
            Tuple<Tuple<Card[], Card[]>, Tuple<Card[], Card[]>> players = new Tuple<Tuple<Card[], Card[]>, Tuple<Card[], Card[]>>(players1and2, players3and4);
            Tuple<bool, int> gameInfo = new Tuple<bool, int>((bool)_Bots_rb.IsChecked, _AmountOfPlayers_cb.SelectedIndex + 1);
            Tuple<Tuple<Tuple<Card[], Card[]>, Tuple<Card[], Card[]>>, Tuple<bool, int>> information;
            information = new Tuple<Tuple<Tuple<Card[], Card[]>, Tuple<Card[], Card[]>>, Tuple<bool, int>>(players, gameInfo);
            this.Frame.Navigate(typeof(SecondStateGame), information);
        }
        #region Generate Card Array
        private Card[] GenerateCardArray()
        {
            Card[] array = new Card[4];
            Random random = new Random();
            for (int i = 0; i < array.Length; i++)
            {
                int index = 0;
                bool found = false;
                while (!found || index < Deck.Instance._cards.Length)
                {
                    Debug.WriteLine("Length: " + Deck.Instance._cards.Length + ", " + i + ", " + index + ", " + Deck.Instance._cards[index].Taken());
                    found = !Deck.Instance._cards[index].Taken();
                    if (found)
                    {
                        array[i] = Deck.Instance._cards[index];
                        Deck.Instance._cards[index].Taken(true);
                        break;
                    }
                    else { index++; }
                }
                if (!found)
                {
                    Debug.WriteLine("GenerateCardArray(), Could not find an avaible card.");
                }
            }
            return array;
        }
        #endregion
        #endregion
    }
}
