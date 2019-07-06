using System.Collections.Generic;
using System.Diagnostics;

namespace Chess
{
    enum EGameState
    {
        NORMAL,
        CHECK,
        CHECKMATE,
        STALEMATE,
        PROMOTION
    }

    class Board
    {
        public EGameState State { get; private set; } = EGameState.NORMAL;
        private Piece[,] mMap = new Piece[8, 8];
        public EPlayerType Turn { get; private set; } = EPlayerType.BLACK;
        private Piece mPromotion;

        public Board()
        {
        }

        public void ToggleTurn()
        {
            if (Turn == EPlayerType.BLACK)
            {
                Turn = EPlayerType.WHITE;
            }
            else
            {
                Turn = EPlayerType.BLACK;
            }
        }

        public Piece GetPieceOrNull(int x, int y)
        {
            Debug.Assert(x >= 0 && x <= 7, $"Wrong X position: {x}");
            Debug.Assert(y >= 0 && y <= 7, $"Wrong Y position: {y}");
            return mMap[y, x];
        }
        public void PutPiece(int x, int y, EPieceType piece, EPlayerType player)
        {
            Debug.Assert(x >= 0 && x <= 7, $"Wrong X position: {x}");
            Debug.Assert(y >= 0 && y <= 7, $"Wrong Y position: {y}");
            mMap[y, x] = new Piece(piece, player);
        }

