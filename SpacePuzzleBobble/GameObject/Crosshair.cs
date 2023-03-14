using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpacePuzzleBobble.GameObject
{
    class Crosshair : GameObject
    {
        private Texture2D _bobbleTexture;
        
        private Bubble _bubbleNext, _bubbleNextTwo;

        private float _elapsedTime;

        private bool _spacebarPressed = false;
        private float _delayTime = 1f;

        public Crosshair(Texture2D texture, Bubble _bubbleNext, Bubble _bubbleNextTwo) : base(texture)
        {
            this._bubbleNext = _bubbleNext;
            _elapsedTime = 0f;
        }
        
        public override void Update(GameTime gameTime)
        {
            // Elapsed Time
            _elapsedTime += gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;

            // Crosshair Move
            if (_elapsedTime > 0.03f && Rotation > -0.45f && Rotation < 0.45f)
            {
                if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Left) || Singleton.Instance.CurrentKey.IsKeyDown(Keys.A))
                {
                    Rotation -= 0.06f;
                }
                if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Right) || Singleton.Instance.CurrentKey.IsKeyDown(Keys.D))
                {
                    Rotation += 0.06f;
                }
                _elapsedTime = 0f;
            } 
            if (Rotation >= 0.44f) Rotation = 0.44f;
            else if (Rotation <= -0.44f) Rotation = -0.44f;

            /*if (Singleton.Instance.isShooting)
                _bubbleNext.Update(gameTime);*/
            if (!_spacebarPressed && Singleton.Instance.CurrentKey.IsKeyDown(Keys.Space) && !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
            {
                //Singleton.Instance.isShooting = true;
                _spacebarPressed = true;

                _bubbleNext.Position = new Vector2(Singleton.SCREENWIDTH / 2 - Singleton.TILESIZE / 2, Singleton.SCREENHEIGHT);
                _bubbleNext.Scale = new Vector2(0.25f, 0.25f);

                Matrix m = new Matrix();
                m = Matrix.CreateRotationZ(Rotation);
                _bubbleNext._direction.X += m.M12 * 25f; //default = 20f
                _bubbleNext._direction.Y -= m.M11 * 25f;

                _bubbleNext.Update(gameTime);

                //_bubbleNext = _bubbleNextTwo;
            }

            if (_spacebarPressed)
            {
                _delayTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_delayTime <= 0f)
                {
                    _spacebarPressed = false;
                    _delayTime = 1f;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {   
            spriteBatch.Draw(_texture, Position, null, Color.White, Rotation, 
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

        public void setBubbleNextOne(Bubble _bubbleNext)
        {
            this._bubbleNext = _bubbleNext;
        }
    }
}
