using System.Collections.Generic;
using System.Linq;

namespace Chessington.GameEngine.Pieces
{
    public class King : Piece
    {
        public King(Player player)
            : base(player) { }

        public override IEnumerable<Square> GetAvailableMoves(Board board)
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
            return availableMoves.ToArray();
        }
    }
}