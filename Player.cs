using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rummikub
{
    class Player
    {
        public string Name;
        private List<Piece> playerHand = new List<Piece>();
        private Piece joker;
        //private List<List<Piece>> groups = new List<List<Piece>>();
        private List<Piece> jokersInHand = new List<Piece>();
        private List<Piece> fakeJokers = new List<Piece>();
        private int jokerCount;
        private int fakeJokerCount;
        private int remainingPieces;

        public Player(string Name)
        {
            this.Name = Name;
        }
        public void preparePlayerToNewGame()
        {
                remainingPieces = 0;
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

        public void organizeAndShowHand()
        {
            if(ifShouldTryDoublesFinish() == true)
            {
                //todo doublesfinish
            }
            else
            {
                List<List<Piece>> organizedHand = new List<List<Piece>>();
                int locationOfUnusedPieces = findConsecutive(playerHand).Count - 1;
                List<List<Piece>> listAfterCons = findConsecutive(playerHand);
                List<List<Piece>> listAfterColors = findColourGroups(listAfterCons[locationOfUnusedPieces]);
                
                //ardısık ve aynı renkli dizileri ilk kez bulduktan sonra bu grupların hepsini tek bir listeye toplama. son eleman kullanılmayan elemanlar.
                for (int i = 0; i < locationOfUnusedPieces; i++)
                {
                    organizedHand.Add(listAfterCons[i]);
                }
                for (int i = 0; i < listAfterColors.Count; i++)
                {
                    organizedHand.Add(listAfterColors[i]);
                }
                remainingPieces = organizedHand[organizedHand.Count-1].Count;
                showHand(organizedHand);
            }

        }

        //finds all consecutive same colored pieces and adds them to a list. last element of list contains all unused pieces
        private List<List<Piece>> findConsecutive(List <Piece> pieceList)
        {
            List<Piece> nonCons = new List<Piece>();
            bool priority = false; //if a run has missing piece between two piece, and if player has okey I am giving this case a priority
            Piece priorityPiece = null; //information of priority piece

            List<List<Piece>> listOfRuns = new List<List<Piece>>();

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

                    for (int x = 0; x < playerHand.Count; x++)
                    {
                        Piece nextPiece = playerHand[x];
                        //ignore if next piece is joker
                        if (nextPiece.ifSamePiece(joker))
                        {
                            //Console.WriteLine("sonra ki tas okey");
                            //TODO if joker is the next piece
                        }
                        // run'a ardısık elemanı ekleme
                        else
                        {
                            //if last piece of a run is a fakejoker
                            if (run[run.Count - 1].fakeJoker == true)
                            {
                                if (piece.ifNextConsecutivePiece(joker))
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
                    }
                    //if run has 2 elements we can special cases to make it 3
                    if (run.Count == 2)
                    {
                        // adding "1" to a run which has two pieces and ending with 13
                        if (run[run.Count - 1].number == 13)
                        {
                            run.Add(playerHand.FirstOrDefault(x => x.color == run[run.Count - 1].color & x.number == 1));
                        }

                        if (ifPlayerHasJoker() == true)
                        {
                            //Todo joker kullanıldı mı kontrolu yok
                            //if player has joker and there is a priority case
                            if (priority == true)
                            {
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

                    if (run.Count <= 2)
                    {
                        foreach (Piece item in run)
                        {
                            nonCons.Add(item);
                        }
                    }
                    else if (run.Count > 2)
                        listOfRuns.Add(run);
                }
                //if (run.Count > 2)
                //    listOfRuns.Add(run);
            }
            listOfRuns.Add(nonCons);
            return listOfRuns;
        }

        //finds all samenumbered different colored pieces and adds them to a list. last element of list contains all unused pieces
        private List<List<Piece>> findColourGroups(List<Piece> pieceList)
        {
            int ammountOfJokersInHand = pieceList.FindAll(x=> x.ifSamePiece(joker)).Count;
            
            List<Piece> play = new List<Piece>();
            List<List<Piece>> ColorGroupList = new List<List<Piece>>();
            List<Piece> unusedPiecesList = new List<Piece>();
            play.AddRange(pieceList);

            for (int i = 0; i < play.Count; i++)
            {
                List<Piece> colourGroup = new List<Piece>();
                Piece piece = play[i];
                if (piece.ifSamePiece(joker))
                {
                    //TODO first element joker
                }
                else
                {
                    colourGroup.Add(piece);
                    play.Remove(piece);

                    for (int x = 1; x < play.Count; x++)
                    {
                        Piece nextPiece = playerHand[x];
                        //if last piece of a colourGroup is a fakejoker
                        if (colourGroup[colourGroup.Count - 1].fakeJoker == true)
                        {
                            if (piece.ifDifferentColor(joker))
                            {
                                colourGroup.Add(nextPiece);
                                play.Remove(nextPiece);
                            }
                        }
                        //if piece is different color add it to colourGroup
                        else if (colourGroup[colourGroup.Count - 1].ifDifferentColor(nextPiece))
                        {
                            bool ifGroupHasTheColour = false;
                            foreach (Piece item in colourGroup)
                            {
                                if(item.color == nextPiece.color)
                                {
                                    ifGroupHasTheColour = true;
                                    break;
                                }
                            }
                            if(ifGroupHasTheColour != true)
                            {
                                colourGroup.Add(nextPiece);
                                play.Remove(nextPiece);
                            }
                        }
                        //if checked piece is a fakejoker
                        else if (nextPiece.fakeJoker == true)
                        {
                            //if fakejoker is a correct fit
                            if (colourGroup[colourGroup.Count - 1].ifDifferentColor(joker))
                            {
                                colourGroup.Add(nextPiece);
                                play.Remove(nextPiece);
                            }
                        }
                    }
                }
                //if colourGroup has 2 elements we can add joker to make it 3
                if (colourGroup.Count == 2)
                {
                    if (ammountOfJokersInHand > 0)
                    {
                        for (int j = 0; j < play.Count; j++)
                        {
                            if (play[i].ifSamePiece(joker))
                            {
                                colourGroup.Add(play[i]);
                                break;
                            }
                        }
                        
                    }
                }
                else if (colourGroup.Count < 3)
                {
                    foreach (Piece item in colourGroup)
                    {
                        unusedPiecesList.Add(item);
                    }
                }
                if(colourGroup.Count > 2)//colour group un 2 den fazla elemanı olması
                    ColorGroupList.Add(colourGroup);
            }
            ColorGroupList.Add(unusedPiecesList);
            return ColorGroupList;
        }

        private bool ifShouldTryDoublesFinish()
        {
            int doubleCount = 0;
            for (int j = 0; j < playerHand.Count; j++)              
            {
                Piece piece = playerHand[j];
                if (piece.ifSamePiece(joker))
                    doubleCount++;
                else
                {
                    for (int i = 1; i < playerHand.Count; i++)
                    {
                        if (i == j)
                        {
                            break;
                        }
                        else
                        {
                            if (piece.ifSamePiece(joker))
                                break;
                            else
                            {
                                if (piece.ifSamePiece(playerHand[i]))
                                    doubleCount++;
                            }
                        }
                    }                    
                }
            }
            if (doubleCount > 3)
                return true;
            return false;
        }

        public void showHand(List<List<Piece>> listofPieceLists)
        {
            Console.WriteLine("Okey: " + joker.color + " " + joker.number);
            for (int i = 0; i < listofPieceLists.Count - 1; i++)
            {
                if(i < listofPieceLists.Count )
                {
                    foreach (Piece item in listofPieceLists[i])
                    {
                        if(item.fakeJoker == true)
                            Console.WriteLine("Sahte Okey");
                        else 
                            Console.WriteLine(item.color + " " + item.number);
                    }
                }
            }
            Console.WriteLine("Kullanilmayan Taşlar:");
            foreach (Piece item in listofPieceLists[listofPieceLists.Count - 1])
            {
                if (item.fakeJoker == true)
                    Console.WriteLine("Sahte Okey");
                else
                    Console.WriteLine(item.color + " " + item.number);
            }
        }

        public int getRemainingPieces()
        {
            return remainingPieces;
        }
    }
}
