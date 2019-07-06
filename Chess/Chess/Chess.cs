using System;
using System.Diagnostics;
using System.Threading;

namespace Chess
{

    static class Chess
    {
        private static GameMode startMode = new StartMode();
        private static GameMode normalMode = new NormalMode();
        private static GameMode promotionMode = new PromotionMode();
        private static GameMode gamemateMode = new GameMateMode();

        public static void Play()
        {
            GameMode.CalculateAllMoves();
            while (true)
            {
                switch (GameMode.GetGameMode())
                {
                    case EGameMode.START_MODE:
                        startMode.Draw();
                        startMode.WaitForKeyInput();
                        break;
                    case EGameMode.NORMAL_MODE:
                        normalMode.Draw();
                        normalMode.WaitForKeyInput();
                        break;
                    case EGameMode.PROMOTION_MODE:
                        promotionMode.Draw();
                        promotionMode.WaitForKeyInput();
                        break;
                    case EGameMode.GAME_MATE_MODE:
                        gamemateMode.Draw();
                        gamemateMode.WaitForKeyInput();
                        break;
                    case EGameMode.END_MODE:
                        Console.WriteLine();
                        Console.WriteLine("Thank you for Playing!");
                        Thread.Sleep(3000);
                        return;
                    default:
                        Debug.Assert(false, "Wrong Game Mode!");
                        break;
                }
                Thread.Sleep(100);
            }

        }
    }
}
