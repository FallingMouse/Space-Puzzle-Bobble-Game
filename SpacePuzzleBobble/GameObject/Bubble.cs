using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpacePuzzleBobble.GameObject
{
    class Bubble : GameObject
    {
        public Vector2 _tick, _direction, _positionBubble;
        public int color, fy, fx;

        public Texture2D[] _bubbleTexture;

        //public static int indexOne, indexTwo, priority = 1;

        public bool IsHitTop; // Bubble Hit the Bubble or Ceiling
        public bool IsDead; // Bubble Hit Floor

        public enum BubbleType
        {
            Red,
            Blue,
            Green,
            Yellow,
            Pink,
            SIZE
        }
        public static BubbleType CurrentBubbleType;

        // override
        public Bubble(Texture2D texture) : base(texture)
        {
            IsHitTop = false;
            IsDead = false;
            _direction = Vector2.Zero;
        }

        public Bubble(Texture2D[] texture) : base(texture[GetRandomColor()])
        {
            IsHitTop = false;
            IsDead = false;
            _direction = Vector2.Zero;
            _bubbleTexture = texture;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            // Maybe will use this to link between _bubbleNextOne and _bubbleNextTwo
            // Then we can use this in MainScene.cs like Tetris Lab

            //Random the colour of bubble -> called by MainScene.Update()
            color = 0;
            CurrentBubbleType = (BubbleType)(Singleton.Instance.Random.Next((int)BubbleType.SIZE));

            switch (CurrentBubbleType)
            {
                case BubbleType.Red:
                    color = 0;
                    break;
                case BubbleType.Blue:
                    color = 1;
                    break;
                case BubbleType.Green:
                    color = 2;
                    break;
                case BubbleType.Yellow:
                    color = 3;
                    break;
                case BubbleType.Pink:
                    color = 4;
                    break;
            }
            //index = indexOne;
            //return color;

            base.Reset();
        }

        public override void Update(GameTime gameTime)
        {
            // Bubble Move
            Position += _direction;

            // Bubble Hit Table
            if (Position.X <= Singleton.TILESIZE * 11 ||
                Position.X >= Singleton.TILESIZE * 18)
            {
                _direction.X *= -1;
            }
            if (Position.Y <= Singleton.TILESIZE)
            {
                _direction = Vector2.Zero;

                DetectCollision();
                Position = _positionBubble;

                //IsHitTop = true;

                /*if (CheckHit())
                {
                    IsHitTop = true;
                }*/
            }

            base.Update(gameTime);
        }
        public void DetectCollision()
        {
            fy = (int)(Position.Y - Singleton.TILESIZE + (Singleton.TILESIZE / 2)) / Singleton.TILESIZE;
            fx = (int)((Position.X - (Singleton.TILESIZE * 11) + (Singleton.TILESIZE / 2) - ((fy % 2) * (Singleton.TILESIZE / 2))) / Singleton.TILESIZE);
            //_positionBubble = new Vector2(fx, fy);
            
            Rectangle boundary = new Rectangle((fx * Singleton.TILESIZE) + (Singleton.TILESIZE * 11) + ((fy % 2) * (Singleton.TILESIZE / 2)),
                            (int)(fy * Singleton.TILESIZE) + Singleton.TILESIZE, Singleton.TILESIZE, Singleton.TILESIZE);
            _positionBubble = new Vector2(boundary.X, boundary.Y);
        }

        //test if it can use only 1 switch-case
        public static int GetRandomColor()
        {
            int index = 0;
            CurrentBubbleType = (BubbleType)(Singleton.Instance.Random.Next((int)BubbleType.SIZE));

                switch (CurrentBubbleType)
                {
                    case BubbleType.Red:
                        index = 0;
                        break;
                    case BubbleType.Blue:
                        index = 1;
                        break;
                    case BubbleType.Green:
                        index = 2;
                        break;
                    case BubbleType.Yellow:
                        index = 3;
                        break;
                    case BubbleType.Pink:
                        index = 4;
                        break;
                }
                //index = indexOne;
            return index;
        }

        private bool CheckHit()
        {
            bool isHisTop = false;

            if(Position.Y / Singleton.TILESIZE < Singleton.GAMEHEIGHT && 
               Position.Y / Singleton.TILESIZE >= 0)
            {
                //collision occur -> hit the  top
                isHisTop = true;
            }
            else if(Position.Y / Singleton.TILESIZE > Singleton.GAMEHEIGHT)
            {
                //game over
            }

            //still have an exception
            /*
            else if (Singleton.Instance.GameBoard[ (int)Position.Y / Singleton.TILESIZE, 
                        (int)Position.X / Singleton.TILESIZE] != -1)
            {
                //collision occur -> hit other bubble
                isHisTop = true;
            }*/

            return isHisTop;
        }

    private bool CheckDead()
        {
            bool isDead = false;

            return isDead;
        }
    } //class
}
