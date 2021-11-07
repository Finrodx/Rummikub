using System;
using System.Collections.Generic;

namespace Rummikub
{
    class Program
    {

        static void Main(string[] args)
        {
            GameMaster gm = new GameMaster();
            gm.PrepareNewGame();
            gm.printNotOrganizedPlayerHands();
            gm.printOrganizedPlayerHands();
            gm.findClosestToWin();

        }
    }
}
