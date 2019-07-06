using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestCases();
            Play();
        }

        static void Play()
        {
            GameMode.PutPiece(0, 0, EPieceType.ROOK, EPlayerType.BLACK);
            GameMode.PutPiece(1, 0, EPieceType.KNIGHT, EPlayerType.BLACK);
            GameMode.PutPiece(2, 0, EPieceType.BISHOP, EPlayerType.BLACK);
            GameMode.PutPiece(3, 0, EPieceType.QUEEN, EPlayerType.BLACK);
            GameMode.PutPiece(4, 0, EPieceType.KING, EPlayerType.BLACK);
            GameMode.PutPiece(5, 0, EPieceType.BISHOP, EPlayerType.BLACK);
            GameMode.PutPiece(6, 0, EPieceType.KNIGHT, EPlayerType.BLACK);
            GameMode.PutPiece(7, 0, EPieceType.ROOK, EPlayerType.BLACK);
            GameMode.PutPiece(0, 1, EPieceType.PAWN, EPlayerType.BLACK);
            GameMode.PutPiece(1, 1, EPieceType.PAWN, EPlayerType.BLACK);
            GameMode.PutPiece(2, 1, EPieceType.PAWN, EPlayerType.BLACK);
            GameMode.PutPiece(3, 1, EPieceType.PAWN, EPlayerType.BLACK);
            GameMode.PutPiece(4, 1, EPieceType.PAWN, EPlayerType.BLACK);
            GameMode.PutPiece(5, 1, EPieceType.PAWN, EPlayerType.BLACK);
            GameMode.PutPiece(6, 1, EPieceType.PAWN, EPlayerType.BLACK);
            GameMode.PutPiece(7, 1, EPieceType.PAWN, EPlayerType.BLACK);

            GameMode.PutPiece(0, 7, EPieceType.ROOK, EPlayerType.WHITE);
            GameMode.PutPiece(1, 7, EPieceType.KNIGHT, EPlayerType.WHITE);
            GameMode.PutPiece(2, 7, EPieceType.BISHOP, EPlayerType.WHITE);
            GameMode.PutPiece(3, 7, EPieceType.QUEEN, EPlayerType.WHITE);
            GameMode.PutPiece(4, 7, EPieceType.KING, EPlayerType.WHITE);
            GameMode.PutPiece(5, 7, EPieceType.BISHOP, EPlayerType.WHITE);
            GameMode.PutPiece(6, 7, EPieceType.KNIGHT, EPlayerType.WHITE);
            GameMode.PutPiece(7, 7, EPieceType.ROOK, EPlayerType.WHITE);
            GameMode.PutPiece(0, 6, EPieceType.PAWN, EPlayerType.WHITE);
            GameMode.PutPiece(1, 6, EPieceType.PAWN, EPlayerType.WHITE);
            GameMode.PutPiece(2, 6, EPieceType.PAWN, EPlayerType.WHITE);
            GameMode.PutPiece(3, 6, EPieceType.PAWN, EPlayerType.WHITE);
            GameMode.PutPiece(4, 6, EPieceType.PAWN, EPlayerType.WHITE);
            GameMode.PutPiece(5, 6, EPieceType.PAWN, EPlayerType.WHITE);
            GameMode.PutPiece(6, 6, EPieceType.PAWN, EPlayerType.WHITE);
            GameMode.PutPiece(7, 6, EPieceType.PAWN, EPlayerType.WHITE);

            Chess.Play();
        }

        static void TestCases()
        {
            //Case3_castling();
            Case4_EnPassant();
        }

        static void Case4_EnPassant()
        {
            GameMode.PutPiece(4, 1, EPieceType.PAWN, EPlayerType.BLACK);
            GameMode.PutPiece(5, 3, EPieceType.PAWN, EPlayerType.WHITE);
            GameMode.PutPiece(3, 3, EPieceType.PAWN, EPlayerType.WHITE);

            GameMode.PutPiece(4, 4, EPieceType.PAWN, EPlayerType.BLACK);
            GameMode.PutPiece(6, 4, EPieceType.PAWN, EPlayerType.BLACK);
            GameMode.PutPiece(5, 6, EPieceType.PAWN, EPlayerType.WHITE);
            Chess.Play();
        }

        static void Case3_castling()
        {
            GameMode.PutPiece(4, 0, EPieceType.KING, EPlayerType.BLACK);
            GameMode.PutPiece(0, 0, EPieceType.ROOK, EPlayerType.BLACK);
            GameMode.PutPiece(7, 0, EPieceType.ROOK, EPlayerType.BLACK);
            GameMode.PutPiece(4, 7, EPieceType.KING, EPlayerType.WHITE);
            GameMode.PutPiece(0, 7, EPieceType.ROOK, EPlayerType.WHITE);
            GameMode.PutPiece(7, 7, EPieceType.ROOK, EPlayerType.WHITE);
            Chess.Play();
        }

        static void Case2_promotion()
        {
            GameMode.PutPiece(0, 5, EPieceType.PAWN, EPlayerType.BLACK);
            GameMode.PutPiece(7, 2, EPieceType.PAWN, EPlayerType.WHITE);
            Chess.Play();
        }

        static void Case1_check_checkmate_stalemate()
        {
            GameMode.PutPiece(4, 0, EPieceType.ROOK, EPlayerType.BLACK);
            GameMode.PutPiece(3, 4, EPieceType.KING, EPlayerType.BLACK);
            GameMode.PutPiece(5, 4, EPieceType.KING, EPlayerType.WHITE);
            Chess.Play();
        }
    }
}
