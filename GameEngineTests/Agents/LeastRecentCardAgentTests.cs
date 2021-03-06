﻿using GameEngine;
using GameEngine.Agents;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameEngineTests.Agents
{
    [TestClass]
    public class LeastRecentCardAgentTests
    {
        [TestMethod]
        public void ShouldPlayOldestCardFromHand()
        {
            var state = TestUtilities.GenerateTestState(2);
            var firstCard = CardType.Red;
            var secondCard = CardType.Orange;
            state.CurrentPlayerHand.Cards.Clear();
            state.CurrentPlayerHand.Add(firstCard, secondCard);
            var player = new LeastRecentCardAgent();

            var play = player.FormulatePlay(state);
            Assert.AreEqual(firstCard, play.Card);

            state.CurrentPlayerHand.Discard(play.Card);
            play = player.FormulatePlay(state);
            Assert.AreEqual(secondCard, play.Card);
        }
    }
}
