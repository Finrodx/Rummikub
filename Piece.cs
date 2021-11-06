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

        public bool ifSamePiece(Piece piece)
        {
            if (this.color == piece.color & this.number == piece.number)
            {
                return true;
            }
            else
                return false;
        }

        public bool ifNextConsecutivePiece(Piece piece)
        {
            if (this.color == piece.color & this.number + 1 == piece.number)
            {
                return true;
            }
            else
                return false;
        }
        public bool ifDifferentColor(Piece piece)
        {
            if (this.color != piece.color & this.number  == piece.number)
            {
                return true;
            }
            else
                return false;
        }
    }
}
