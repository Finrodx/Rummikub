using System;
using System.Collections.Generic;
using System.Text;

namespace Rummikub
{
    class Piece
    {
        public int number { get; set; }
        private char color  { get; set; }

    public Piece(int number, char color)
        {
            this.number = number;
            this.color = color;
        }


    }
}
