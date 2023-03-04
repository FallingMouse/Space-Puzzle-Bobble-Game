using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpacePuzzleBobble.GameObject
{
    class Bubble : GameObject
    {
        Vector2 _tick, _pose, _direction;
        public Texture2D _bubbleTexture;

        public bool IsHitTop; // Bobble Hit the Bobble or Ceiling
        public bool IsDead; // Bobble Hit Floor

        public enum BubbleType
        {
            Red,
            Blue,
            Green,
            Yellow,
            Pink
        }
        public BubbleType CurrentBubbleType;

        // override
        public Bubble(Texture2D texture) : base(texture)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color.White, 0f, Vector2.Zero, 1/1.45f, SpriteEffects.None, 0);
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




























        /*public String GetRandomColor()
        {
            _bubbleTexture = "";

            CurrentBubbleType = (BubbleType)(Singleton.Instance.Random.Next((int)BubbleType.Size));
            switch (CurrentBubbleType)
            {
                case BubbleType.Red:
                    _bubbleTexture = "_bubbleTextureRed";
                    break;
                case BubbleType.Blue:
                    _bubbleTexture = "_bubbleTextureBlue";
                    break;
                case BubbleType.Green:
                    _bubbleTexture = "_bubbleTextureGreen";
                    break;
                case BubbleType.Yellow:
                    _bubbleTexture = "_bubbleTextureYellow";
                    break;
                case BubbleType.Pink:
                    _bubbleTexture = "_bubbleTexturePink";
                    break;
            }
            return _bubbleTexture;
        }*/
    }
}
