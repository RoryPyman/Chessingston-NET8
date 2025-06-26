using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Chessington.GameEngine.Pieces;

namespace Chessington.GameEngine
{
    public class Board
    {
        private readonly Piece?[,] board;
        public Piece lastMovedPiece = null;
        public Square whiteKingLoc = Square.At(7, 4);
        public Square blackKingLoc = Square.At(0, 4);

        public Player CurrentPlayer { get; private set; }
        public IList<Piece> CapturedPieces { get; private set; }

        public Board()
            : this(Player.White) { }

        public Board(Player currentPlayer, Piece[,]? boardState = null)
        {
            board = boardState ?? new Piece[GameSettings.BoardSize, GameSettings.BoardSize];
            CurrentPlayer = currentPlayer;
            CapturedPieces = new List<Piece>();
        }

        public void AddPiece(Square square, Piece pawn)
        {
            board[square.Row, square.Col] = pawn;
        }

        public Piece? GetPiece(Square square)
        {
            return board[square.Row, square.Col];
        }

        public Square FindPiece(Piece piece)
        {
            for (var row = 0; row < GameSettings.BoardSize; row++)
                for (var col = 0; col < GameSettings.BoardSize; col++)
                    if (board[row, col] == piece)
                        return Square.At(row, col);

            throw new ArgumentException("The supplied piece is not on the board.", "piece");
        }

        public void MovePiece(Square from, Square to)
        {
            var movingPiece = board[from.Row, from.Col];
            if (movingPiece == null) { return; }

            if (movingPiece.Player != CurrentPlayer)
            {
                throw new ArgumentException("The supplied piece does not belong to the current player.");
            }

            //If the space we're moving to is occupied, we need to mark it as captured.
            if (board[to.Row, to.Col] != null)
            {
                OnPieceCaptured(board[to.Row, to.Col]!);
            }

            if (movingPiece is King) handleKingMove(movingPiece.Player, to);
            if (movingPiece is Pawn) lastMovedPiece = handlePawnMove((Pawn)movingPiece, from, to);

            else
            {
                lastMovedPiece = movingPiece;
            }
            //Move the piece and set the 'from' square to be empty.
            board[to.Row, to.Col] = lastMovedPiece;
            board[from.Row, from.Col] = null;

            CurrentPlayer = movingPiece.Player == Player.White ? Player.Black : Player.White;
            OnCurrentPlayerChanged(CurrentPlayer);
        }

        private Piece handlePawnMove(Pawn movingPiece, Square from, Square to)
        {
            // Check for En Pessant
            if (to.Col != from.Col && GetPiece(to) == null)
            {
                Square sq = FindPiece(lastMovedPiece);
                OnPieceCaptured(lastMovedPiece);
                board[sq.Row, sq.Col] = null;
            }
            if ((to.Row == 0 && movingPiece.Player == Player.White) || to.Row == 7 && movingPiece.Player == Player.Black)
            {
                return new Queen(movingPiece.Player);
            }

            if (to.Row == from.Row + 2)
            {
                movingPiece.hasJustMovedTwo = true;
            }
            else
            {
                movingPiece.hasJustMovedTwo = false;
            }
            return movingPiece;
        }

        private void handleKingMove(Player player, Square to) {
            if (player == Player.White) whiteKingLoc = to;
            else blackKingLoc = to;

        }

        public bool isSquareInCheck(Square square, Player player)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Piece? piece = board[row, col];
                    if (piece is null || piece.Player == player) continue;

                    if (piece.GetAvailableMoves(this, true).Contains(square)) return true;
                }
            }
            return false;
        }

        public bool doesMoveCauseCheck(Square to, Piece piece)
        {
        Square currentLoc = FindPiece(piece);
        Piece? temp = board[to.Row, to.Col];

        // Backup king positions
        var originalWhiteKingLoc = whiteKingLoc;
        var originalBlackKingLoc = blackKingLoc;

        // If piece is king, simulate king move
        if (piece is King)
        {
            if (piece.Player == Player.White)
                whiteKingLoc = to;
            else
                blackKingLoc = to;
        }

        // Simulate move
        board[to.Row, to.Col] = piece;
        board[currentLoc.Row, currentLoc.Col] = null;

        // If king disappeared for any reason, revert and return false
            if (findKing(piece.Player) == null)
            {
                board[currentLoc.Row, currentLoc.Col] = piece;
                board[to.Row, to.Col] = temp;
                whiteKingLoc = originalWhiteKingLoc;
                blackKingLoc = originalBlackKingLoc;
                return false;
            }

        // Evaluate check
        bool causesCheck = isSquareInCheck(
            piece.Player == Player.White ? whiteKingLoc : blackKingLoc,
            piece.Player
        );

        // Revert simulated move
        board[currentLoc.Row, currentLoc.Col] = piece;
        board[to.Row, to.Col] = temp;
        whiteKingLoc = originalWhiteKingLoc;
        blackKingLoc = originalBlackKingLoc;

        return causesCheck;
    }


        public Square? findKing(Player player)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Piece? piece = board[row, col];
                    if (piece is King && piece.Player == player) return Square.At(row, col);

                }
            }
            return null;
        }


        public delegate void PieceCapturedEventHandler(Piece piece);

        public event PieceCapturedEventHandler? PieceCaptured;

        protected virtual void OnPieceCaptured(Piece piece)
        {
            var handler = PieceCaptured;
            if (handler != null) handler(piece);
        }

        public delegate void CurrentPlayerChangedEventHandler(Player player);

        public event CurrentPlayerChangedEventHandler? CurrentPlayerChanged;

        protected virtual void OnCurrentPlayerChanged(Player player)
        {
            var handler = CurrentPlayerChanged;
            if (handler != null) handler(player);
        }
    }
}
