namespace BRAmongUS
{
    public struct Constants
    {
        public struct Scenes
        {
            public const int MainMenu = 1;
            public const int Game = 2;
            public const int Shop = 3;
            public const int Leaderboard = 4;
        }
        
        public struct Tags
        {
            public const string Player = "Player";
            public const string Enemy = "Enemy";
            public const string AmmoBox = "AmmoBox";
            public const string Coin = "Coin";
            public const string Obstacle = "Obstacle";
        }
        
        public struct IconsTags
        {
            public const string Coin = "<sprite name=\"coin\">";
            public const string Heal = "<sprite name=\"heal\">";
            public const string Shield = "<sprite name=\"shield\">";
            public const string Kill = "<sprite name=\"kill\">";
        }
        
        public struct ID
        {
            public const string LeaderboardID = "CountKill";
        }
    }
}