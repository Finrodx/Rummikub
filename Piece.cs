using System;
using System.Collections.Generic;
using System.Text;

namespace Rummikub
{
    class Piece
    {
        public int number { get; set; }
        public string color  { get; set; }
        public bool fakeJoker { get; set; }

        public Piece(int number, string color)
        {
            this.number = number;
            this.color = color;
            if(number == 99)
            {
                fakeJoker = true;
            }
            else
            {
                fakeJoker = false;
            }
        }

        

    }
}
