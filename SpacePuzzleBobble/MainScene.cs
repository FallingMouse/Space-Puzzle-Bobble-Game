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
        private Controller _controller;

        Texture2D _backgroundTexture, _rectTestTexture,
                    _controllerTexture, _bubbleTexture;

        public MainScene()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = Singleton.SCREENWIDTH;
            _graphics.PreferredBackBufferHeight = Singleton.SCREENHEIGHT;
            _graphics.ApplyChanges();

            _controller = new Controller(_controllerTexture)
            {
                //Position = new Vector2(897, 769)
            };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _backgroundTexture = this.Content.Load<Texture2D>("normalBoard");
            _controllerTexture = this.Content.Load<Texture2D>("controller");


            _rectTestTexture = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            Color[] data = new Color[1 * 1];
            for(int i = 0; i< data.Length; i++)
            {
                data[i] = Color.White;
            }
            _rectTestTexture.SetData(data);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            _controller.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Draw Background
            _spriteBatch.Draw(_backgroundTexture, new Vector2(0,0), Color.White);

            // Test Game Table
            _spriteBatch.Draw(_rectTestTexture, new Vector2(705, 65), null, Color.White, 0f, Vector2.Zero, new Vector2(Singleton.GAMEWIDTH * Singleton.TILESIZE, Singleton.GAMEHEIGHT * Singleton.TILESIZE), SpriteEffects.None, 0);

            // Test Draw Bubble
            _spriteBatch.Draw(_rectTestTexture, new Vector2(897, 743), null, Color.Red,
                            0f, Vector2.Zero, new Vector2(2 * Singleton.TILESIZE, 2 * Singleton.TILESIZE), SpriteEffects.None, 0);

            // Test Draw Controller
            //_spriteBatch.Draw(_controllerTexture, new Vector2(897, 769), null, Color.White,
            //                0f, Vector2.Zero, new Vector2(2 * Singleton.TILESIZE, 4 * Singleton.TILESIZE), SpriteEffects.None, 0);

            //_spriteBatch.Draw(_controllerTexture, new Vector2(897, 769), null, Color.White,
            //                0f, Vector2.Zero, 0.25f, SpriteEffects.None, 0);

            _controller.Draw(_spriteBatch);
            _spriteBatch.Draw(_controllerTexture, new Vector2(897, 769), Color.White);

            _spriteBatch.End();
            _graphics.BeginDraw();

            base.Draw(gameTime);
        }
    }
}