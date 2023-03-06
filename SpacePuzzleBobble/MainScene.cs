using System;
using System.ComponentModel.Design;
using static System.Formats.Asn1.AsnWriter;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpacePuzzleBobble.GameObject;

namespace SpacePuzzleBobble
{
    public class MainScene : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Crosshair _crosshair;
        Bubble _bubbleNextOne, _bubbleNextTwo;
        Bubble[,] _bubbleTable;

        Texture2D _backgroundTexture, _rectTestTexture, _crosshairTexture;
        Texture2D[] _bubbleTexture;

        SpriteFont _font;

        public MainScene()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // 1920, 1080 (16:9 Resolution)
            Window.Title = "UFO Khon Kaen";
            Window.AllowUserResizing= true;
            //Window.IsBorderless= true;
            _graphics.PreferredBackBufferWidth = Singleton.SCREENWIDTH;
            _graphics.PreferredBackBufferHeight = Singleton.SCREENHEIGHT;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load Texture
            _backgroundTexture = this.Content.Load<Texture2D>("Background/background");
            _crosshairTexture = this.Content.Load<Texture2D>("Crosshair/crosshair");

            _bubbleTexture = new Texture2D[5];
            _bubbleTexture[0] = this.Content.Load<Texture2D>("Bubble/red");
            _bubbleTexture[1] = this.Content.Load<Texture2D>("Bubble/blue");
            _bubbleTexture[2] = this.Content.Load<Texture2D>("Bubble/green");
            _bubbleTexture[3] = this.Content.Load<Texture2D>("Bubble/yellow");
            _bubbleTexture[4] = this.Content.Load<Texture2D>("Bubble/pink");

            _rectTestTexture = new Texture2D(_graphics.GraphicsDevice, 1, 1);

            Color[] data = new Color[1];
            for(int i = 0; i< data.Length; i++) data[i] = Color.White;
            _rectTestTexture.SetData(data);

            _font = Content.Load<SpriteFont>("Font/GameFont");

            Reset();
        }

        protected override void Update(GameTime gameTime)
        {
            Singleton.Instance.CurrentKey = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Score
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Space) && !Singleton.Instance.spacebarPressed)
            {
                Singleton.Instance.Score += 100;
                Singleton.Instance.spacebarPressed = true;
            }
            if (keyboardState.IsKeyUp(Keys.Space))
            {
                Singleton.Instance.spacebarPressed = false;
            }


            // TODO: Add your update logic here
            _crosshair.Update(gameTime);
            
            //Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Draw Background
            _spriteBatch.Draw(_backgroundTexture, new Vector2(0,0), Color.White);

            _bubbleNextOne.Draw(_spriteBatch);
            _bubbleNextTwo.Draw(_spriteBatch);

            //Score
            _spriteBatch.DrawString(_font, "Score: " + Singleton.Instance.Score.ToString(), new Vector2(10, 10), Color.White);

            // Draw Crosshair
            _crosshair.Draw(_spriteBatch);

            //Draw starting pattern bubbles
            //PS : when the game state is set-up  (Idle : Defualt state)
            for (int i = 0; i < Singleton.Instance.GameBoard.GetLength(0); i++)
            {
                for (int j = 0; j < Singleton.Instance.GameBoard.GetLength(1); j++)
                {
                    // Red Table
                    /*Rectangle _rectTable = new Rectangle((j * Singleton.TILESIZE) + (Singleton.TILESIZE * 11) + ((i % 2) * (Singleton.TILESIZE / 2)),
                                                    (int)i * Singleton.TILESIZE + (Singleton.TILESIZE), Singleton.TILESIZE, Singleton.TILESIZE);
                    _spriteBatch.Draw(_rectTestTexture, new Rectangle(_rectTable.X, _rectTable.Y, Singleton.TILESIZE, 1), new Color(Color.Red, 100));
                    _spriteBatch.Draw(_rectTestTexture, new Rectangle(_rectTable.X, _rectTable.Y, 1, Singleton.TILESIZE), new Color(Color.Red, 100));*/

                    if (_bubbleTable[i,j] != null)
                        _bubbleTable[i, j].Draw(_spriteBatch);
                }
            }

            _spriteBatch.End();
            _graphics.BeginDraw();

            base.Draw(gameTime);
        }

        protected void Reset()
        {
            //Starting pattern bubbles
            Singleton.Instance.GameBoard = new int[Singleton.GAMEHEIGHT, Singleton.GAMEWIDTH]
            {
            /*        0    1   2  3   4   5   6   7  */
            /*0*/    {0   ,0 , 1 ,1  ,2  ,2  ,3  ,3  },
            /*1*/    {0   ,0 , 1 ,1  ,2  ,2  ,3  ,-1  },
            /*2*/    {1   ,1 , 2 ,2  ,3  ,3  ,0  ,0  },
            /*3*/    {1   ,1 , 2 ,2  ,3  ,3  ,0  ,-1  },
            /*4*/    {-1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 },
            /*5*/    {-1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 },
            /*6*/    {-1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 },
            /*7*/    {-1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 },
            /*8*/    {-1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 }
            };

            _crosshair = new Crosshair(_crosshairTexture)
            {
                Position = new Vector2(Singleton.SCREENWIDTH / 2, Singleton.SCREENHEIGHT)
            };

            _bubbleNextOne = new Bubble(_bubbleTexture)
            {
                Position = new Vector2((Singleton.SCREENWIDTH / 2) - Singleton.TILESIZE * 1.40f,
                            (Singleton.TILESIZE * 11) + Singleton.TILESIZE / 8f),
                Scale = new Vector2(1/1.45f, 1/1.45f)
            };

            _bubbleNextTwo = new Bubble(_bubbleTexture)
            {
                Position = new Vector2((Singleton.SCREENWIDTH / 2) - Singleton.TILESIZE * 3.6f, 
                            Singleton.TILESIZE * 12.3f),
                Scale = new Vector2(0.28f, 0.28f)
            };

            _bubbleTable = new Bubble[Singleton.GAMEHEIGHT, Singleton.GAMEWIDTH];

            for(int i = 0; i < Singleton.Instance.GameBoard.GetLength(0); i++)
            {
                for (int j = 0; j < Singleton.Instance.GameBoard.GetLength(1); j++)
                {
                    Rectangle _rectTable = new Rectangle((j * Singleton.TILESIZE) + (Singleton.TILESIZE * 11) + ((i % 2) * (Singleton.TILESIZE / 2)),
                                                (int)i * Singleton.TILESIZE + (Singleton.TILESIZE), Singleton.TILESIZE, Singleton.TILESIZE);

                    if (Singleton.Instance.GameBoard[i, j] != -1)
                    {
                        _bubbleTable[i, j] = new Bubble(_bubbleTexture[Singleton.Instance.GameBoard[i, j]])
                        {
                            
                            Position = new Vector2(_rectTable.X, _rectTable.Y),
                            // Old Position(not in table)
                            //Position = new Vector2(Singleton.TILESIZE * j + 703, Singleton.TILESIZE * i + Singleton.TILESIZE),
                            Scale = new Vector2(0.25f, 0.25f)
                        };
                    }
                }
            }

            Singleton.Instance.Score = 0;
        }
    }
}