        public void SetGameState()
        {
            if (ValidateIfCheck(mMap, Turn))
            {
                State = EGameState.CHECK;
                if (ValidateIfCheckmate(mMap, Turn))
                {
                    State = EGameState.CHECKMATE;
                }
            }
            else
            {
                State = EGameState.NORMAL;
                if (ValidateIfPromotion(mMap))
                {
                    State = EGameState.PROMOTION;
                }
                else if (ValidateIfStalemate(mMap, Turn))
                {
                    State = EGameState.STALEMATE;
                }
            }

        }
        private bool ValidateIfCheck(Piece[,] map, EPlayerType player)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Piece piece = map[i, j];
                    if (piece != null && piece.Player != player)
                    {
                        List<Move> moves = piece.PossibleMoves;
                        foreach (Move move in moves)
                        {
                            int x = move.X;
                            int y = move.Y;
                            Piece king = map[y, x];
                            if (king != null && king.Type == EPieceType.KING && king.Player != piece.Player)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        private bool ValidateIfCheckmate(Piece[,] map, EPlayerType player)
        {
            Debug.Assert(State == EGameState.CHECK, "Cannot be Checkmate if not checked");
            if (CountPossibleMoves(player) == 0)
            {
                return true;
            }
            else if (!SimulateIfCheckCanbeSolved(player))
            {
                return true;
            }
            return false;
        }
        private bool ValidateIfStalemate(Piece[,] map, EPlayerType player)
        {
            Debug.Assert(State != EGameState.CHECK, "Cannot be Stalemate if checked");
            if (CountPossibleMoves(player) == 0)
            {
                return true;
            }
            else if (SimulateIfAllPossibleMovesMakeCheck(player))
            {
                return true;
            }
            return false;
        }
        private bool ValidateIfPromotion(Piece[,] map)
        {
            Debug.Assert(State != EGameState.CHECK, "Cannot Promote if checked");
            for (int i = 0; i < 8; i++)
            {
                Piece pawn = map[0, i];
                if (pawn == null)
                {
                    continue;
                }
                else if (pawn.Type == EPieceType.PAWN)
                {
                    mPromotion = pawn;
                    return true;
                }
            }
            for (int i = 0; i < 8; i++)
            {
                Piece pawn = map[7, i];
                if (pawn == null)
                {
                    continue;
                }
                else if (pawn.Type == EPieceType.PAWN)
                {
                    mPromotion = pawn;
                    return true;
                }
            }
            return false;
        }       

        public Move GetMoveInPossibleMovesOrNull(int xTo, int yTo, Piece piece)
        {
            return piece.GetMoveOrNull(xTo, yTo);
        }

        public void NormalMove(int xFrom, int yFrom, int xTo, int yTo)
        {
            Debug.Assert(xFrom >= 0 || xFrom <= 7, $"Wrong x of start position! xFrom: {xFrom}");
            Debug.Assert(yFrom >= 0 || yFrom <= 7, $"Wrong y of start position! yFrom: {yFrom}");
            Debug.Assert(xTo >= 0 || xTo <= 7, $"Wrong x of end position! xTo: {xTo}");
            Debug.Assert(yTo >= 0 || yTo <= 7, $"Wrong y of end position! yTo: {yTo}");
            NormalMove(xFrom, yFrom, xTo, yTo, mMap);
        }
        private void NormalMove(int xFrom, int yFrom, int xTo, int yTo, Piece[,] map)
        {
            Debug.Assert(xFrom >= 0 || xFrom <= 7, $"Wrong x of start position! xFrom: {xFrom}");
            Debug.Assert(yFrom >= 0 || yFrom <= 7, $"Wrong y of start position! yFrom: {yFrom}");
            Debug.Assert(xTo >= 0 || xTo <= 7, $"Wrong x of end position! xTo: {xTo}");
            Debug.Assert(yTo >= 0 || yTo <= 7, $"Wrong y of end position! yTo: {yTo}");

            map[yTo, xTo] = map[yFrom, xFrom];
            map[yFrom, xFrom] = null;
            map[yTo, xTo].SetMoved();
        }

        public void CastlingMove(EPlayerType player, int xRook)
        {
            Debug.Assert(xRook == 0 || xRook == 7, $"Wrong Rook x position. xRook: {xRook}");
            CastlingMove(player, xRook, mMap);
        }
        private void CastlingMove(EPlayerType player, int xRook, Piece[,] map)
        {
            Debug.Assert(xRook == 0 || xRook == 7, $"Wrong Rook x position. xRook: {xRook}");

            if(player == EPlayerType.BLACK)
            {
                if(xRook == 0)
                {
                    map[0, 2] = map[0, 4];
                    map[0, 4] = null;
                    map[0, 2].SetMoved();
                    map[0, 3] = map[0, 0];
                    map[0, 0] = null;
                    map[0, 3].SetMoved();
                }
                else
                {
                    map[0, 6] = map[0, 4];
                    map[0, 4] = null;
                    map[0, 6].SetMoved();
                    map[0, 5] = map[0, 7];
                    map[0, 7] = null;
                    map[0, 5].SetMoved();
                }
            }
            else
            {
                if (xRook == 0)
                {
                    map[7, 2] = map[7, 4];
                    map[7, 4] = null;
                    map[7, 2].SetMoved();
                    map[7, 3] = map[7, 0];
                    map[7, 0] = null;
                    map[7, 3].SetMoved();
                }
                else
                {
                    map[7, 6] = map[7, 4];
                    map[7, 4] = null;
                    map[7, 6].SetMoved();
                    map[7, 5] = map[7, 7];
                    map[7, 7] = null;
                    map[7, 5].SetMoved();
                }
            }
        }

        public void PawnDoubleMove(int xFrom, int yFrom, int xTo, int yTo)
        {
            Debug.Assert(xFrom >= 0 || xFrom <= 7, $"Wrong x of start position! xFrom: {xFrom}");
            Debug.Assert(yFrom >= 0 || yFrom <= 7, $"Wrong y of start position! yFrom: {yFrom}");
            Debug.Assert(xTo >= 0 || xTo <= 7, $"Wrong x of end position! xTo: {xTo}");
            Debug.Assert(yTo >= 0 || yTo <= 7, $"Wrong y of end position! yTo: {yTo}");
            PawnDoubleMove(xFrom, yFrom, xTo, yTo, mMap);
        }
        private void PawnDoubleMove(int xFrom, int yFrom, int xTo, int yTo, Piece[,] map)
        {
            Debug.Assert(xFrom >= 0 || xFrom <= 7, $"Wrong x of start position! xFrom: {xFrom}");
            Debug.Assert(yFrom >= 0 || yFrom <= 7, $"Wrong y of start position! yFrom: {yFrom}");
            Debug.Assert(xTo >= 0 || xTo <= 7, $"Wrong x of end position! xTo: {xTo}");
            Debug.Assert(yTo >= 0 || yTo <= 7, $"Wrong y of end position! yTo: {yTo}");

            map[yTo, xTo] = map[yFrom, xFrom];
            map[yFrom, xFrom] = null;
            map[yTo, xTo].SetEnPassant();
        }

        public void EnPassantMove(int xFrom, int yFrom, int xTo, int yTo)
        {
            EnPassantMove(xFrom, yFrom, xTo, yTo, mMap);
        }
        private void EnPassantMove(int xFrom, int yFrom, int xTo, int yTo, Piece[,] map)
        {
            Debug.Assert(xFrom >= 0 || xFrom <= 7, $"Wrong x of start position! xFrom: {xFrom}");
            Debug.Assert(yFrom >= 0 || yFrom <= 7, $"Wrong y of start position! yFrom: {yFrom}");
            Debug.Assert(xTo >= 0 || xTo <= 7, $"Wrong x of end position! xTo: {xTo}");
            Debug.Assert(yTo >= 0 || yTo <= 7, $"Wrong y of end position! yTo: {yTo}");

            map[yTo, xTo] = map[yFrom, xFrom];
            map[yFrom, xFrom] = null;
            map[yFrom, xTo] = null;
        }

        public void CalculateMoves()
        {
            CalculateMoves(mMap);
        }
        private void CalculateMoves(Piece[,] map)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Piece piece = map[i, j];
                    if (piece == null) continue;
                    switch (piece.Type)
                    {
                        case EPieceType.PAWN:
                            CalculatePawnMoves(piece, j, i, map);
                            if(piece.Player == Turn && piece.State == EPieceState.ABLE_TO_BE_KILLED_BY_EN_PASSANT)
                            {
                                piece.SetMoved();
                            }
                            break;
                        case EPieceType.ROOK:
                            CalculateRookMoves(piece, j, i, map);
                            break;
                        case EPieceType.KNIGHT:
                            CalculateKnightMoves(piece, j, i, map);
                            break;
                        case EPieceType.BISHOP:
                            CalculateBishopMoves(piece, j, i, map);
                            break;
                        case EPieceType.QUEEN:
                            CalculateQueenMoves(piece, j, i, map);
                            break;
                        case EPieceType.KING:
                            CalculateKingMoves(piece, j, i, map);
                            break;
                        default:
                            Debug.Assert(false, "Wrong PieceType!");
                            break;
                    }
                }
            }
        }
        private void CalculatePawnMoves(Piece pawn, int x, int y, Piece[,] map)
        {
            Debug.Assert(pawn != null, "You cannot use this method if piece is null.");
            List<Move> moves = pawn.PossibleMoves;
            moves.Clear();
            if (pawn.Player == EPlayerType.WHITE)
            {
                if (pawn.State == EPieceState.NOT_MOVED)
                {
                    if (y > 1 && map[y - 2, x] == null)
                    {
                        moves.Add(new Move(x, y - 2, EMoveType.PAWN_DOUBLE_MOVE));
                    }
                }
                if (y > 0 && map[y - 1, x] == null)
                {
                    moves.Add(new Move(x, y - 1, EMoveType.NORMAL_MOVE));
                }
                if (x > 0 && y > 0 && map[y - 1, x - 1] != null && map[y - 1, x - 1].Player != pawn.Player)
                {
                    moves.Add(new Move(x - 1, y - 1, EMoveType.NORMAL_MOVE));
                }
                if (x < 7 && y > 0 && map[y - 1, x + 1] != null && map[y - 1, x + 1].Player != pawn.Player)
                {
                    moves.Add(new Move(x + 1, y - 1, EMoveType.NORMAL_MOVE));
                }
                if(x > 0 && map[y, x - 1] != null && map[y, x - 1].Type == EPieceType.PAWN && map[y, x - 1].Player != pawn.Player && map[y, x - 1].State == EPieceState.ABLE_TO_BE_KILLED_BY_EN_PASSANT)
                {
                    moves.Add(new Move(x - 1, y - 1, EMoveType.EN_PASSANT_MOVE));
                }
                if (x < 7 && map[y, x + 1] != null && map[y, x + 1].Type == EPieceType.PAWN && map[y, x + 1].Player != pawn.Player && map[y, x + 1].State == EPieceState.ABLE_TO_BE_KILLED_BY_EN_PASSANT)
                {
                    moves.Add(new Move(x + 1, y - 1, EMoveType.EN_PASSANT_MOVE));
                }
            }
            else
            {
                if (pawn.State == EPieceState.NOT_MOVED)
                {
                    if (y < 6 && map[y + 2, x] == null)
                    {
                        moves.Add(new Move(x, y + 2, EMoveType.PAWN_DOUBLE_MOVE));
                    }
                }
                if (y < 7 && map[y + 1, x] == null)
                {
                    moves.Add(new Move(x, y + 1, EMoveType.NORMAL_MOVE));
                }
                if (x > 0 && y < 7 && map[y + 1, x - 1] != null && map[y + 1, x - 1].Player != pawn.Player)
                {
                    moves.Add(new Move(x - 1, y + 1, EMoveType.NORMAL_MOVE));
                }
                if (x < 7 && y < 7 && map[y + 1, x + 1] != null && map[y + 1, x + 1].Player != pawn.Player)
                {
                    moves.Add(new Move(x + 1, y + 1, EMoveType.NORMAL_MOVE));
                }
                if (x > 0 && map[y, x - 1] != null && map[y, x - 1].Type == EPieceType.PAWN && map[y, x - 1].Player != pawn.Player && map[y, x - 1].State == EPieceState.ABLE_TO_BE_KILLED_BY_EN_PASSANT)
                {
                    moves.Add(new Move(x - 1, y + 1, EMoveType.EN_PASSANT_MOVE));
                }
                if (x < 7 && map[y, x + 1] != null && map[y, x + 1].Type == EPieceType.PAWN && map[y, x + 1].Player != pawn.Player && map[y, x + 1].State == EPieceState.ABLE_TO_BE_KILLED_BY_EN_PASSANT)
                {
                    moves.Add(new Move(x + 1, y + 1, EMoveType.EN_PASSANT_MOVE));
                }
            }
        }
        private void CalculateRookMoves(Piece rook, int x, int y, Piece[,] map)
        {
            Debug.Assert(rook != null, "You cannot use this method if piece is null.");
            List<Move> moves = rook.PossibleMoves;
            moves.Clear();
            int xx = x;
            int yy = y;
            while (true)
            {
                xx--;
                if (xx >= 0)
                {
                    if (map[yy, xx] == null)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                    }
                    else if (map[yy, xx].Player != rook.Player)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                        break;
                    }
                    else if (map[yy, xx].Player == rook.Player)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            xx = x;
            yy = y;
            while (true)
            {
                xx++;
                if (xx <= 7)
                {
                    if (map[yy, xx] == null)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                    }
                    else if (map[yy, xx].Player != rook.Player)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                        break;
                    }
                    else if (map[yy, xx].Player == rook.Player)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            xx = x;
            yy = y;
            while (true)
            {
                yy--;
                if (yy >= 0)
                {
                    if (map[yy, xx] == null)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                    }
                    else if (map[yy, xx].Player != rook.Player)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                        break;
                    }
                    else if (map[yy, xx].Player == rook.Player)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            xx = x;
            yy = y;
            while (true)
            {
                yy++;
                if (yy <= 7)
                {
                    if (map[yy, xx] == null)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                    }
                    else if (map[yy, xx].Player != rook.Player)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                        break;
                    }
                    else if (map[yy, xx].Player == rook.Player)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        private void CalculateKnightMoves(Piece knight, int x, int y, Piece[,] map)
        {
            Debug.Assert(knight != null, "You cannot use this method if piece is null.");
            List<Move> moves = knight.PossibleMoves;
            moves.Clear();
            if (x - 2 >= 0)
            {
                if (y - 1 >= 0)
                {
                    if (map[y - 1, x - 2] == null || map[y - 1, x - 2].Player != knight.Player)
                    {
                        moves.Add(new Move(x - 2, y - 1, EMoveType.NORMAL_MOVE));
                    }
                }
                if (y + 1 <= 7)
                {
                    if (map[y + 1, x - 2] == null || map[y + 1, x - 2].Player != knight.Player)
                    {
                        moves.Add(new Move(x - 2, y + 1, EMoveType.NORMAL_MOVE));
                    }
                }
            }
            if (x + 2 <= 7)
            {
                if (y - 1 >= 0)
                {
                    if (map[y - 1, x + 2] == null || map[y - 1, x + 2].Player != knight.Player)
                    {
                        moves.Add(new Move(x + 2, y - 1, EMoveType.NORMAL_MOVE));
                    }
                }
                if (y + 1 <= 7)
                {
                    if (map[y + 1, x + 2] == null || map[y + 1, x + 2].Player != knight.Player)
                    {
                        moves.Add(new Move(x + 2, y + 1, EMoveType.NORMAL_MOVE));
                    }
                }
            }
            if (y - 2 >= 0)
            {
                if (x - 1 >= 0)
                {
                    if (map[y - 2, x - 1] == null || map[y - 2, x - 1].Player != knight.Player)
                    {
                        moves.Add(new Move(x - 1, y - 2, EMoveType.NORMAL_MOVE));
                    }
                }
                if (x + 1 <= 7)
                {
                    if (map[y - 2, x + 1] == null || map[y - 2, x + 1].Player != knight.Player)
                    {
                        moves.Add(new Move(x + 1, y - 2, EMoveType.NORMAL_MOVE));
                    }
                }
            }
            if (y + 2 <= 7)
            {
                if (x - 1 >= 0)
                {
                    if (map[y + 2, x - 1] == null || map[y + 2, x - 1].Player != knight.Player)
                    {
                        moves.Add(new Move(x - 1, y + 2, EMoveType.NORMAL_MOVE));
                    }
                }
                if (x + 1 <= 7)
                {
                    if (map[y + 2, x + 1] == null || map[y + 2, x + 1].Player != knight.Player)
                    {
                        moves.Add(new Move(x + 1, y + 2, EMoveType.NORMAL_MOVE));
                    }
                }
            }
        }
        private void CalculateBishopMoves(Piece bishop, int x, int y, Piece[,] map)
        {
            Debug.Assert(bishop != null, "You cannot use this method if piece is null.");
            List<Move> moves = bishop.PossibleMoves;
            moves.Clear();
            int xx = x;
            int yy = y;
            while (true)
            {
                xx--;
                yy--;
                if (xx >= 0 && yy >= 0)
                {
                    if (map[yy, xx] == null)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                    }
                    else if (map[yy, xx].Player != bishop.Player)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                        break;
                    }
                    else if (map[yy, xx].Player == bishop.Player)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            xx = x;
            yy = y;
            while (true)
            {
                xx++;
                yy++;
                if (xx <= 7 && yy <= 7)
                {
                    if (map[yy, xx] == null)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                    }
                    else if (map[yy, xx].Player != bishop.Player)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                        break;
                    }
                    else if (map[yy, xx].Player == bishop.Player)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            xx = x;
            yy = y;
            while (true)
            {
                xx--;
                yy++;
                if (xx >= 0 && yy <= 7)
                {
                    if (map[yy, xx] == null)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                    }
                    else if (map[yy, xx].Player != bishop.Player)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                        break;
                    }
                    else if (map[yy, xx].Player == bishop.Player)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            xx = x;
            yy = y;
            while (true)
            {
                xx++;
                yy--;
                if (xx <= 7 && yy >= 0)
                {
                    if (map[yy, xx] == null)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                    }
                    else if (map[yy, xx].Player != bishop.Player)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                        break;
                    }
                    else if (map[yy, xx].Player == bishop.Player)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        private void CalculateQueenMoves(Piece queen, int x, int y, Piece[,] map)
        {
            Debug.Assert(queen != null, "You cannot use this method if piece is null.");
            List<Move> moves = queen.PossibleMoves;
            moves.Clear();
            int xx = x;
            int yy = y;
            while (true)
            {
                xx--;
                if (xx >= 0)
                {
                    if (map[yy, xx] == null)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                    }
                    else if (map[yy, xx].Player != queen.Player)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                        break;
                    }
                    else if (map[yy, xx].Player == queen.Player)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            xx = x;
            yy = y;
            while (true)
            {
                xx++;
                if (xx <= 7)
                {
                    if (map[yy, xx] == null)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                    }
                    else if (map[yy, xx].Player != queen.Player)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                        break;
                    }
                    else if (map[yy, xx].Player == queen.Player)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            xx = x;
            yy = y;
            while (true)
            {
                yy--;
                if (yy >= 0)
                {
                    if (map[yy, xx] == null)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                    }
                    else if (map[yy, xx].Player != queen.Player)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                        break;
                    }
                    else if (map[yy, xx].Player == queen.Player)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            xx = x;
            yy = y;
            while (true)
            {
                yy++;
                if (yy <= 7)
                {
                    if (map[yy, xx] == null)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                    }
                    else if (map[yy, xx].Player != queen.Player)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                        break;
                    }
                    else if (map[yy, xx].Player == queen.Player)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            xx = x;
            yy = y;
            while (true)
            {
                xx--;
                yy--;
                if (xx >= 0 && yy >= 0)
                {
                    if (map[yy, xx] == null)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                    }
                    else if (map[yy, xx].Player != queen.Player)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                        break;
                    }
                    else if (map[yy, xx].Player == queen.Player)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            xx = x;
            yy = y;
            while (true)
            {
                xx++;
                yy++;
                if (xx <= 7 && yy <= 7)
                {
                    if (map[yy, xx] == null)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                    }
                    else if (map[yy, xx].Player != queen.Player)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                        break;
                    }
                    else if (map[yy, xx].Player == queen.Player)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            xx = x;
            yy = y;
            while (true)
            {
                xx--;
                yy++;
                if (xx >= 0 && yy <= 7)
                {
                    if (map[yy, xx] == null)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                    }
                    else if (map[yy, xx].Player != queen.Player)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                        break;
                    }
                    else if (map[yy, xx].Player == queen.Player)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            xx = x;
            yy = y;
            while (true)
            {
                xx++;
                yy--;
                if (xx <= 7 && yy >= 0)
                {
                    if (map[yy, xx] == null)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                    }
                    else if (map[yy, xx].Player != queen.Player)
                    {
                        moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                        break;
                    }
                    else if (map[yy, xx].Player == queen.Player)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        private void CalculateKingMoves(Piece king, int x, int y, Piece[,] map)
        {
            Debug.Assert(king != null, "You cannot use this method if piece is null.");
            List<Move> moves = king.PossibleMoves;
            moves.Clear();
            int xx = x;
            int yy = y;
            xx--;
            if (xx >= 0)
            {
                if (map[yy, xx] == null)
                {
                    moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                }
                else if (map[yy, xx].Player != king.Player)
                {
                    moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                }
            }
            xx = x;
            yy = y;
            xx++;
            if (xx <= 7)
            {
                if (map[yy, xx] == null)
                {
                    moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                }
                else if (map[yy, xx].Player != king.Player)
                {
                    moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                }
            }
            xx = x;
            yy = y;
            yy--;
            if (yy >= 0)
            {
                if (map[yy, xx] == null)
                {
                    moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                }
                else if (map[yy, xx].Player != king.Player)
                {
                    moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                }
            }
            xx = x;
            yy = y;
            yy++;
            if (yy <= 7)
            {
                if (map[yy, xx] == null)
                {
                    moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                }
                else if (map[yy, xx].Player != king.Player)
                {
                    moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                }
            }
            xx = x;
            yy = y;
            xx--;
            yy--;
            if (xx >= 0 && yy >= 0)
            {
                if (map[yy, xx] == null)
                {
                    moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                }
                else if (map[yy, xx].Player != king.Player)
                {
                    moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                }
            }
            xx = x;
            yy = y;
            xx++;
            yy++;
            if (xx <= 7 && yy <= 7)
            {
                if (map[yy, xx] == null)
                {
                    moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                }
                else if (map[yy, xx].Player != king.Player)
                {
                    moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                }
            }
            xx = x;
            yy = y;
            xx--;
            yy++;
            if (xx >= 0 && yy <= 7)
            {
                if (map[yy, xx] == null)
                {
                    moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                }
                else if (map[yy, xx].Player != king.Player)
                {
                    moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                }
            }
            xx = x;
            yy = y;
            xx++;
            yy--;
            if (xx <= 7 && yy >= 0)
            {
                if (map[yy, xx] == null)
                {
                    moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                }
                else if (map[yy, xx].Player != king.Player)
                {
                    moves.Add(new Move(xx, yy, EMoveType.NORMAL_MOVE));
                }
            }

            // Castling Move
            if (king.State == EPieceState.NOT_MOVED && !ValidateIfCheck(map, king.Player))
            {
                if (king.Player == EPlayerType.BLACK)
                {
                    Piece leftRook = map[0, 0];
                    if (leftRook != null && leftRook.Type == EPieceType.ROOK && leftRook.State == EPieceState.NOT_MOVED)
                    {
                        if(map[0,1] == null && map[0,2] == null && map[0,3] == null)
                        {
                            moves.Add(new Move(2, 0, EMoveType.KING_CASTLING));
                        }
                    }
                    Piece rightRook = map[0, 7];
                    if (rightRook != null && rightRook.Type == EPieceType.ROOK && rightRook.State == EPieceState.NOT_MOVED)
                    {
                        if (map[0, 5] == null && map[0, 6] == null)
                        {
                            moves.Add(new Move(6, 0, EMoveType.KING_CASTLING));
                        }                            
                    }
                }
                else
                {
                    Piece leftRook = map[7, 0];
                    if (leftRook != null && leftRook.Type == EPieceType.ROOK && leftRook.State == EPieceState.NOT_MOVED)
                    {
                        if (map[7, 1] == null && map[7, 2] == null && map[7, 3] == null)
                        {
                            moves.Add(new Move(2, 7, EMoveType.KING_CASTLING));
                        }                            
                    }
                    Piece rightRook = map[7, 7];
                    if (rightRook != null && rightRook.Type == EPieceType.ROOK && rightRook.State == EPieceState.NOT_MOVED)
                    {
                        if (map[7, 5] == null && map[7, 6] == null)
                        {
                            moves.Add(new Move(6, 7, EMoveType.KING_CASTLING));
                        }                            
                    }
                }
            }

        }

        private Piece[,] CopyMap(Piece[,] map)
        {
            Piece[,] newMap = new Piece[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Piece piece = map[i, j];
                    if (piece != null)
                    {
                        Piece copyPiece = new Piece(piece.Type, piece.Player);
                        newMap[i, j] = copyPiece;
                    }
                }
            }
            return newMap;
        }

        private int CountPossibleMoves(EPlayerType player)
        {
            int cnt = 0;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Piece piece = GetPieceOrNull(j, i);
                    if (piece != null && piece.Player == player)
                    {
                        cnt += piece.PossibleMoves.Count;
                    }
                }
            }

            return cnt;
        }

        public bool SimulateIfCheckWhenGoToThere(EPlayerType player, int xFrom, int yFrom, int xTo, int yTo)
        {
            Debug.Assert(xFrom >= 0 || xFrom <= 7, "Wrong x of start position!");
            Debug.Assert(yFrom >= 0 || yFrom <= 7, "Wrong y of start position!");
            Debug.Assert(xTo >= 0 || xTo <= 7, "Wrong x of end position!");
            Debug.Assert(yTo >= 0 || yTo <= 7, "Wrong y of end position!");
            Piece[,] map = CopyMap(mMap);
            Piece piece = map[yFrom, xFrom];
            Debug.Assert(piece != null, "You cannot use this method for the position of no piece!");

            NormalMove(xFrom, yFrom, xTo, yTo, map);
            CalculateMoves(map);
            return ValidateIfCheck(map, player);
        }
        private bool SimulateIfCheckCanbeSolved(EPlayerType player)
        {
            Debug.Assert(State == EGameState.CHECK, "You cannot use this method if not Check");
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Piece piece = mMap[i, j];
                    if (piece != null && piece.Player == player)
                    {
                        List<Move> moves = piece.PossibleMoves;
                        foreach (Move move in moves)
                        {
                            int xTo = move.X;
                            int yTo = move.Y;
                            if(!SimulateIfCheckWhenGoToThere(player, j, i, xTo, yTo))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        private bool SimulateIfAllPossibleMovesMakeCheck(EPlayerType player)
        {
            Debug.Assert(State != EGameState.CHECK, "You cannot use this method if Check");
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Piece piece = mMap[i, j];
                    if (piece != null && piece.Player == player)
                    {
                        List<Move> moves = piece.PossibleMoves;
                        foreach (Move move in moves)
                        {
                            int xTo = move.X;
                            int yTo = move.Y;
                            if (!SimulateIfCheckWhenGoToThere(player, j, i, xTo, yTo))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void Promote(EPieceType type)
        {
            mPromotion.Promote(type);
        }
    }
}
