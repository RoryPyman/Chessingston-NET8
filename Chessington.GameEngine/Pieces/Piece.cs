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
        public abstract IEnumerable<Square> GetAvailableMovesNoCheck(Board board);

        public void MoveTo(Board board, Square newSquare)
        {
            var currentSquare = board.FindPiece(this);
            board.MovePiece(currentSquare, newSquare);
        }

        public List<Square> findDirectionalSquares(Board board, Square position, int xIndex, int yIndex, int depth=10)
        {
            List<Square> available = new List<Square>();
            int row = position.Row;
            int col = position.Col;
            int loop = 0;
            while (row < GameSettings.BoardSize && col < GameSettings.BoardSize && row >= 0 && col >= 0 && loop < depth)
            {
                row += xIndex;
                col += yIndex;
                if (row == GameSettings.BoardSize || col == GameSettings.BoardSize || row < 0 || col < 0) return available;
                Square newSquare = new Square(row, col);
                if (board.GetPiece(newSquare) is null || board.GetPiece(newSquare)!.Player != board.GetPiece(position)!.Player)
                {
                    available.Add(newSquare);
                    if (board.GetPiece(newSquare) != null) return available;
                }
                else
                {
                    return available;
                }

                loop++;
            }
            return available;
        }
    }
}