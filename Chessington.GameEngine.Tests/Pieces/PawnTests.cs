﻿using System.Linq;
using Chessington.GameEngine.Pieces;
using FluentAssertions;
using NUnit.Framework;

namespace Chessington.GameEngine.Tests.Pieces
{
    [TestFixture]
    public class PawnTests
    {
        [Test]
        public void WhitePawns_CanMoveOneSquareUp()
        {
            var board = new Board();
            var pawn = new Pawn(Player.White);
            board.AddPiece(Square.At(7, 0), pawn);

            var moves = pawn.GetAvailableMoves(board);

            moves.Should().Contain(Square.At(6, 0));
        }

        [Test]
        public void BlackPawns_CanMoveOneSquareDown()
        {
            var board = new Board();
            var pawn = new Pawn(Player.Black);
            board.AddPiece(Square.At(1, 0), pawn);

            var moves = pawn.GetAvailableMoves(board);

            moves.Should().Contain(Square.At(2, 0));
        }

        [Test]
        public void WhitePawns_WhichHaveNeverMoved_CanMoveTwoSquareUp()
        {
            var board = new Board();
            var pawn = new Pawn(Player.White);
            board.AddPiece(Square.At(6, 5), pawn);

            var moves = pawn.GetAvailableMoves(board);

            moves.Should().Contain(Square.At(4, 5));
        }

        [Test]
        public void BlackPawns_WhichHaveNeverMoved_CanMoveTwoSquareUp()
        {
            var board = new Board();
            var pawn = new Pawn(Player.Black);
            board.AddPiece(Square.At(1, 3), pawn);

            var moves = pawn.GetAvailableMoves(board);

            moves.Should().Contain(Square.At(3, 3));
        }

        [Test]
        public void WhitePawns_WhichHaveAlreadyMoved_CanOnlyMoveOneSquare()
        {
            var board = new Board();
            var pawn = new Pawn(Player.White);
            board.AddPiece(Square.At(6, 2), pawn);

            pawn.MoveTo(board, Square.At(5, 2));
            var moves = pawn.GetAvailableMoves(board).ToList();

            moves.Should().HaveCount(1);
            moves.Should().Contain(square => square.Equals(Square.At(4, 2)));
        }

        [Test]
        public void BlackPawns_WhichHaveAlreadyMoved_CanOnlyMoveOneSquare()
        {
            var board = new Board(Player.Black);
            var pawn = new Pawn(Player.Black);
            board.AddPiece(Square.At(5, 2), pawn);

            pawn.MoveTo(board, Square.At(6, 2));
            var moves = pawn.GetAvailableMoves(board).ToList();

            moves.Should().HaveCount(1);
            moves.Should().Contain(square => square.Equals(Square.At(7, 2)));
        }

        [Test]
        public void Pawns_CannotMove_IfThereIsAPieceInFront()
        {
            var board = new Board();
            var pawn = new Pawn(Player.Black);
            var blockingPiece = new Rook(Player.White);
            board.AddPiece(Square.At(1, 3), pawn);
            board.AddPiece(Square.At(2, 3), blockingPiece);

            var moves = pawn.GetAvailableMoves(board);

            moves.Should().BeEmpty();
        }

        [Test]
        public void Pawns_CannotMoveTwoSquares_IfThereIsAPieceTwoSquaresInFront()
        {
            var board = new Board();
            var pawn = new Pawn(Player.Black);
            var blockingPiece = new Rook(Player.White);
            board.AddPiece(Square.At(1, 3), pawn);
            board.AddPiece(Square.At(3, 3), blockingPiece);

            var moves = pawn.GetAvailableMoves(board);

            moves.Should().NotContain(Square.At(3, 3));
        }

        [Test]
        public void WhitePawns_CannotMove_AtTheTopOfTheBoard()
        {
            var board = new Board();
            var pawn = new Pawn(Player.White);
            board.AddPiece(Square.At(0, 3), pawn);

            var moves = pawn.GetAvailableMoves(board);

            moves.Should().BeEmpty();
        }

        [Test]
        public void BlackPawns_CannotMove_AtTheBottomOfTheBoard()
        {
            var board = new Board();
            var pawn = new Pawn(Player.Black);
            board.AddPiece(Square.At(7, 3), pawn);

            var moves = pawn.GetAvailableMoves(board);

            moves.Should().BeEmpty();
        }

        [Test]
        public void BlackPawns_CanMoveDiagonally_IfThereIsAPieceToTake()
        {
            var board = new Board();
            var pawn = new Pawn(Player.Black);
            var firstTarget = new Pawn(Player.White);
            var secondTarget = new Pawn(Player.White);
            board.AddPiece(Square.At(5, 3), pawn);
            board.AddPiece(Square.At(6, 4), firstTarget);
            board.AddPiece(Square.At(6, 2), secondTarget);

            var moves = pawn.GetAvailableMoves(board).ToList();

            moves.Should().Contain(Square.At(6, 2));
            moves.Should().Contain(Square.At(6, 4));
        }

        [Test]
        public void WhitePawns_CanMoveDiagonally_IfThereIsAPieceToTake()
        {
            var board = new Board();
            var pawn = new Pawn(Player.White);
            var firstTarget = new Pawn(Player.Black);
            var secondTarget = new Pawn(Player.Black);
            board.AddPiece(Square.At(7, 3), pawn);
            board.AddPiece(Square.At(6, 4), firstTarget);
            board.AddPiece(Square.At(6, 2), secondTarget);

            var moves = pawn.GetAvailableMoves(board).ToList();

            moves.Should().Contain(Square.At(6, 2));
            moves.Should().Contain(Square.At(6, 4));
        }

