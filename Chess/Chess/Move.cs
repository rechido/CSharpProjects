using System.Diagnostics;

namespace Chess
{
    enum EMoveType
    {
        PAWN_DOUBLE_MOVE,
        KING_CASTLING,
        NORMAL_MOVE,
        EN_PASSANT_MOVE
    }

    class Move
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public EMoveType Type { get; private set; }

        public Move(int x, int y, EMoveType type)
        {
            Debug.Assert(x >= 0 && x <= 7, "Wrong Move x Position!");
            Debug.Assert(y >= 0 && y <= 7, "Wrong Move y Position!");
            X = x;
            Y = y;
            Type = type;
        }
    }
}
