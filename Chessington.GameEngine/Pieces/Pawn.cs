using System.Collections.Generic;
using System.Linq;

namespace Chessington.GameEngine.Pieces
{
    public class Pawn : Piece
    {
        public Pawn(Player player) 
            : base(player) { }

        public override IEnumerable<Square> GetAvailableMoves(Board board)
        {
            Square postition = board.FindPiece(this);
            int offset = this.Player == Player.White ? -1 : 1;
            if (postition.Row > 8)
            {
                return Enumerable.Empty<Square>();
            }
            Square new_loc = new Square(postition.Row + offset, postition.Col);
            return board.GetPiece(new_loc) == null ? new Square[] { new_loc } : Enumerable.Empty<Square>();
        }
    }
}