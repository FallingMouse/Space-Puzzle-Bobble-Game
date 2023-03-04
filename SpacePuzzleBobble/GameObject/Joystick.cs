using System;
using System.Net.NetworkInformation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static SpacePuzzleBobble.GameObject.Bubble;

namespace SpacePuzzleBobble.GameObject
{
    class Joystick : GameObject
    {
        private Texture2D _bobbleTexture;
        
        private Color _color;
        private Color[] _colorMonitor;
        
        private Bubble _bubbleNext;

        private float rotation;
        private float _elapsedTime;

        public Joystick(Texture2D texture, Bubble _bubbleNext) : base(texture)
        {
            this._bubbleNext = _bubbleNext;
            rotation = 0f;
            _elapsedTime = 0f;
        }

        public override void Update(GameTime gameTime)
        {
            // Elapsed Time
            _elapsedTime += gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;

            // Joystick Move
            if (_elapsedTime > 0.03f)
            {
                if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Left) || Singleton.Instance.CurrentKey.IsKeyDown(Keys.A))
                {
                    rotation -= 0.06f;
                }
                if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Right) || Singleton.Instance.CurrentKey.IsKeyDown(Keys.D))
                    {
                    rotation += 0.06f;
                }
                _elapsedTime = 0f;
            }
            /*if (Singleton.Instance.isShooting)
                _bubbleNext.Update(gameTime);*/

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {   
            // Next Bubble 1 Position
            //spriteBatch.Draw(_texture, new Vector2(897, 743), null, Color.White, rotation,)

            spriteBatch.Draw(_texture, Position, null, Color.White, rotation, 
                            new Vector2(_texture.Width/2, _texture.Height), 1f, SpriteEffects.None, 0);

            // Old Code don't delete
            //if (!Singleton.Instance.isShooting)
            //    spriteBatch.Draw(_texture, new Vector2(897, 743), _color);
            //else
            //    _bubbleNext.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        public override void Reset()
        {

            base.Reset();
        }
    }
}
