using System;
using System.Threading;

namespace Chess
{
    class StartMode : GameMode
    {
        public override void Draw()
        {
            Console.Clear();
            Console.WriteLine("1: Start Game");
            Console.WriteLine("2: End Game");
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
                        case ConsoleKey.D2:
                            Action2();
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
            SetNormalMode();
        }
        private void Action2()
        {
            SetEndMode();
        }
    }
}
