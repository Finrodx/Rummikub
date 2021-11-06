using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rummikub
{
    class Player
    {
        private List<Piece> playerHand = new List<Piece>();
        private List<Piece> play = new List<Piece>();
        private Piece joker;
        private List<List<Piece>> groups = new List<List<Piece>>();
        private List<Piece> jokersInHand;
        private List<Piece> fakeJokers;
        private int jokerCount;
        private int fakeJokerCount;

        public void preparePlayerToNewGame()
        {
            groups.Clear();
            if (ifPlayerHasJoker())
                howManyJokersPlayerHas();
            if (ifPlayerHasFakeJoker())
                howManyFakeJokersPlayerHas();            
            sortHand();
        }


        private bool ifPlayerHasJoker()
        {
            jokersInHand.Clear();
            jokerCount = 0;
            if (playerHand.Contains(joker))
                return true;                            
            return false;
        }

        private int howManyJokersPlayerHas()
        {
            jokersInHand = playerHand.FindAll(x => x.number == joker.number & x.color == joker.color);
            jokerCount = jokersInHand.Count();
            return jokerCount;
        }

        private bool ifPlayerHasFakeJoker()
        {
            foreach (Piece piece in playerHand)
            {
                if (piece.fakeJoker == true)
                {
                    return true;
                }
            }
            return false;
        }

        private int howManyFakeJokersPlayerHas()
        {
            fakeJokerCount = 0;
            foreach (Piece piece in playerHand)
            {
                if (piece.fakeJoker == true)
                {
                    fakeJokerCount += 1;
                }
            }
            return fakeJokerCount;
        }

        private void sortHand()
        {
            playerHand = playerHand.OrderBy(x => x.number).ToList();
        }

        public void newGameJokerInfoToPlayers(Piece joker)
        {
            this.joker = joker;
        }

        public List<Piece> getPlayersHand()
        {
            return playerHand;
        }

        public void organizeHand()
        {

        }

        private void findConsecutive()
        {
            bool priority = false; //if a run has missing piece between two piece, and if player has okey I am giving this case a priority
            Piece priorityPiece; //information of priority piece

            play.AddRange(playerHand);

            for (int i = 0; i < play.Count; i++)
            {
                List<Piece> run = new List<Piece>();
                Piece piece = play[i];
                run.Add(piece);
                play.Remove(piece);

                for (int x = i; x < play.Count; x++)
                {
                    Piece nextPiece = playerHand[x];
                    //ignore if next piece is joker
                    if (nextPiece.ifSamePiece(joker))
                    {
                        //TODO if joker is the next piece
                    }
                    else
                    {
                        //if last piece of a run is a fakejoker
                        if (run[run.Count - 1].fakeJoker == true)
                        {
                            if (piece.ifNextConsecutivePiece(nextPiece))
                            {
                                run.Add(nextPiece);
                                play.Remove(nextPiece);
                            }
                        }

                        else if (piece.ifNextConsecutivePiece(nextPiece))
                        {
                            run.Add(nextPiece);
                            play.Remove(nextPiece);
                        }
                        //if checked piece is a fakejoker
                        else if (nextPiece.fakeJoker == true)
                        {
                            //if fakejoker is a correct fit
                            if (run[run.Count - 1].ifNextConsecutivePiece(joker))
                            {
                                run.Add(nextPiece);
                                play.Remove(nextPiece);
                            }
                        }
                        else if(run[run.Count-1].number + 2 == nextPiece.number)
                        {
                            priority = true;
                            priorityPiece = nextPiece;
                        }
                    }

                }
                //if run has 2 elements we can special cases to make it 3
                if (run.Count == 2)
                {
                    // adding "1" to a run which has two pieces and ending with 13
                    if (run[run.Count - 1].number == 13)
                        run.Add(playerHand.FirstOrDefault(x => x.color == run[run.Count - 1].color & x.number == 1));
                    if(ifPlayerHasJoker() == true)
                    {

                    }
                }
                else if(run.Count <= 2)
                {
                    run.Clear();
                }
                else if (run.Count > 2)
                {
                    groups.Add(run);
                }
            }
        }
    }
}
