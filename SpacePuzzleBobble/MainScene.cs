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

        Vector2 _bubbleSize;

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
            _graphics.PreferredBackBufferWidth = Singleton.SCREENWIDTH;
            _graphics.PreferredBackBufferHeight = Singleton.SCREENHEIGHT;
            _graphics.ApplyChanges();

            //Starting pattern bubbles
            Singleton.Instance.GameBoard = new int[9,8]
            {
            /*        0    1   2  3   4   5   6   7  */
            /*0*/    {0   ,0 , 1 ,1  ,2  ,2  ,3  ,3  },
            /*1*/    {0   ,0 , 1 ,1  ,2  ,2  ,3  ,3  },
            /*2*/    {1   ,1 , 2 ,2  ,3  ,3  ,0  ,0  },
            /*3*/    {1   ,1 , 2 ,2  ,3  ,3  ,0  ,0  },
            /*4*/    {-1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 },
            /*5*/    {-1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 },
            /*6*/    {-1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 },
            /*7*/    {-1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 },
            /*8*/    {-1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 }
            };

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

            // Draw Game Table
            /*_spriteBatch.Draw(_rectTestTexture, new Vector2(Singleton.TILESIZE * 11, Singleton.TILESIZE), 
                            null, Color.Black, 0f, Vector2.Zero, 
                            new Vector2(Singleton.GAMEWIDTH * Singleton.TILESIZE, Singleton.GAMEHEIGHT * Singleton.TILESIZE), 
                            SpriteEffects.None, 0);
            */

            // Draw Bubble Crosshair
            _bubbleNextOne.Draw(_spriteBatch);
            _bubbleNextTwo.Draw(_spriteBatch);

            //Score
            _spriteBatch.DrawString(_font, "Score: " + Singleton.Instance.Score.ToString(), new Vector2(10, 10), Color.White);

            // Draw Crosshair
            _crosshair.Draw(_spriteBatch);

            //Draw starting pattern bubbles
            //PS : Maybe will move to Singleton.cs , when the game state is set-up  (Idle : Defualt state)
            for(int i = 0; i < Singleton.Instance.GameBoard.GetLength(0); i++)
            {
                for (int j = 0; j < Singleton.Instance.GameBoard.GetLength(1); j++)
                {
                    
                    //draw game board
                    /*_spriteBatch.Draw(_rectTestTexture, new Vector2(Singleton.TILESIZE * 11, Singleton.TILESIZE),
                            null, Color.Black, 0f, Vector2.Zero,
                            new Vector2(Singleton.GAMEWIDTH * Singleton.TILESIZE, Singleton.GAMEHEIGHT * Singleton.TILESIZE),
                            SpriteEffects.None, 0);
                    */

                    switch (Singleton.Instance.GameBoard[i, j])
                    {
                        case 0:
                            //Draw Red bubble
                            _spriteBatch.Draw(_bubbleTexture[0], new Vector2(Singleton.TILESIZE * j + 701, Singleton.TILESIZE * i + 70),
                                            null, Color.White, 0f, Vector2.Zero,
                                            _bubbleSize, SpriteEffects.None, 0);
                            break;

                        case 1:
                            //Draw Blue bubble
                            _spriteBatch.Draw(_bubbleTexture[1], new Vector2(Singleton.TILESIZE * j + 701, Singleton.TILESIZE * i + 70),
                                            null, Color.White, 0f, Vector2.Zero,
                                            _bubbleSize, SpriteEffects.None, 0);
                            break;

                        case 2:
                            //Draw Green bubble
                            _spriteBatch.Draw(_bubbleTexture[2], new Vector2(Singleton.TILESIZE * j + 701, Singleton.TILESIZE * i + 70),
                                            null, Color.White, 0f, Vector2.Zero,
                                            _bubbleSize, SpriteEffects.None, 0);
                            break;

                        case 3:
                            //Draw Yellow bubble
                            _spriteBatch.Draw(_bubbleTexture[3], new Vector2(Singleton.TILESIZE * j + 701, Singleton.TILESIZE * i + 70),
                                            null, Color.White, 0f, Vector2.Zero,
                                            _bubbleSize, SpriteEffects.None, 0);
                            break;

                        case 4:
                            //Draw Pink bubble
                            _spriteBatch.Draw(_bubbleTexture[4], new Vector2(Singleton.TILESIZE * j + 701, Singleton.TILESIZE * i + 70),
                                            null, Color.White, 0f, Vector2.Zero,
                                            _bubbleSize, SpriteEffects.None, 0);
                            break;
                    }
                }
            }

            _spriteBatch.End();
            _graphics.BeginDraw();

            base.Draw(gameTime);
        }

        protected void Reset()
        {
            _crosshair = new GameObject.Crosshair(_crosshairTexture)
            {
                Position = new Vector2(Singleton.SCREENWIDTH / 2, Singleton.SCREENHEIGHT)
            };

            _bubbleNextOne = new Bubble(_bubbleTexture)
            {
                Position = new Vector2((Singleton.SCREENWIDTH / 2) - Singleton.TILESIZE * 1.40f,
                            (Singleton.TILESIZE * 11) + Singleton.TILESIZE / 8f),
                Scale = new Vector2(1/1.45f, 1/1.45f)
            };

            _bubbleNextTwo= new Bubble(_bubbleTexture)
            {
                Position = new Vector2((Singleton.SCREENWIDTH / 2) - Singleton.TILESIZE * 3.6f, 
                            Singleton.TILESIZE * 12.3f),
                Scale = new Vector2(0.28f, 0.28f)
            };

            Singleton.Instance.Score = 0;
            _bubbleSize = new Vector2(0.26f, 0.26f);
        }
    }
}