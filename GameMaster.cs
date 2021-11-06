﻿using System;
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
        List<Player> players = new List<Player>() {new Player(), new Player(), new Player(), new Player()};
        Random rnd = new Random();

        public void PrepareNewGame()
        {
            ShufllePlayers();
            CreatePieces();
            ShufflePieces();
            CreateJokerPiece();
            DealPieces();
            sendJokerInfoToPlayers();
            playersOrganizePieces();
        }

        private void ShufllePlayers()
        {
            int random = rnd.Next(0, 4);

            Player luckyPlayer = players[random];
            players[random] = players[0];
            players[0] = luckyPlayer;

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
            //removing one of the indicator piece since it wont be in game
            pieces.Remove(indicatorPiece);
            if(indicatorPiece.number != 13)
            {
                foreach (Piece jokerPiece in pieces.FindAll(x => x.number == indicatorPiece.number + 1 & x.color == indicatorPiece.color))
                {
                    jokers.Add(jokerPiece);
                }
            }
            //if indicator is 13. 1 is our joker
            else if(indicatorPiece.number == 13)
            {
                foreach (Piece jokerPiece in pieces.FindAll(x => x.number == 1 & x.color == indicatorPiece.color))
                {
                    jokers.Add(jokerPiece);
                }
            }
        }

        private void DealPieces()
        {
            // deal 3 times
            for (int dealNo = 0; dealNo < 3; dealNo++)
            {
                // 4 players 
                for (int playerNo = 0; playerNo < 4; playerNo++)
                {
                    // deal 5 piece each time
                    for (int pieceNo = 0; pieceNo < 5; pieceNo++)
                    {
                        if(dealNo != 2)
                        {
                            players[playerNo].getPlayersHand().Add(pieces[0]);
                            pieces.RemoveAt(0);
                        }
                        else if (dealNo == 2 & playerNo == 0)
                        {
                            players[0].getPlayersHand().Add(pieces[0]);
                            pieces.RemoveAt(0);
                        }
                        else if (dealNo == 2 & playerNo != 0)
                        {
                            if(pieceNo == 4)
                            {
                                pieceNo = 5;
                            }
                            else
                            {
                                players[playerNo].getPlayersHand().Add(pieces[0]);
                                pieces.RemoveAt(0);
                            }
                        }
                    }
                }
            }
        }
        public List<Piece> GetPieces()
        {
            return pieces;
        }

        public void printPlayerHands()
        {
            foreach (Player player in players)
            {
                Console.WriteLine("\n Player: \n");
                foreach (Piece piece in player.getPlayersHand())
                {
                    if (jokers.Contains(piece))
                    {
                        Console.WriteLine(piece.color + " " + piece.number + " " + "okey");
                    }
                    else
                    Console.WriteLine(piece.color + " " + piece.number);
                }              
            }
        }
        public void sendJokerInfoToPlayers()
        {
            foreach (Player player in players)
            {
                player.newGameJokerInfoToPlayers(jokers[0]);
            }
        }

        public void playersOrganizePieces()
        {
            foreach (Player player in players)
            {
                player.preparePlayerToNewGame();
            }
        }
    }
}
