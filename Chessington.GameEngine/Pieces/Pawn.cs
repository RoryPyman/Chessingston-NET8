using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Chessington.GameEngine.Pieces
{
    public class Pawn : Piece
    {
        public Pawn(Player player)
            : base(player) { }

        public override IEnumerable<Square> GetAvailableMoves(Board board)
        {
            Square position = board.FindPiece(this);
            int offset = this.Player == Player.White ? -1 : 1;


            List<Square> allMoves = new List<Square>();
            if (position.Row + offset > 7 || position.Row + offset < 0) return Enumerable.Empty<Square>();
            Square newLoc = new Square(position.Row + offset, position.Col);
            Square newUnmovedLoc;

            if (!(position.Row + offset * 2 > 7 || position.Row + offset * 2 < 0))
            {
                newUnmovedLoc = new Square(position.Row + offset * 2, position.Col);
                if (IsStartingPosition(position, this.Player) && board.GetPiece(newUnmovedLoc) == null && board.GetPiece(newLoc) == null) allMoves.Add(newUnmovedLoc);
            }
            if (board.GetPiece(newLoc) == null) allMoves.Add(newLoc);


            allMoves.AddRange(findDirectionalSquares(board, position, offset, 1, 1, true));
            allMoves.AddRange(findDirectionalSquares(board, position, offset, -1, 1, true));


            return allMoves.ToArray();
        }
        private bool IsStartingPosition(Square position, Player player)
        {
            if (player == Player.White && position.Row == 7)
            {
                return true;
            }
            if (player == Player.Black && position.Row == 1)
            {
                return true;
            }
            return false;
        }
    }
}