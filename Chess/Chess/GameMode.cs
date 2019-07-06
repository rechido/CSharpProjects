using System;
using System.Collections.Generic;

namespace Chess
{
    enum EGameMode
    {
        START_MODE,
        NORMAL_MODE,
        PROMOTION_MODE,
        GAME_MATE_MODE,
        END_MODE
    }

    abstract class GameMode
    {
        protected static Board mBoard = new Board();
        protected static EGameMode mGameMode = EGameMode.START_MODE;
        protected static int mX = 0; // 0~7
        protected static int mY = 0; // 0~7
        protected static int mGrepX = 0; // 0~7
        protected static int mGrepY = 0; // 0~7
        protected static bool mGrep = false;

        public abstract void Draw();
        protected static void DrawBoard()
        {
            Piece grep;
            List<Move> moves = new List<Move>();
            if (mGrep)
            {
                grep = mBoard.GetPieceOrNull(mGrepX, mGrepY);
                moves = grep.PossibleMoves;
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    // background coloring
                    if (i == mY && j == mX)
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                    }
                    else if (mGrep && i == mGrepY && j == mGrepX)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        bool possibleMove = false;
                        foreach (Move move in moves)
                        {
                            if (move == null) continue;
                            if (i == move.Y && j == move.X)
                            {
                                Console.BackgroundColor = ConsoleColor.Green;
                                possibleMove = true;
                                break;
                            }
                        }
                        if (!possibleMove)
                        {
                            if (i % 2 == 0)
                            {
                                if (j % 2 == 0)
                                {
                                    Console.BackgroundColor = ConsoleColor.Cyan;
                                }
                                else
                                {
                                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                                }
                            }
                            else
                            {
                                if (j % 2 == 0)
                                {
                                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                                }
                                else
                                {
                                    Console.BackgroundColor = ConsoleColor.Cyan;
                                }
                            }
                        }
                    }

                    // character coloring
                    Piece piece = mBoard.GetPieceOrNull(j, i);
                    if (piece == null)
                    {
                        Console.Write("   ");
                    }
                    else
                    {
                        if (piece.Player == EPlayerType.WHITE)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        string name = "";
                        switch (piece.Type)
                        {
                            case EPieceType.PAWN:
                                name = "P";
                                break;
                            case EPieceType.ROOK:
                                name = "R";
                                break;
                            case EPieceType.KNIGHT:
                                name = "N";
                                break;
                            case EPieceType.BISHOP:
                                name = "B";
                                break;
                            case EPieceType.QUEEN:
                                name = "Q";
                                break;
                            case EPieceType.KING:
                                name = "K";
                                break;
                        }
                        Console.Write($" {name} ");
                    }
                }
                Console.WriteLine();
            }
        }

        public abstract void WaitForKeyInput();

        public static EGameMode GetGameMode()
        {
            return mGameMode;
        }

        public static EGameState GetGameState()
        {
            return mBoard.State;
        }

        public static void SetStartMode()
        {
            mGameMode = EGameMode.START_MODE;
        }

        public static void SetNormalMode()
        {
            mGameMode = EGameMode.NORMAL_MODE;
        }

        public static void SetPromotionMode()
        {
            mGameMode = EGameMode.PROMOTION_MODE;
        }

        public static void SetGameMateMode()
        {
            mGameMode = EGameMode.GAME_MATE_MODE;
        }

        public static void SetEndMode()
        {
            mGameMode = EGameMode.END_MODE;
        }

        public static void CalculateAllMoves()
        {
            mBoard.CalculateMoves();
        }

        public static void PutPiece(int x, int y, EPieceType piece, EPlayerType player)
        {
            mBoard.PutPiece(x, y, piece, player);
        }
    }
}
