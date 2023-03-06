using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpacePuzzleBobble.GameObject
{
    class Bubble : GameObject
    {
        Vector2 _tick, _pose, _direction;

        public Texture2D[] _bubbleTexture;

        public static int indexOne, indexTwo, priority = 1;

        public bool IsBubbleNext;
        public bool IsHitTop; // Bobble Hit the Bobble or Ceiling
        public bool IsDead; // Bobble Hit Floor

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

        }

        public Bubble(Texture2D[] texture) : base(texture[GetRandomColor()])
        {
            _bubbleTexture = texture;
            this.IsBubbleNext = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(IsBubbleNext)
                spriteBatch.Draw(_texture, Position, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
            else 
                spriteBatch.Draw(_texture, Position, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
            base.Draw(spriteBatch);
        }

        public override void Reset()
        {

            base.Reset();
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public static int GetRandomColor()
        {
            int index = 0;
            if (priority == 1) { 
            
                CurrentBubbleType = (BubbleType)(Singleton.Instance.Random.Next((int)BubbleType.SIZE));
            
                switch (CurrentBubbleType)
                {
                    case BubbleType.Red:
                        indexOne = 0;
                        break;
                    case BubbleType.Blue:
                        indexOne = 1;
                        break;
                    case BubbleType.Green:
                        indexOne = 2;
                        break;
                    case BubbleType.Yellow:
                        indexOne = 3;
                        break;
                    case BubbleType.Pink:
                        indexOne = 4;
                        break;
                }
                index = indexOne;
            }

            else if (priority == 2)
            {

                CurrentBubbleType = (BubbleType)(Singleton.Instance.Random.Next((int)BubbleType.SIZE));

                switch (CurrentBubbleType)
                {
                    case BubbleType.Red:
                        indexTwo = 0;
                        break;
                    case BubbleType.Blue:
                        indexTwo = 1;
                        break;
                    case BubbleType.Green:
                        indexTwo = 2;
                        break;
                    case BubbleType.Yellow:
                        indexTwo = 3;
                        break;
                    case BubbleType.Pink:
                        indexTwo = 4;
                        break;
                }
                index = indexTwo;
                priority = 0;
            } 
            priority++;
            
            return index;
        }

    }
}
