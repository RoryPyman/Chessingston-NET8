using System.Collections.Generic;
using System.Linq;

namespace Chessington.GameEngine.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(Player player)
            : base(player) { }

        public override IEnumerable<Square> GetAvailableMoves(Board board)
        {
            Square square = board.FindPiece(this);
            List<Square> availableMoves = new List<Square>();


            availableMoves.AddRange(findDirectionalSquares(board, square, 1, -1));
            availableMoves.AddRange(findDirectionalSquares(board, square, -1, -1));
            availableMoves.AddRange(findDirectionalSquares(board, square, 1, 1));
            availableMoves.AddRange(findDirectionalSquares(board, square, -1, 1));
            return availableMoves.ToArray();
        }


    }
}