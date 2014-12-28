using System;
using System.Collections.Generic;
using System.Text;

namespace Bus_Game
{
    public class Card
    {
        private readonly int _Index, _Value;
        private readonly string _Name = "";
        private bool _Taken;
        public Card(int index, int value, string name)
        {
            this._Index = index;
            this._Value = value;
            this._Name = name;
            this._Taken = false;
        }
        public int Index { get { return _Index; } }
        public int Value { get { return _Value; } }
        public string Name { get { return _Name; } }
        public bool Taken() { return _Taken; }
        public void Taken(bool taken) { this._Taken = taken; }
    }
}
