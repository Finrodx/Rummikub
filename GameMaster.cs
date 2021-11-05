using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace Rummikub
{
    class GameMaster
    {  
        string[] colors = { "yellow", "blue", "black", "red" };
        List<Piece> pieces = new List<Piece>();
        List<Piece> indicators = new List<Piece>();
        List<Piece> jokers = new List<Piece>();

        public void prepareNewGame()
        {
            CreatePieces();
            ShufflePieces();
            CreateJokerPiece();
        }

        public List<Piece> CreatePieces()
        {
            foreach (string color in colors)
            {
                for (int i = 1; i < 14; i++)
                {
                    Piece piece = new Piece(i, color);
                    pieces.Add(piece);
                }
            }
            pieces.Add(new Piece(99, "black"));
            pieces.AddRange(pieces);
            return pieces;
        }

        public void ShufflePieces()
        {
            Shuffle(pieces);
        }

        // Fisher-Yates shuffle
        private void Shuffle(List<Piece> pieces)
        {
            Random rnd = new Random();
            int n = pieces.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                Piece piece = pieces[k];
                pieces[k] = pieces[n];
                pieces[n] = piece;
            }
        }

        public void CreateJokerPiece()
        {
            Random rnd = new Random();
            int random = rnd.Next(0, 106);

            Piece indicatorPiece;
            while (true)
            {
                indicatorPiece = pieces[random];
                if (indicatorPiece.fakeJoker != true)
                {
                    break;
                }
            }
            pieces.Remove(indicatorPiece);

            foreach (Piece jokerPiece in pieces.FindAll(x => x.number == indicatorPiece.number + 1 & x.color == indicatorPiece.color))
            {
                jokers.Add(jokerPiece);
            }
        }

        public List<Piece> GetPieces()
        {
            return pieces;
        }
    }
}
