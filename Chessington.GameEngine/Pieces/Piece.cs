using System;
using System.Collections.Generic;
using System.Linq;

namespace Chessington.GameEngine.Pieces
{
    public abstract class Piece
    {
        protected Piece(Player player)
        {
            Player = player;
        }

        public Player Player { get; private set; }

        public abstract IEnumerable<Square> GetAvailableMoves(Board board);

        public void MoveTo(Board board, Square newSquare)
        {
            var currentSquare = board.FindPiece(this);
            board.MovePiece(currentSquare, newSquare);
        }

        public List<Square> findDirectionalSquares(Board board, Square position, int xIndex, int yIndex)
        {
            List<Square> available = new List<Square>();
            int row = position.Row;
            int col = position.Col;
            while (row < GameSettings.BoardSize && col < GameSettings.BoardSize && row >= 0 && col >= 0)
            {
                if (board.GetPiece(new Square(row, col)) == null)
                {
                    available.Add(new Square(row, col));
                }
                row += xIndex;
                col += yIndex;
            }
            return available;
        }
    }
}