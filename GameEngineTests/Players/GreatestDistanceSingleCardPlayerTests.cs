﻿using System.Linq;
using GameEngine;
using GameEngine.Players;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameEngineTests.Players
{
    [TestClass]
    public class GreatestDistanceSingleCardPlayerTests
    {
        [TestMethod]
        public void ShouldPlayCardOnOnlyOwlThatGoesFurthest()
        {
            var player = new GreatestDistanceSingleCardPlayer();
            var board = new GameBoard(2, 1);
            player.AddCardsToHand(CardTypeExtensions.OneCardOfEachColor);

            var play = player.FormulatePlay(board);

            Assert.AreEqual(CardType.Red, play.Card);
            Assert.AreEqual(0, play.Position);
        }

        [TestMethod]
        public void ShouldPlayCardOnOwlThatGoesFurthestWithHootingIntoNest()
        {
            var player = new GreatestDistanceSingleCardPlayer();
            var board = new GameBoard(2);
            board.Owls.Move(0, 6);
            player.AddCardsToHand(CardTypeExtensions.OneCardOfEachColor);

            var play = player.FormulatePlay(board);

            Assert.AreEqual(CardType.Red, play.Card);
            Assert.AreEqual(1, play.Position);
        }
    }
}