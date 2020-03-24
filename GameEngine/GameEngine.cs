﻿using GameEngine.Agents;
using System;

namespace GameEngine
{
    public class GameEngine
    {
        public void RunGame()
        {
            var player = new RandomAgent();
            int numberOfTurns = 0;
            var game = new Game(4);
            while(!game.IsOver)
            {
                game.TakeTurn(player);
                numberOfTurns++;
            }
            Console.WriteLine("Game is {0}!", game.IsLost ? "lost" : "won");
            Console.WriteLine("Game took {0} turns!", numberOfTurns);
        }
    }
}
