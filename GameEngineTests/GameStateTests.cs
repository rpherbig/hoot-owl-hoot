﻿using GameEngine;
using GameEngine.Players;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace GameEngineTests
{
    [TestClass]
    public class GameStateTests
    {
        private IPlayer Player;
        private Deck Deck;
        private GameState State;
        private int Multiplier;
        private int NumberOfOwls;

        [TestInitialize]
        public void TestInitialize()
        {
            Player = new LeastRecentCardPlayer();
            Multiplier = 6;
            NumberOfOwls = 6;
            Deck = new Deck(Multiplier);
            State = new GameState(Player, Deck, Multiplier);
        }

        [TestMethod]
        public void ShouldStartGame()
        {
            var startingDeckSize = State.Deck.Cards.Count;

            State.StartGame();

            Assert.AreEqual(0, State.SunCounter);
            Assert.AreEqual(3, State.Player.Hand.Count);
            Assert.AreEqual(startingDeckSize - 3, State.Deck.Cards.Count);
        }

        [TestMethod]
        public void ShouldTakeOneTurnWithoutSunCard()
        {
            Deck.Cards.InsertRange(0, new List<CardType> {
                CardType.Red,
                CardType.Orange,
                CardType.Yellow,
                CardType.Green
            });
            State.StartGame();

            State.TakeTurn();

            var expectedHand = new List<CardType> {
                CardType.Orange,
                CardType.Yellow,
                CardType.Green
            };
            CollectionAssert.AreEqual(expectedHand, State.Player.Hand);
            Assert.IsTrue(State.Board.Owls.Inhabit(6));
            Assert.AreEqual(42, State.Deck.Cards.Count);
            Assert.AreEqual(0, State.SunCounter);
        }

        [TestMethod]
        public void ShouldPlaySunCardFirstWhenPossible()
        {
            Deck.Cards.InsertRange(0, new List<CardType> {
                CardType.Orange,
                CardType.Yellow,
                CardType.Sun,
                CardType.Green
            });
            State.StartGame();

            State.TakeTurn();

            var expectedHand = new List<CardType> {
                CardType.Orange,
                CardType.Yellow,
                CardType.Green
            };
            var expectedOwlPositions = Enumerable.Range(0, 6).ToArray();
            CollectionAssert.AreEqual(expectedHand, State.Player.Hand);
            State.Board.AssertOwlPositionsMatch(expectedOwlPositions);
            Assert.AreEqual(42, State.Deck.Cards.Count);
            Assert.AreEqual(1, State.SunCounter);
        }

        [TestMethod]
        public void ShouldWinGameWhenOwlsReachNest()
        {
            var redCards = Enumerable.Repeat(CardType.Red, Multiplier * NumberOfOwls);
            Deck.Cards.InsertRange(0, redCards);
            State.StartGame();

            foreach (int expectedNestedOwls in Enumerable.Range(0, NumberOfOwls))
            {
                var owlsStillInStartingPositions = Enumerable.Range(0, NumberOfOwls - expectedNestedOwls).ToArray();

                State.Board.AssertOwlPositionsMatch(owlsStillInStartingPositions);
                Assert.AreEqual(expectedNestedOwls, State.Board.Owls.InTheNest);
                Assert.IsFalse(State.GameIsWon());

                foreach (int playsForThisOwl in Enumerable.Range(0, Multiplier))
                {
                    Assert.AreEqual(CardType.Red, State.Player.Hand[0]);

                    State.TakeTurn();
                    
                    int expectedOwlsAtStart = NumberOfOwls - expectedNestedOwls - 1;
                    var expectedNewPosition = expectedNestedOwls + Multiplier * (playsForThisOwl + 1);
                    var expectedPositions = Enumerable.Range(0, expectedOwlsAtStart)
                        .Concat(new[] { expectedNewPosition })
                        .ToArray();
                    State.Board.AssertOwlPositionsMatch(expectedPositions);
                }
            }

            Assert.AreEqual(NumberOfOwls, State.Board.Owls.InTheNest);
            Assert.AreEqual(0, State.SunCounter);
            Assert.IsTrue(State.GameIsWon());
        }

        [TestMethod]
        public void ShouldLoseGameWhenSunCounterReachesMaximum()
        {
            var cards = Enumerable.Repeat(CardType.Sun, Multiplier);
            Deck.Cards.InsertRange(0, cards);
            State.StartGame();

            Assert.IsFalse(State.GameIsLost());

            var allOwlsAtStartPosition = Enumerable.Range(0, NumberOfOwls).ToArray();
            for (int i = 0; i < Multiplier; i++)
            {
                Assert.AreEqual(CardType.Sun, State.Player.Hand[0]);

                State.TakeTurn();

                State.Board.AssertOwlPositionsMatch(allOwlsAtStartPosition);
            }

            Assert.AreEqual(Multiplier, State.SunCounter);
            Assert.IsTrue(State.GameIsLost());
        }
    }
}
