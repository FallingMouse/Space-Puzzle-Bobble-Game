//using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpacePuzzleBobble.GameObject;
using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static System.Formats.Asn1.AsnWriter;

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
        public int countdown = 15;
        public float timer = 0f;

        public Boolean isPaused = false;
        public Boolean spacebarPressed = false;
        public Boolean isShooting = false;

        public int[,] GameBoard;
        public Bubble[,] _bubbleTable;

        public int posCeiling = 0;

        public KeyboardState PreviousKey, CurrentKey;

        public Random Random = new Random();

        public MouseState MousePrevious, MouseCurrent;

        public enum GameState
        {
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
