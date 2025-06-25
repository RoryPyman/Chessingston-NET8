using System.Collections.Generic;
using System.Linq;

namespace Chessington.GameEngine.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(Player player)
            : base(player) { }

        public override IEnumerable<Square> GetAvailableMoves(Board board)
        {
            int i = 0;
            int j = 0;
            while (i < GameSettings.BoardSize && j < GameSettings.BoardSize && i > 0 && j > 0)
        }
    }
}