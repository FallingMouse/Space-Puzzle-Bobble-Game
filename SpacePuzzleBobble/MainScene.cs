using System;
using System.ComponentModel.Design;
using static System.Formats.Asn1.AsnWriter;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpacePuzzleBobble.GameObject;
using System.Collections.Generic;
using System.Reflection;

namespace SpacePuzzleBobble
{
    public class MainScene : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Crosshair _crosshair;
        Bubble _bubbleNextOne, _bubbleNextTwo, _bubbleMonitor;
        //Bubble[,] _bubbleTable;

        Texture2D _backgroundTexture, _monitorTexture, _rectTestTexture, _crosshairTexture;
        Texture2D[] _bubbleTexture;

        SpriteFont _font2;
        SpriteFont _font1;

        public MainScene()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // 1920, 1080 (16:9 Resolution)
            Window.Title = "Stardenburdenhardenbart Puzzle";
            Window.AllowUserResizing= true;
            Window.IsBorderless= true;
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
            _monitorTexture = this.Content.Load<Texture2D>("Background/monitor");
            _crosshairTexture = this.Content.Load<Texture2D>("Crosshair/crosshair");

            _bubbleTexture = new Texture2D[6];
            _bubbleTexture[0] = this.Content.Load<Texture2D>("Bubble/red");
            _bubbleTexture[1] = this.Content.Load<Texture2D>("Bubble/blue");
            _bubbleTexture[2] = this.Content.Load<Texture2D>("Bubble/green");
            _bubbleTexture[3] = this.Content.Load<Texture2D>("Bubble/yellow");
            _bubbleTexture[4] = this.Content.Load<Texture2D>("Bubble/pink");

            _rectTestTexture = new Texture2D(_graphics.GraphicsDevice, 1, 1);

            Color[] data = new Color[1];
            for(int i = 0; i< data.Length; i++) data[i] = Color.White;
            _rectTestTexture.SetData(data);
            _bubbleTexture[5] = _rectTestTexture;

            _font2 = Content.Load<SpriteFont>("Font/GameFont2");
            _font1 = Content.Load<SpriteFont>("Font/GameFont1");

