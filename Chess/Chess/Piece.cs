using System.Diagnostics;
using System.Collections.Generic;

namespace Chess
{
    enum EPlayerType
    {
        WHITE,
        BLACK
    }

    enum EPieceType
    {
        PAWN,
        ROOK,
        KNIGHT,
        BISHOP,
        QUEEN,
        KING
    }

    enum EPieceState
    {
        NOT_MOVED, // To use for validate Pawn first move, castling of King and Rooks
        MOVED,
        ABLE_TO_BE_KILLED_BY_EN_PASSANT // To validate en passant
    }

    class Piece
    {
        public EPieceType Type { get; private set; }
        public EPieceState State { get; private set; } = EPieceState.NOT_MOVED;
        public List<Move> PossibleMoves { get; private set; } = new List<Move>(20);
        public EPlayerType Player { get; private set; }

        public Piece(EPieceType pieceType, EPlayerType player)
        {
            Type = pieceType;
            Player = player;
        }

        public void Promote(EPieceType type)
        {
            Debug.Assert(Type == EPieceType.PAWN, "You cannot use this method except for Pawn.");
            Debug.Assert(type != EPieceType.PAWN, "Pawn cannot promote to Pawn.");

            Type = type;
        }

        public void SetMoved()
        {
            State = EPieceState.MOVED;
        }

        public void SetEnPassant()
        {
            Debug.Assert(Type == EPieceType.PAWN, "You cannot use this method except for Pawn.");
            Debug.Assert(State == EPieceState.NOT_MOVED, "You cannot use this method if the pawn already moved.");

            State = EPieceState.ABLE_TO_BE_KILLED_BY_EN_PASSANT;
        }

        public Move GetMoveOrNull(int x, int y)
        {
            foreach(Move move in PossibleMoves)
            {
                if(x == move.X && y == move.Y)
                {
                    return move;
                }
            }
            return null;
        }
    }
}
