﻿using System.Collections.Generic;
using System.Linq;

namespace Chessington.GameEngine.Pieces
{
    public class Knight : Piece
    {
        public Knight(Player player)
            : base(player) { }
            
        
        public override IEnumerable<Square> GetAvailableMoves(Board board)
        {
            
            return GetAvailableMovesNoCheck(board).Where(s => !board.doesMoveCauseCheck(s, this));
        }

        public override IEnumerable<Square> GetAvailableMovesNoCheck(Board board)
        {
            Square position = board.FindPiece(this);
            List<Square> possibleMoves = new List<Square>();
            possibleMoves.AddRange(FindKnightMoves(board, position, 2, 1));
            possibleMoves.AddRange(FindKnightMoves(board, position, 1, 2));

            possibleMoves.RemoveAll(p => p.Col < 0 || p.Col > 7 || p.Row < 0 || p.Row > 7);
            possibleMoves = possibleMoves.Where(p => board.GetPiece(p) == null || board.GetPiece(p)!.Player != this.Player).ToList<Square>();
            return possibleMoves;
        }


        private List<Square> FindKnightMoves(Board board, Square position, int xIndex, int yIndex)
        {
            List<Square> available = new List<Square>();


            int row = position.Row;
            int col = position.Col;

            available.Add(new Square(row + xIndex, col + yIndex));
            available.Add(new Square(row - xIndex, col + yIndex));
            available.Add(new Square(row + xIndex, col - yIndex));
            available.Add(new Square(row - xIndex, col - yIndex));
            return available;
        }
    }
}