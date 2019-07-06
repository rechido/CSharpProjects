using System;
using System.Threading;

namespace Chess
{
    class PromotionMode : GameMode
    {
        public override void Draw()
        {
            Console.Clear();
            DrawBoard();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("*********");
            Console.WriteLine("Promotion");
            Console.WriteLine("*********");
            Console.WriteLine();
            Console.WriteLine("R: Rook");
            Console.WriteLine("N: Knight");
            Console.WriteLine("B: Bishop");
            Console.WriteLine("Q: Queen");
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
                        case ConsoleKey.R:
                            ActionR();
                            return;
                        case ConsoleKey.N:
                            ActionN();
                            return;
                        case ConsoleKey.B:
                            ActionB();
                            return;
                        case ConsoleKey.Q:
                            ActionQ();
                            return;
                        default:
                            break;
                    }
                }
                Thread.Sleep(100);
            }
        }

        private void ActionR()
        {
            mBoard.Promote(EPieceType.ROOK);
            SetNormalMode();
        }

        private void ActionN()
        {
            mBoard.Promote(EPieceType.KNIGHT);
            SetNormalMode();
        }

        private void ActionB()
        {
            mBoard.Promote(EPieceType.BISHOP);
            SetNormalMode();
        }

        private void ActionQ()
        {
            mBoard.Promote(EPieceType.QUEEN);
            SetNormalMode();
        }
    }
}
