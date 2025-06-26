using System.Collections.Generic;
using System.Linq;

namespace Chessington.GameEngine.Pieces
{
    public class King : Piece
    {
        private bool hasMoved = false;
        public King(Player player)
            : base(player) { }

        public override IEnumerable<Square> GetAvailableMoves(Board board, bool ignoreCheck = false)
        {
            List<Square> availableMoves = new List<Square>();

            Square square = board.FindPiece(this);

            availableMoves.AddRange(findDirectionalSquares(board, square, 1, 0, 1));
            availableMoves.AddRange(findDirectionalSquares(board, square, 0, 1, 1));
            availableMoves.AddRange(findDirectionalSquares(board, square, -1, 0, 1));
            availableMoves.AddRange(findDirectionalSquares(board, square, 0, -1, 1));
            availableMoves.AddRange(findDirectionalSquares(board, square, 1, -1, 1));
            availableMoves.AddRange(findDirectionalSquares(board, square, -1, -1, 1));
            availableMoves.AddRange(findDirectionalSquares(board, square, 1, 1, 1));
            availableMoves.AddRange(findDirectionalSquares(board, square, -1, 1, 1));


            availableMoves = availableMoves.Where(s => ignoreCheck || !board.doesMoveCauseCheck(s, this)).ToList();
            return availableMoves;
        }

        private List<Square> AttemptCastle(Board board)
        {
            int[] offsets = [1, -1];
            List<Square> availableMoves = new List<Square>();
            Square position = board.FindPiece(this);
            if (hasMoved) return [];

            int whiteOffset = 0;
            if (this.Player == Player.White)
            {
                whiteOffset = 7;
            }

            foreach (int xOffset in offsets)
            {
                if (SquareFree(board, whiteOffset, xOffset) && SquareFree(board, whiteOffset, xOffset * 2))
                {
                    Piece? maybeRook = board.GetPiece(Square.At(whiteOffset, (xOffset > 0) ? 7 : 0));
                    if (maybeRook is Rook)
                    {
                        Rook rook = (Rook)maybeRook;
                        if (!rook.HasMoved()) availableMoves.Add(Square.At(whiteOffset, xOffset + 4));
                    }
                }
            }
            return availableMoves;
        }

        public void setMovedToTrue()
        {
            hasMoved = true;
        }

        public bool HasMoved()
        {
            return hasMoved;
        }

        public bool SquareFree(Board board, int whiteOffset, int x)
        {
            return board.GetPiece(Square.At(whiteOffset, 4 + x)) == null;
        }
    }
}