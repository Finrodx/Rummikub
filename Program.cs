using System;

namespace Rummikub
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Piece piece = new Piece(1, 'c');

            Console.WriteLine(piece.number);
        }
    }
}
