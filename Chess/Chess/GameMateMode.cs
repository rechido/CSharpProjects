using System;
using System.Diagnostics;
using System.Threading;

namespace Chess
{
    class GameMateMode : GameMode
    {
        public override void Draw()
        {
            Console.Clear();
            DrawBoard();
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            if (mBoard.State == EGameState.CHECKMATE)
            {
                if(mBoard.Turn == EPlayerType.BLACK)
                {
                    Console.WriteLine("Checkmate : WHITE is Winner!");
                }
                else
                {
                    Console.WriteLine("Checkmate : BLACK is Winner!");
                }               
            }
            else if (mBoard.State == EGameState.STALEMATE)
            {
                Console.WriteLine($"Stalemate : Draw!");
            }
            else
            {
                Debug.Assert(false, "GameMateMode cannot be run except for CHECKMATE or STALEMATE");
            }
            Console.WriteLine();
            Console.WriteLine("1: Start Menu");
        }

        public override void WaitForKeyInput()
        {
            while (true)
            {
                ConsoleKeyInfo keys;
                if (Console.KeyAvailable)
                {
                    keys = Console.ReadKey(true);
                    switch (keys.Key)
                    {
                        case ConsoleKey.D1:
                            Action1();
                            return;
                        default:
                            break;
                    }
                }
                Thread.Sleep(100);
            }
        }

        private void Action1()
        {
            SetStartMode();
        }
    }
}
