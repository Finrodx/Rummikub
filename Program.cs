using System;
using System.Collections.Generic;

namespace Rummikub
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            GameMaster gm = new GameMaster();
            gm.PrepareNewGame();
            gm.getPlayerHands();


        }
    }
}
