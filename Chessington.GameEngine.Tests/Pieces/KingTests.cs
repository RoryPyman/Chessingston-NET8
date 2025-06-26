using System.Collections.Generic;
using Chessington.GameEngine.Pieces;
using FluentAssertions;
using NUnit.Framework;

namespace Chessington.GameEngine.Tests.Pieces
{
    [TestFixture]
    public class KingTests
    {
        [Test]
        public void KingsCanMoveToAdjacentSquares()
        {
            var board = new Board();
            var king = new King(Player.White);
            board.AddPiece(Square.At(4, 4), king);

            var moves = king.GetAvailableMoves(board);

            var expectedMoves = new List<Square>
            {
                Square.At(3, 3),
                Square.At(3, 4),
                Square.At(3, 5),
                Square.At(4, 3),
                Square.At(4, 5),
                Square.At(5, 3),
                Square.At(5, 4),
                Square.At(5, 5)
            };

            moves.Should().BeEquivalentTo(expectedMoves);
        }

        [Test]
        public void Kings_CannotLeaveTheBoard()
        {
            var board = new Board();
            var king = new King(Player.White);
            board.AddPiece(Square.At(0, 0), king);

            var moves = king.GetAvailableMoves(board);

            var expectedMoves = new List<Square>
            {
                Square.At(1, 0),
                Square.At(1, 1),
                Square.At(0, 1)
            };

            moves.Should().BeEquivalentTo(expectedMoves);
        }

        [Test]
        public void Kings_CanTakeOpposingPieces()
        {
            var board = new Board();
            var king = new King(Player.White);
            board.AddPiece(Square.At(4, 4), king);
            var pawn = new Pawn(Player.Black);
            board.AddPiece(Square.At(4, 5), pawn);

            var moves = king.GetAvailableMoves(board);
            moves.Should().Contain(Square.At(4, 5));
        }

        [Test]
        public void Kings_CannotTakeFriendlyPieces()
        {
            var board = new Board();
            var king = new King(Player.White);
            board.AddPiece(Square.At(4, 4), king);
            var pawn = new Pawn(Player.White);
            board.AddPiece(Square.At(4, 5), pawn);

            var moves = king.GetAvailableMoves(board);
            moves.Should().NotContain(Square.At(4, 5));
        }
        [Test]
        public void MovingBlackKingIntoCheck()
        {
            var board = new Board(Player.Black);

            var blackKing = new King(Player.Black);
            var whiteRook = new Rook(Player.White);

            board.AddPiece(Square.At(0, 4), blackKing);   // Black King on row 0, column 4
            board.AddPiece(Square.At(2, 3), whiteRook);   // White Rook on row 2, column 4

            bool causesCheck = board.doesMoveCauseCheck(Square.At(0, 3), blackKing);

            Assert.That(causesCheck, Is.True);
        }

        [Test]
        public void KingCantEscapeCheck()
        {
            var board = new Board(Player.Black);

            var blackKing = new King(Player.Black);
            var whiteQueen = new Queen(Player.White);

            board.AddPiece(Square.At(0, 4), blackKing);   // Black King on row 0, column 4
            board.AddPiece(Square.At(1, 4), whiteQueen);   // White Rook on row 2, column 4

            List<Square> moves = blackKing.GetAvailableMoves(board).ToList();

            moves.Should().BeEquivalentTo([Square.At(1, 4)]);
        }

        [Test]
        public void KingCanCastle()
        {
            var board = new Board(Player.White);

            var king = new King(Player.White);
            var rook = new Rook(Player.White);

            board.AddPiece(Square.At(7, 4), king);
            board.AddPiece(Square.At(7, 7), rook);

            var kingSquare = Square.At(7, 4);
            var rookSquare = Square.At(7, 7);

            var moves = king.GetAvailableMoves(board);

            moves.Should().Contain(Square.At(7, 6));
        }

        [Test]
        public void KingCantCastleAfterMoving()
        {
            var board = new Board(Player.White);

            var king = new King(Player.White);
            var rook = new Rook(Player.White);

            board.AddPiece(Square.At(7, 4), king);
            board.AddPiece(Square.At(7, 7), rook);

            board.MovePiece(Square.At(7, 4), Square.At(7, 5));
            board.MovePiece(Square.At(7, 5), Square.At(7, 4));

            var moves = king.GetAvailableMoves(board);

            Assert.Equals(moves.Contains(Square.At(7, 6)), false);
        }

        [Test]
        public void KingCantCastleThroughCheck()
        {
            var board = new Board(Player.White);

            var king = new King(Player.White);
            var rook = new Rook(Player.White);
            var blackRook = new Rook(Player.Black);

            board.AddPiece(Square.At(7, 4), king);
            board.AddPiece(Square.At(7, 7), rook);

            board.AddPiece(Square.At(0, 5), blackRook);

            var moves = king.GetAvailableMoves(board);
            Assert.Equals(moves.Contains(Square.At(7, 6)), false);
        }

        [Test]
        public void KingCantCastleWhileInCheck()
        {
            var board = new Board(Player.White);

            var king = new King(Player.White);
            var rook = new Rook(Player.White);
            var blackRook = new Rook(Player.Black);

            board.AddPiece(Square.At(7, 4), king);
            board.AddPiece(Square.At(7, 7), rook);

            board.AddPiece(Square.At(0, 4), blackRook);

            var moves = king.GetAvailableMoves(board);
            Assert.Equals(moves.Contains(Square.At(7, 6)), false);
        }
    }
}