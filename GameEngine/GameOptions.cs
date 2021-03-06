﻿namespace GameEngine
{
    public class GameOptions
    {
        public int ColoredCardsPerColor { get; private set; }
        public int ColoredSpacesPerColor { get; private set; }
        public int SunCards { get; private set; }
        public int SunSpaces { get; private set; }
        public int Owls { get; private set; }

        private GameOptions() { }

        public static GameOptions FromMultiplier(int gameSizeMultiplier)
        {
            return new GameOptions
            {
                ColoredCardsPerColor = gameSizeMultiplier,
                ColoredSpacesPerColor = gameSizeMultiplier,
                SunCards = gameSizeMultiplier,
                SunSpaces = gameSizeMultiplier,
                Owls = gameSizeMultiplier
            };
        }
    }
}
