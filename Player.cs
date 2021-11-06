using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rummikub
{
    class Player
    {
        private List<Piece> playerHand = new List<Piece>();
        private Piece joker;
        private List<List<Piece>> groups = new List<List<Piece>>();
        private List<Piece> jokersInHand = new List<Piece>();
        private List<Piece> fakeJokers = new List<Piece>();
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
            organizeHand();
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

        private void organizeHand()
        {

            findConsecutive(playerHand);
        }

        private void findConsecutive(List <Piece> pieceList)
        {
            Console.WriteLine("\n \n \n PLAYER \n");
            bool priority = false; //if a run has missing piece between two piece, and if player has okey I am giving this case a priority
            Piece priorityPiece = null; //information of priority piece

            List<Piece> play = new List<Piece>();
            play.AddRange(pieceList);

            for (int i = 0; i < play.Count; i++)
            {
                Piece piece = play[i];
                List<Piece> run = new List<Piece>();
                if (piece.ifSamePiece(joker))
                {
                    //TODO first element joker
                }
                else
                {
                    run.Add(piece);
                    play.Remove(piece);

                    for (int x = 0; x < play.Count; x++)
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
                            //if piece is consecutive add it to run
                            else if (run[run.Count - 1].ifNextConsecutivePiece(nextPiece))
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
                            //detecting priorty case
                            else if (run[run.Count - 1].number + 2 == nextPiece.number & ifPlayerHasJoker())
                            {
                                priority = true;
                                priorityPiece = nextPiece;
                            }
                        }
                        Console.WriteLine("Run eleman sayısı: " + run.Count);
                        foreach (Piece item in run)
                        {
                            Console.WriteLine("Run da islem yapılmadan once: " + item.color + " " + item.number);
                        }
                    }
                    //if run has 2 elements we can special cases to make it 3
                    if (run.Count == 2)
                    {
                        // adding "1" to a run which has two pieces and ending with 13
                        if (run[run.Count - 1].number == 13)
                        {
                            run.Add(playerHand.FirstOrDefault(x => x.color == run[run.Count - 1].color & x.number == 1));
                            Console.WriteLine("run da 2 eleman var ve 13-1 ozel durumu var ");
                        }

                        if (ifPlayerHasJoker() == true)
                        {
                            //Todo joker kullanıldı mı kontrolu yok
                            Console.WriteLine("run da 2 eleman var ve player da okey var ");
                            //if player has joker and there is a priority case
                            if (priority == true)
                            {
                                Console.WriteLine("run da 2 eleman var, okey var ve oncelikli durum var");
                                Piece jokerInHand = play.First(x => x.ifSamePiece(joker));
                                run.Add(jokerInHand);
                                play.Remove(jokerInHand);
                                run.Add(priorityPiece);
                                play.Remove(priorityPiece);

                                priorityPiece = null;
                                priority = false;
                            }
                        }
                    }

                    else if (run.Count <= 2)
                    {
                        Console.WriteLine("Runda silinenler. 2 veya daha az elemanlı olmalı");
                        foreach (Piece item in run)
                        {
                            Console.WriteLine(item.color + " " + item.number);
                        }

                        run.Clear();
                    }
                    else if (run.Count > 2)
                    {
                        Console.WriteLine("Gruba Eklenen Run. 2'den daha cokelemanlı olmalı");
                        foreach (Piece item in run)
                        {
                            Console.WriteLine(item.color + " " + item.number);
                        }
                        Console.WriteLine("Run eleman sayısı: " + run.Count);
                        groups.Add(run);
                        Console.WriteLine("Grup eleman sayısı:" + groups.Count);
                    }
                }

            }
        }
        private void findColourGroups(List<Piece> pieceList)
        {
            List<Piece> play = new List<Piece>();
            List<Piece> colourGroup = new List<Piece>();
            List<List<Piece>> Group = new List<List<Piece>>();
            play.AddRange(pieceList);
            
            for (int i = 0; i < play.Count; i++)
            {
                colourGroup.Add(play[i]);
                Piece piece = play[i];
                for (int x = 1; x < play.Count; x++)
                {
                    if (piece.ifDifferentColor(play[x]))
                    {
                        colourGroup.Add(play[x]);
                        play.Remove(play[x]);
                    }
                }

            }
        }
    }
}
