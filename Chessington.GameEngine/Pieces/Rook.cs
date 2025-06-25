using System.Collections.Generic;
using System.Linq;

namespace Chessington.GameEngine.Pieces
{
    public class Rook : Piece
    {
        public Rook(Player player)
            : base(player) { }

        public override IEnumerable<Square> GetAvailableMoves(Board board)
        {
            List<Square> availableMoves = new List<Square>();
            Square position = board.FindPiece(this);
            for (int i = 0; i < GameSettings.BoardSize; i++)
            {
                Square square = new Square(position.Row, i);
                if (board.GetPiece(square) != null) continue;
                availableMoves.Add(square);
            }
            for (int i = 0; i < GameSettings.BoardSize; i++) {
                Square square = new Square(i, position.Col);
                if (board.GetPiece(square) != null) continue;
                availableMoves.Add(square);
            }
            return availableMoves;

        }
    }
}