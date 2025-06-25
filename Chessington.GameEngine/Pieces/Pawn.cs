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
            if (position.Row > 8)
            {
                return Enumerable.Empty<Square>();
            }

            List<Square> allMoves = new List<Square>();
            Square new_loc = new Square(position.Row + offset, position.Col);
            Square new_unmoved_loc = new Square(position.Row + offset * 2, position.Col);

            if (board.GetPiece(new_loc) == null) allMoves.Add(new_loc);
            if (IsStartingPosition(position, this.Player) && board.GetPiece(new_unmoved_loc) == null) allMoves.Add(new_unmoved_loc);
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