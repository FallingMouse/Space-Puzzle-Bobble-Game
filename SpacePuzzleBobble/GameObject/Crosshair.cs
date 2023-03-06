using System;
using System.Net.NetworkInformation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static SpacePuzzleBobble.GameObject.Bubble;

namespace SpacePuzzleBobble.GameObject
{
    class Crosshair : GameObject
    {
        private Texture2D _bobbleTexture;
        
        private Color _color;
        private Color[] _colorMonitor;
        
        private Bubble _bubbleNextOne, _bubbleNextTwo;

        private float rotation;
        private float _elapsedTime;

        public Crosshair(Texture2D texture, Bubble _bubbleNextOne, Bubble _bubbleNextTwo) : base(texture)
        {
            this._bubbleNextOne = _bubbleNextOne;
            this._bubbleNextTwo = _bubbleNextTwo;
            rotation = 0f;
            _elapsedTime = 0f;
        }

        public override void Update(GameTime gameTime)
        {
            // Elapsed Time
            _elapsedTime += gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;

            // Bubble Move
            _bubbleNextOne.Position += _bubbleNextOne._direction;

            // Crosshair Move
            if (_elapsedTime > 0.03f && rotation > -0.45f && rotation < 0.45f)
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
            if (rotation >= 0.44f) rotation = 0.44f;
            else if (rotation <= -0.44f) rotation = -0.44f;

            // Bubble Hit Table
            if(_bubbleNextOne.Position.X <= Singleton.TILESIZE * 11 || 
                _bubbleNextOne.Position.X >= Singleton.TILESIZE * 18)
            {
                _bubbleNextOne._direction.X *= -1;
            }
            if (_bubbleNextOne.Position.Y <= Singleton.TILESIZE)
            {
                _bubbleNextOne._direction = Vector2.Zero;
            }

            /*if (Singleton.Instance.isShooting)
                _bubbleNext.Update(gameTime);*/
            if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Space) && !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
            {
                Singleton.Instance.isShooting = true;

                _bubbleNextOne.Position = new Vector2(Singleton.SCREENWIDTH / 2 - Singleton.TILESIZE / 2, Singleton.SCREENHEIGHT);
                _bubbleNextOne.Scale = new Vector2(0.25f, 0.25f);

                Matrix m = new Matrix();
                m = Matrix.CreateRotationZ(rotation);
                _bubbleNextOne._direction.X += m.M12 * 20f; //20f
                _bubbleNextOne._direction.Y -= m.M11 * 20f;
            }


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
