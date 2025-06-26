using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Chessington.GameEngine.Pieces
{
    public class Pawn : Piece
    {
        public bool hasJustMovedTwo = false;
        public Pawn(Player player)
            : base(player) { }

        public override IEnumerable<Square> GetAvailableMoves(Board board, bool ignoreCheck=false)
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
                if (IsStartingPosition(position, this.Player) && board.GetPiece(newUnmovedLoc) == null && board.GetPiece(newLoc) == null)
                {
                    allMoves.Add(newUnmovedLoc);
                }
            }
            if (board.GetPiece(newLoc) == null) allMoves.Add(newLoc);


            allMoves.AddRange(findCapture(board, position, offset, 1));
            allMoves.AddRange(findCapture(board, position, offset, -1));
            allMoves.AddRange(FindEnPessant(board, Player));

            return allMoves.Where(s => ignoreCheck || !board.doesMoveCauseCheck(s, this));
        }
        private bool IsStartingPosition(Square position, Player player)
        {
            if (player == Player.White && position.Row == 6)
            {
                return true;
            }
            if (player == Player.Black && position.Row == 1)
            {
                return true;
            }
            return false;
        }

        private List<Square> FindEnPessant(Board board, Player player)
        {
            List<Square> availableMoves = new List<Square>();
            Square position = board.FindPiece(this);
            int offset = this.Player == Player.White ? -1 : 1;

            if (position.Col + 1 <= 7) availableMoves.Add(new Square(position.Row + offset, position.Col + 1));
            if (position.Col - 1 >= 0) availableMoves.Add(new Square(position.Row + offset, position.Col - 1));
            return availableMoves.Where<Square>(s => IsSquareValidForEnPessant(board, player, s)).ToList();
        }

        private bool IsSquareValidForEnPessant(Board board, Player player, Square square)
        {
            int offset = this.Player == Player.White ? 1 : -1;

            Piece? possiblePawn = board.GetPiece(new Square(square.Row + offset, square.Col));
            if (possiblePawn != null && possiblePawn is Pawn)
            {
                Pawn pawn = (Pawn)possiblePawn;
                if (board.lastMovedPiece == pawn && pawn.hasJustMovedTwo)
                {
                    return true;
                }
            }
            return false;
        }

        private List<Square> findCapture(Board board, Square position, int x, int y)
        {
            List<Square> available = new List<Square>();
            int row = position.Row + x;
            int col = position.Col + y;
            if (row == GameSettings.BoardSize || col == GameSettings.BoardSize || row < 0 || col < 0) return available;
            Square newSquare = new Square(row, col);
            if (board.GetPiece(newSquare) != null && board.GetPiece(newSquare)!.Player != board.GetPiece(position)!.Player)
            {
                available.Add(newSquare);
                return available;
            }
            return [];
        }
    }
}