            Reset();
        }

        protected override void Update(GameTime gameTime)
        {
            Singleton.Instance.CurrentKey = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Score
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Up) && !Singleton.Instance.spacebarPressed)
            {
                Singleton.Instance.Score += 100;
                Singleton.Instance.spacebarPressed = true;
            }
            if (keyboardState.IsKeyUp(Keys.Up))
            {
                Singleton.Instance.spacebarPressed = false;
            }

            _bubbleMonitor = new Bubble(_bubbleTexture[_bubbleNextOne.color])
            {
                Position = new Vector2((Singleton.SCREENWIDTH / 2) - Singleton.TILESIZE * 1.40f,
                                           (Singleton.TILESIZE * 11) + Singleton.TILESIZE / 8f),
                Scale = new Vector2(1 / 1.45f, 1 / 1.45f),
            };

            // TODO: Add your update logic here
            _crosshair.Update(gameTime);
            _bubbleNextOne.Update(gameTime);

            //test change to next bubble, when the first one was shooted
            //actually this action MUST be in game state, but have to check if it's work
            //so  I was coding it down here
            if (_bubbleNextOne.IsDead)
            {
                //Reset();
            }
            else if (_bubbleNextOne.IsHitTop)
            {
                // Update Bubble Table
                //Singleton.Instance.GameBoard[_bubbleNextOne.fx, _bubbleNextOne.fy] = _bubbleNextOne.color;
                //Singleton.Instance._bubbleTable[_bubbleNextOne.fx, _bubbleNextOne.fy] = _bubbleNextOne;

                // Change Bubble to next Bubble
                _bubbleNextOne = _bubbleNextTwo;
                _crosshair.setBubbleNextOne(_bubbleNextOne);  //change color in Crosshair.cs

                _bubbleNextOne.Position = new Vector2((Singleton.SCREENWIDTH / 2) - Singleton.TILESIZE * 1.40f,
                            (Singleton.TILESIZE * 11) + Singleton.TILESIZE / 8f);
                _bubbleNextOne.Scale = new Vector2(1 / 1.45f, 1 / 1.45f);

                _bubbleNextTwo = new Bubble(_bubbleTexture)
                {
                    Position = new Vector2((Singleton.SCREENWIDTH / 2) - Singleton.TILESIZE * 3.6f,
                            Singleton.TILESIZE * 12.3f),
                    Scale = new Vector2(0.28f, 0.28f)
                };

                _bubbleNextTwo.Reset();
            }

            Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Draw Background
            _spriteBatch.Draw(_backgroundTexture, new Vector2(0, 0), Color.White);

            _bubbleNextOne.Draw(_spriteBatch);

            // Draw Monitor
            _spriteBatch.Draw(_monitorTexture, new Vector2(0, 0), Color.White);

            _bubbleMonitor.Draw(_spriteBatch);
            _bubbleNextTwo.Draw(_spriteBatch);

            // Draw Score
            //_spriteBatch.DrawString(_font1, "SCORE ", new Vector2(220, 850), Color.White);
            //_spriteBatch.DrawString(_font, Singleton.Instance.Score.ToString("D6"), new Vector2(179, 900), Color.White);

            // Draw Score
            float rotationScore1 = MathHelper.ToRadians(-12.2f);
            float rotationScore2 = MathHelper.ToRadians(-13.8f);
            _spriteBatch.DrawString(_font1, "SCORE", new Vector2(270, 858), Color.White, rotationScore1, _font1.MeasureString("SCORE") / 2f, 1f, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(_font2, Singleton.Instance.Score.ToString("D6"), new Vector2(245, 905), Color.White, rotationScore2, _font1.MeasureString("SCORE") / 2f, 1f, SpriteEffects.None, 0f);

            // Draw CountDown
            float rotationCountDown = MathHelper.ToRadians(12.0f);
            _spriteBatch.DrawString(_font2, "60", new Vector2(1530, 870), Color.White, rotationCountDown, _font2.MeasureString("60") / 2f, 1f, SpriteEffects.None, 0f);

            // Draw Crosshair
            _crosshair.Draw(_spriteBatch);

            //Draw starting pattern bubbles
            //PS : when the game state is set-up  (Idle : Defualt state)
            for (int i = 0; i < Singleton.Instance.GameBoard.GetLength(0); i++)
            {
                for (int j = 0; j < Singleton.Instance.GameBoard.GetLength(1); j++)
                {
                    if (Singleton.Instance._bubbleTable[i,j] != null)
                        Singleton.Instance._bubbleTable[i, j].Draw(_spriteBatch);
                }
            }

            _spriteBatch.End();
            _graphics.BeginDraw();

            base.Draw(gameTime);
        }

        protected void Reset()
        {
            //Starting pattern bubbles
            /*Singleton.Instance.GameBoard = new int[Singleton.GAMEHEIGHT, Singleton.GAMEWIDTH];

            for (int i = 0; i < Singleton.Instance.GameBoard.GetLength(0); i++)
            {
                for (int j = 0; j < Singleton.Instance.GameBoard.GetLength(1); j++)
                {
                     Singleton.Instance.GameBoard[i, j] = 5;
                }
            }*/

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

            Singleton.Instance._bubbleTable = new Bubble[Singleton.GAMEHEIGHT, Singleton.GAMEWIDTH];

            // Bubble Table
            for (int i = 0; i < Singleton.Instance.GameBoard.GetLength(0); i++)
            {
                for (int j = 0; j < Singleton.Instance.GameBoard.GetLength(1); j++)
                {
                    Rectangle _rectTable = new Rectangle((j * Singleton.TILESIZE) + (Singleton.TILESIZE * 11) + ((i % 2) * (Singleton.TILESIZE / 2)),
                                                (int)i * Singleton.TILESIZE + (Singleton.TILESIZE), Singleton.TILESIZE, Singleton.TILESIZE);

                    if (Singleton.Instance.GameBoard[i, j] != -1)
                    {
                        Singleton.Instance._bubbleTable[i, j] = new Bubble(_bubbleTexture[Singleton.Instance.GameBoard[i, j]])
                        {
                            Position = new Vector2(_rectTable.X, _rectTable.Y),
                            Scale = new Vector2(0.25f, 0.25f)
                        };
                        Singleton.Instance._bubbleTable[i, j].Reset();
                        Singleton.Instance._bubbleTable[i, j].color = Singleton.Instance.GameBoard[i, j];
                    }
                    else
                    {
                        Singleton.Instance._bubbleTable[i, j] = new Bubble(_bubbleTexture[5])
                        {
                            Position = new Vector2(_rectTable.X, _rectTable.Y),
                            Scale = new Vector2(0.25f, 0.25f)
                        };
                        Singleton.Instance._bubbleTable[i, j].Reset();
                        Singleton.Instance._bubbleTable[i, j].color = 5;
                    }
                    //refer to index of array
                    int fy = (int)(Singleton.Instance._bubbleTable[i, j].Position.Y - Singleton.TILESIZE + (Singleton.TILESIZE / 2)) / Singleton.TILESIZE;
                    int fx = (int)((Singleton.Instance._bubbleTable[i, j].Position.X - (Singleton.TILESIZE * 11) + (Singleton.TILESIZE / 2) - ((fy % 2) * (Singleton.TILESIZE / 2))) / Singleton.TILESIZE);

                    Singleton.Instance._bubbleTable[i, j]._positionBox = new Vector2(fx, fy);

                    //refer to position on game board
                    Rectangle boundary = new Rectangle((fx * Singleton.TILESIZE) + (Singleton.TILESIZE * 11) + ((fy % 2) * (Singleton.TILESIZE / 2)),
                                    (int)(fy * Singleton.TILESIZE) + Singleton.TILESIZE, Singleton.TILESIZE, Singleton.TILESIZE);
                    Singleton.Instance._bubbleTable[i, j]._positionBubble = new Vector2(boundary.X, boundary.Y);

                    //Singleton.Instance.GameBoard[fy, fx] = Singleton.Instance._bubbleTable[i, j].color; // Not Equal -1 but it's not _bubbleTexture
                    
                }
            }

            _bubbleNextOne = new Bubble(_bubbleTexture)
            {
                Position = new Vector2((Singleton.SCREENWIDTH / 2) - Singleton.TILESIZE * 1.40f,
                            (Singleton.TILESIZE * 11) + Singleton.TILESIZE / 8f),
                Scale = new Vector2(1/1.45f, 1/1.45f)
            };

            _bubbleNextOne.Reset();

            _bubbleNextTwo = new Bubble(_bubbleTexture)
            {
                Position = new Vector2((Singleton.SCREENWIDTH / 2) - Singleton.TILESIZE * 3.6f, 
                            Singleton.TILESIZE * 12.3f),
                Scale = new Vector2(0.28f, 0.28f)
            };

            _bubbleNextTwo.Reset();

            _crosshair = new Crosshair(_crosshairTexture, _bubbleNextOne, _bubbleNextTwo)
            {
                Position = new Vector2(Singleton.SCREENWIDTH / 2, Singleton.SCREENHEIGHT)
            };



            //changeing to next bubble
            //should be in game state "Playing"
            /*
            _bubbleNextOne = _bubbleNextTwo;
            _bubbleNextTwo = new Bubble(_bubbleTexture);
            */

            Singleton.Instance.Score = 0;
        }    
    }
}