using System;
using System.Diagnostics;
using System.Threading;

namespace Chess
{
    class NormalMode : GameMode
    {
        public override void Draw()
        {
            Console.Clear();
            DrawBoard();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine($"Turn: {mBoard.Turn}");
            Console.WriteLine();
            if (mBoard.State == EGameState.CHECK)
            {
                Console.WriteLine("CHECK");
            }
            Console.WriteLine();
            Console.WriteLine("LeftArrow: Left move");
            Console.WriteLine("RightArrow: Right move");
            Console.WriteLine("UpArrow: Up move");
            Console.WriteLine("DownArrow: Down move");
            Console.WriteLine("Escape: Cancel Grep");
            Console.WriteLine("Enter: Grep or Put");


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
                        case ConsoleKey.LeftArrow:
                            ActionLeft();
                            return;
                        case ConsoleKey.RightArrow:
                            ActionRight();
                            return;
                        case ConsoleKey.UpArrow:
                            ActionUp();
                            return;
                        case ConsoleKey.DownArrow:
                            ActionDown();
                            return;
                        case ConsoleKey.Escape:
                            ActionEscape();
                            return;
                        case ConsoleKey.Enter:
                            ActionEnter();
                            return;
                        default:
                            break;
                    }
                }
                Thread.Sleep(100);
            }
        }

        private void ActionLeft()
        {
            if (mX > 0)
            {
                mX--;
            }
        }
        private void ActionRight()
        {
            if (mX < 7)
            {
                mX++;
            }
        }
        private void ActionUp()
        {
            if (mY > 0)
            {
                mY--;
            }
        }
        private void ActionDown()
        {
            if (mY < 7)
            {
                mY++;
            }
        }
        private void ActionEscape()
        {
            if (mGrep == true)
            {
                mGrep = false;
            }
        }
        private void ActionEnter()
        {
            if (mGrep == false)
            {
                Piece piece = mBoard.GetPieceOrNull(mX, mY);
                if (piece != null)
                {
                    if (piece.Player == mBoard.Turn)
                    {
                        mGrepX = mX;
                        mGrepY = mY;
                        mGrep = true;
                    }
                }
            }
            else
            {
                Piece grep = mBoard.GetPieceOrNull(mGrepX, mGrepY);
                Debug.Assert(grep != null, "Grep must not be null");

                Move move = mBoard.GetMoveInPossibleMovesOrNull(mX, mY, grep);
                if (move != null)
                {
                    switch (move.Type)
                    {
                        case EMoveType.NORMAL_MOVE:
                            if (!mBoard.SimulateIfCheckWhenGoToThere(grep.Player, mGrepX, mGrepY, mX, mY))
                            {
                                mBoard.NormalMove(mGrepX, mGrepY, mX, mY);
                                mGrep = false;
                                mBoard.ToggleTurn();
                                mBoard.CalculateMoves();
                                mBoard.SetGameState();
                                if (mBoard.State == EGameState.PROMOTION)
                                {
                                    SetPromotionMode();
                                }
                                else if (mBoard.State == EGameState.CHECKMATE || mBoard.State == EGameState.STALEMATE)
                                {
                                    SetGameMateMode();
                                }
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("Cannot go there. Check if you go there!");
                            }
                            break;

                        case EMoveType.PAWN_DOUBLE_MOVE:
                            if (!mBoard.SimulateIfCheckWhenGoToThere(grep.Player, mGrepX, mGrepY, mX, mY))
                            {
                                mBoard.PawnDoubleMove(mGrepX, mGrepY, mX, mY);
                                mGrep = false;
                                mBoard.ToggleTurn();
                                mBoard.CalculateMoves();
                                mBoard.SetGameState();
                                if (mBoard.State == EGameState.PROMOTION)
                                {
                                    SetPromotionMode();
                                }
                                else if (mBoard.State == EGameState.CHECKMATE || mBoard.State == EGameState.STALEMATE)
                                {
                                    SetGameMateMode();
                                }
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("Cannot go there. Check if you go there!");
                            }
                            break;

                        case EMoveType.KING_CASTLING:
                            if(mGrepX < mX)
                            {
                                for(int i=mGrepX + 1; i<=mX; i++)
                                {
                                    if(mBoard.SimulateIfCheckWhenGoToThere(grep.Player, mGrepX, mGrepY, i, mY))
                                    {
                                        Console.WriteLine();
                                        Console.WriteLine("Cannot do Castling now. The way is under attack!");
                                        return;
                                    }
                                }
                                mBoard.CastlingMove(grep.Player, 7);
                                mGrep = false;
                                mBoard.ToggleTurn();
                                mBoard.CalculateMoves();
                                mBoard.SetGameState();
                                if (mBoard.State == EGameState.PROMOTION)
                                {
                                    SetPromotionMode();
                                }
                                else if (mBoard.State == EGameState.CHECKMATE || mBoard.State == EGameState.STALEMATE)
                                {
                                    SetGameMateMode();
                                }
                            }
                            else
                            {
                                for (int i = mGrepX - 1; i >= mX; i--)
                                {
                                    if (mBoard.SimulateIfCheckWhenGoToThere(grep.Player, mGrepX, mGrepY, i, mY))
                                    {
                                        Console.WriteLine();
                                        Console.WriteLine("Cannot do Castling now. The way is under attack!");
                                        return;
                                    }
                                }
                                mBoard.CastlingMove(grep.Player, 0);
                                mGrep = false;
                                mBoard.ToggleTurn();
                                mBoard.CalculateMoves();
                                mBoard.SetGameState();
                                if (mBoard.State == EGameState.PROMOTION)
                                {
                                    SetPromotionMode();
                                }
                                else if (mBoard.State == EGameState.CHECKMATE || mBoard.State == EGameState.STALEMATE)
                                {
                                    SetGameMateMode();
                                }
                            }
                            break;

                        case EMoveType.EN_PASSANT_MOVE:
                            if (!mBoard.SimulateIfCheckWhenGoToThere(grep.Player, mGrepX, mGrepY, mX, mY))
                            {
                                mBoard.EnPassantMove(mGrepX, mGrepY, mX, mY);
                                mGrep = false;
                                mBoard.ToggleTurn();
                                mBoard.CalculateMoves();
                                mBoard.SetGameState();
                                if (mBoard.State == EGameState.PROMOTION)
                                {
                                    SetPromotionMode();
                                }
                                else if (mBoard.State == EGameState.CHECKMATE || mBoard.State == EGameState.STALEMATE)
                                {
                                    SetGameMateMode();
                                }
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("Cannot go there. Check if you go there!");
                            }
                            break;

                        default:
                            Debug.Assert(false, $"Wrong Move type: {move.Type}");
                            break;
                    }
                }
            }
        }
    }
}
