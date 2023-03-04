using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpacePuzzleBobble.GameObject;
using static SpacePuzzleBobble.GameObject.Bubble;
using System;

namespace SpacePuzzleBobble
{
    public class MainScene : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        GameObject.Joystick _joystick;
        GameObject.Bubble _bubbleJoyStick;

        Texture2D _backgroundTexture, _rectTestTexture, _joystickTexture;
                //_bubbleTextureRed, _bubbleTextureBlue, _bubbleTextureGreen, _bubbleTextureYellow, _bubbleTexturePink;
        Texture2D[] _bubbleTexture;

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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load Texture
            _backgroundTexture = this.Content.Load<Texture2D>("Background/background");
            _joystickTexture = this.Content.Load<Texture2D>("Joystick/joystick");

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

            Reset();
        }

        protected override void Update(GameTime gameTime)
        {
            Singleton.Instance.CurrentKey = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            _joystick.Update(gameTime);
            
            //Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Draw Background
            _spriteBatch.Draw(_backgroundTexture, new Vector2(0,0), Color.White);

            // Test Game Table
            _spriteBatch.Draw(_rectTestTexture, new Vector2(705, 65), null, Color.Black, 0f, Vector2.Zero, new Vector2(Singleton.GAMEWIDTH * Singleton.TILESIZE, Singleton.GAMEHEIGHT * Singleton.TILESIZE), SpriteEffects.None, 0);

            // Draw Bubble
            _bubbleJoyStick.Draw(_spriteBatch);

            // Draw Joystick
            _joystick.Draw(_spriteBatch);

            _spriteBatch.End();
            _graphics.BeginDraw();

            base.Draw(gameTime);
        }

        protected void Reset()
        {
            _joystick = new GameObject.Joystick(_joystickTexture, _bubbleJoyStick)
            {
                Position = new Vector2(Singleton.SCREENWIDTH / 2, Singleton.SCREENHEIGHT)
            };

            _bubbleJoyStick = new Bubble(_bubbleTexture[Singleton.Instance.Random.Next(_bubbleTexture.Length)])
            {
                Position = new Vector2((Singleton.SCREENWIDTH / 2) - Singleton.TILESIZE * 1.40f,
                            (Singleton.TILESIZE * 11) + Singleton.TILESIZE / 8f)
            };

        }
    }
}