using System.Collections.Generic;
using System.Linq;

namespace Chessington.GameEngine.Pieces
{
    public class Knight : Piece
    {
        public Knight(Player player)
            : base(player) { }

        public override IEnumerable<Square> GetAvailableMoves(Board board)
        {
            Square position = board.FindPiece(this);
            List<Square> possibleMoves = new List<Square>();
            possibleMoves.AddRange(FindKnightMoves(board, position, 2, 1));
            possibleMoves.AddRange(FindKnightMoves(board, position, 1, 2));

            possibleMoves.RemoveAll(p => p.Col < 0 || p.Col > 7 || p.Row < 0 || p.Row > 8);
            possibleMoves.RemoveAll(p => board.GetPiece(p) != null);
            return possibleMoves;
        }


        private List<Square> FindKnightMoves(Board board, Square position, int xIndex, int yIndex)
        {
            List<Square> available = new List<Square>();
            int row = position.Row;
            int col = position.Col;

            available.Add(new Square(row + xIndex, row + yIndex));
            available.Add(new Square(row - xIndex, row + yIndex));
            available.Add(new Square(row + xIndex, row - yIndex));
            available.Add(new Square(row - xIndex, row - yIndex));
            return available;
        }
    }
}