        [Test]
        public void BlackPawns_CannotMoveDiagonally_IfThereIsNoPieceToTake()
        {
            var board = new Board();
            var pawn = new Pawn(Player.Black);
            board.AddPiece(Square.At(5, 3), pawn);

            var friendlyPiece = new Pawn(Player.Black);
            board.AddPiece(Square.At(6, 2), friendlyPiece);

            var moves = pawn.GetAvailableMoves(board).ToList();

            moves.Should().NotContain(Square.At(6, 2));
            moves.Should().NotContain(Square.At(6, 4));
        }

        [Test]
        public void WhitePawns_CannotMoveDiagonally_IfThereIsNoPieceToTake()
        {
            var board = new Board();
            var pawn = new Pawn(Player.White);
            board.AddPiece(Square.At(7, 3), pawn);

            var friendlyPiece = new Pawn(Player.White);
            board.AddPiece(Square.At(6, 2), friendlyPiece);

            var moves = pawn.GetAvailableMoves(board).ToList();

            moves.Should().NotContain(Square.At(6, 2));
            moves.Should().NotContain(Square.At(6, 4));
        }

        [Test]
        public void WhitePawns_CanEnPessant()
        {
            var board = new Board(Player.Black);
            var blackPawn = new Pawn(Player.Black);
            var whitePawn = new Pawn(Player.White);

            board.AddPiece(Square.At(3, 2), whitePawn);
            board.AddPiece(Square.At(1, 1), blackPawn);
            blackPawn.MoveTo(board, Square.At(3, 1));
            var moves = whitePawn.GetAvailableMoves(board).ToList();
            moves.Should().BeEquivalentTo([Square.At(2, 1), Square.At(2, 2)]);

        }

        [Test]
        public void PawnsCaptureOn_EnPessant()
        {
            var board = new Board(Player.Black);
            var blackPawn = new Pawn(Player.Black);
            var whitePawn = new Pawn(Player.White);

            board.AddPiece(Square.At(3, 2), whitePawn);
            board.AddPiece(Square.At(1, 1), blackPawn);
            blackPawn.MoveTo(board, Square.At(3, 1));
            var moves = whitePawn.GetAvailableMoves(board).ToList();
            whitePawn.MoveTo(board, Square.At(2, 1));
            Assert.That(board.GetPiece(Square.At(3, 1)), Is.Null);

        }

        [Test]
        public void WhitePawns_CantEnPessantWhenMovedOnce()
        {
            var board = new Board(Player.Black);
            var blackPawn = new Pawn(Player.Black);
            var whitePawn = new Pawn(Player.White);
            var whiteRook = new Rook(Player.White);

            board.AddPiece(Square.At(3, 2), whitePawn);
            board.AddPiece(Square.At(6, 6), whiteRook);
            board.AddPiece(Square.At(1, 1), blackPawn);
            blackPawn.MoveTo(board, Square.At(2, 1));
            whiteRook.MoveTo(board, Square.At(7, 6));
            blackPawn.MoveTo(board, Square.At(3, 1));
            var moves = whitePawn.GetAvailableMoves(board).ToList();
            moves.Should().BeEquivalentTo([Square.At(2, 2)]);

        }

        public void WhitePawns_CantEnPessantAfterOtherMove()
        {
            var board = new Board(Player.Black);
            var blackPawn = new Pawn(Player.Black);
            var whitePawn = new Pawn(Player.White);
            var blackRook = new Rook(Player.Black);
            var whiteRook = new Rook(Player.White);

            board.AddPiece(Square.At(2, 3), whitePawn);
            board.AddPiece(Square.At(1, 1), blackPawn);
            board.AddPiece(Square.At(5, 5), blackRook);
            board.AddPiece(Square.At(6, 6), whiteRook);
            blackPawn.MoveTo(board, Square.At(1, 3));
            whiteRook.MoveTo(board, Square.At(7, 6));
            blackRook.MoveTo(board, Square.At(4, 5));

            var moves = whitePawn.GetAvailableMoves(board).ToList();
            moves.Should().BeEquivalentTo([Square.At(2, 2)]);

        }

                [Test]
        public void WhitePawn_PromotesToQueen_AtLastRow()
        {
            // Arrange
            var board = new Board();
            var pawn = new Pawn(Player.White);
            var startSquare = new Square(1, 4);
            var endSquare = new Square(0, 4);

            board.AddPiece(startSquare, pawn);

            // Act
            board.MovePiece(startSquare, endSquare);
            var promotedPiece = board.GetPiece(endSquare);

            // Assert
            Assert.That(promotedPiece, Is.InstanceOf<Queen>());
            Assert.That(promotedPiece!.Player, Is.EqualTo(Player.White));
        }

        [Test]
        public void BlackPawn_PromotesToQueen_AtLastRow()
        {
            // Arrange
            var board = new Board(Player.Black);
            var pawn = new Pawn(Player.Black);
            var startSquare = new Square(6, 3);
            var endSquare = new Square(7, 3);

            board.AddPiece(startSquare, pawn);

            // Act
            board.MovePiece(startSquare, endSquare);
            var promotedPiece = board.GetPiece(endSquare);

            // Assert
            Assert.That(promotedPiece, Is.InstanceOf<Queen>());
            Assert.That(promotedPiece!.Player, Is.EqualTo(Player.Black));
        }

        [Test]
        public void Pawn_DoesNotPromote_BeforeFinalRow()
        {
            // Arrange
            var board = new Board();
            var pawn = new Pawn(Player.White);
            var startSquare = new Square(2, 4);
            var endSquare = new Square(1, 4);

            board.AddPiece(startSquare, pawn);

            // Act
            board.MovePiece(startSquare, endSquare);
            var piece = board.GetPiece(endSquare);

            // Assert
            Assert.That(piece, Is.InstanceOf<Pawn>());
        }
    }
}