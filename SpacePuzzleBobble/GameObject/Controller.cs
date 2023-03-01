using System;
using System.Net.NetworkInformation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpacePuzzleBobble.GameObject
{
    class Controller : GameObject
    {
        private Texture2D _bobbleTexture;
        private Color _color;
        private Color[] _colorMonitor;
        private Bubble _bubbleNext;
        private float rotation;
        private float _elapsedTime = 0f;

        private Random _rand = new Random();

        public Controller(Texture2D texture) : base(texture)
        {

        }

        //Test 2 Parameter
        /*public Controller(Texture2D texture, Texture2D bobble) : base(texture)
        {
            _bobbleTexture = bobble;
            _color = GetRandomColor();
        }*/

        public override void Update(GameTime gameTime)
        {
            // Elapsed Time
            //_elapsedTime += gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;

            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Controller Move
            if (_elapsedTime > 0.05f)
            {
                if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Left) || Singleton.Instance.CurrentKey.IsKeyDown(Keys.D)
                    && !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
                {
                    rotation -= 0.1f;
                }
                if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Right) || Singleton.Instance.CurrentKey.IsKeyDown(Keys.A)
                    && !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
                {
                    rotation += 0.1f;
                }
                _elapsedTime = 0f;
            }
            /*if (Singleton.Instance.isShooting)
                _bubbleNext.Update(gameTime);*/

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {   // Position = 897, 769
            if (_texture != null)
            {
                //spriteBatch.Draw(_texture, Position + new Vector2(50, 50), null, Color.White,
                //                        rotation, new Vector2(_texture.Width / 2, _texture.Height / 2),
                //                        0.25f, SpriteEffects.None, 2);
                spriteBatch.Draw(_texture, new Vector2(897, 743), _color);
            }
            else
            {
                Console.WriteLine("No Texture");
            }

            //test
            //spriteBatch.Draw(_texture, new Vector2(897, 769), Color.White);

            // Old Code don't delete
            //if (!Singleton.Instance.isShooting)
            //    spriteBatch.Draw(_bobbleTexture, new Vector2(897, 743), _color);
            //else 
            //    _bubbleNext.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            base.Reset();
        }

        public Color GetRandomColor()
        {
            _colorMonitor = new Color[] 
                            { Color.Red, Color.Blue, Color.Green, 
                                Color.Yellow, Color.Pink };
            _color = _colorMonitor[_rand.Next(0, _colorMonitor.Length)];
            return _color;
        }
    }
}
