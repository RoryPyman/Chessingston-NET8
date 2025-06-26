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
            #pragma warning disable CS8602 // Dereference of a possibly null reference.
            possibleMoves = possibleMoves.Where(p => board.GetPiece(p) == null || board.GetPiece(p).Player != this.Player).ToList<Square>();
            #pragma warning restore CS8602 // Dereference of a possibly null reference.
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