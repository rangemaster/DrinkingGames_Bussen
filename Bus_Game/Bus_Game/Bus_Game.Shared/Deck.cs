using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Bus_Game
{
    public class Deck
    {
        private static Deck _Instance = null;
        private static object pathLock = new object();
        public readonly Card[] _cards;
        private Deck()
        {
            this._cards = new Card[52];
            Init();
        }
        public static Deck Instance
        {
            get
            {
                lock (pathLock)
                {
                    if (_Instance == null)
                        _Instance = new Deck();
                    return _Instance;
                }
            }
        }
        private void Init()
        {
            string[] kinds = new string[] { "Diamand", "Hart", "Spade", "Clubs" };
            for (int i = 0; i < _cards.Length; i++)
            {
                int value = (i % 13) + 2;
                string kind = kinds[(i / 13)];
                Debug.WriteLine("Value: " + i + ", " + value + ", " + kind);
                _cards[i] = new Card(i+1, value, kind);
            }
        }
    }
}
