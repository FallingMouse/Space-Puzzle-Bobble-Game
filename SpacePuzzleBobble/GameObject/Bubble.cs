using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpacePuzzleBobble.GameObject
{
    class Bubble : GameObject
    {
        Vector2 _tick;

        public bool IsHitTop; // Bobble Hit the Bobble or Ceiling
        public bool IsDead; // Bobble Hit Floor

        public enum BubbleType
        {
            Red,
            Blue,
            Green,
            Yellow,
            Pink,
            Size
        }
        public BubbleType CurrentBubbleType;

        // override
        public Bubble(Texture2D texture) : base(texture)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
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
    }
}
