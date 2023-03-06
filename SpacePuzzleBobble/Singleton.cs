using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace SpacePuzzleBobble
{
    class Singleton
    {
        private static Singleton instance;

        public const int TILESIZE = 64;

        public const int GAMEWIDTH = 8;
        public const int GAMEHEIGHT = 9;

        public const int SCREENWIDTH = 1920;
        public const int SCREENHEIGHT = 1080;

        public int Score;
        public int Level;

        public Boolean spacebarPressed = false;
        public Boolean isShooting = false;

        public int[,] GameBoard;

        public KeyboardState PreviousKey, CurrentKey;

        public Random Random = new Random();

        public enum GameState
        {
            Idle, // Default state
            Playing,
            Paused,
            GameWon,
            GameOver, // Lost
        }
        public GameState _currentGameState;


        private Singleton()
        {
        }

        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }
    }
}
