using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpacePuzzleBobble.GameObject
{
    class Bubble : GameObject
    {
        public List<Bubble> neighbor, groupNeighbor, FallingBubble;
        public Vector2 _tick, _direction, _positionBox,_positionBubble;
        public int color, fy, fx;
        public static int index;

        //test
        public Vector2 tmpPosition = Vector2.Zero;
        public double tmpDistance = 0, tmpRadius = 0;
        public bool tmpB = false;
        public float dxTmp = 0, dyTmp = 0;

        public Texture2D[] _bubbleTexture;


        public bool IsHitTop; // Bubble Hit the Bubble or Ceiling
        public bool IsDead; // Bubble Hit Floor
        public bool CanDestroy = false;

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
            //CurrentBubbleType = (BubbleType)(Singleton.Instance.Random.Next((int)BubbleType.SIZE));

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

            // Bubble Hit Ceiling
            if (Position.Y <= Singleton.TILESIZE)
            {
                _direction = Vector2.Zero;

                DetectCollision();

                IsHitTop = true;

                /*if (CheckHit())
                {
                    IsHitTop = true;
                }*/
            }
            else // Bubble Hit Bubble
            //if (Position.Y > Singleton.TILESIZE) // Bubble Hit Bubble
            {
                for (int i = 0; i < Singleton.Instance.GameBoard.GetLength(0); i++)
                {
                    for (int j = 0; j < Singleton.Instance.GameBoard.GetLength(1); j++)
                    {
                        if (Singleton.Instance._bubbleTable[i, j] != this && Singleton.Instance.GameBoard[i, j] != -1)
                        {
                            // check Bubble collide
                            if (BubbleCollider(Singleton.Instance._bubbleTable[i, j]))
                            {
                                //_direction = Vector2.Zero;

                                DetectCollision();
                                PasteBubble(Singleton.Instance._bubbleTable[i, j]);

                                //Singleton.Instance._bubbleTable[i, j] = new Bubble(_bubbleTexture[5]);
                                
                                IsHitTop = true;
                            }
                        }
                    }
                }
            }

            base.Update(gameTime);
        }
        public void DetectCollision()
        {
            //refer to index of array
            fy = (int)(Position.Y - Singleton.TILESIZE + (Singleton.TILESIZE / 2)) / Singleton.TILESIZE;
            fx = (int)((Position.X - (Singleton.TILESIZE * 11) + (Singleton.TILESIZE / 2) - ((fy % 2) * (Singleton.TILESIZE / 2))) / Singleton.TILESIZE);

            _positionBox = new Vector2(fx, fy);

            //refer to position on game board
            Rectangle boundary = new Rectangle((fx * Singleton.TILESIZE) + (Singleton.TILESIZE * 11) + ((fy % 2) * (Singleton.TILESIZE / 2)),
                            (int)(fy * Singleton.TILESIZE) + Singleton.TILESIZE, Singleton.TILESIZE, Singleton.TILESIZE);
            _positionBubble = new Vector2(boundary.X, boundary.Y);

            Singleton.Instance.GameBoard[fy, fx] = color; // Not Equal -1 but it's not _bubbleTexture
            Singleton.Instance._bubbleTable[fy, fx] = this;
            Singleton.Instance._bubbleTable[fy, fx].Position = _positionBubble;
        }

        public bool BubbleCollider(Bubble _bubbleTable)
        {
            float dx = _bubbleTable.Position.X - Position.X;
            float dy = _bubbleTable.Position.Y - Position.Y;            

            int radiusOb1 = Singleton.TILESIZE / 2;
            int radiusOb2 = Singleton.TILESIZE / 2;
            double radiusTotal = radiusOb1 + radiusOb2;
            double radiusE = Math.Pow(radiusTotal, 2);

            double distance = Math.Pow(dx, 2) + Math.Pow(dy, 2);

            //test
            tmpPosition = _bubbleTable.Position;
            dxTmp = dx;
            dyTmp = dy;

            if (distance <= radiusE)
            {
                tmpB = true;
                return true;
            }
            tmpDistance = distance;
            tmpRadius = radiusE;
            return false;
        }

        public void FindNeighbor()
        {
            neighbor = new List<Bubble>();
            neighbor.Clear();

            for (int i = 0; i < Singleton.Instance.GameBoard.GetLength(0); i++)
            {
                for (int j = 0; j < Singleton.Instance.GameBoard.GetLength(1); j++)
                {
                    // top left
                    if (Singleton.Instance._bubbleTable[i, j]._positionBox
                        == new Vector2(_positionBox.X - ((_positionBox.Y + 1) % 2),
                        _positionBox.Y - 1))
                    {
                        neighbor.Add(Singleton.Instance._bubbleTable[i, j]);
                    }
                    // top right
                    else if (Singleton.Instance._bubbleTable[i, j]._positionBox
                        == new Vector2(_positionBox.X - ((_positionBox.Y + 1) % 2) + 1, 
                        _positionBox.Y - 1))
                    {
                        neighbor.Add(Singleton.Instance._bubbleTable[i, j]);
                    }
                    // left
                    else if (Singleton.Instance._bubbleTable[i, j]._positionBox
                        == new Vector2(_positionBox.X - 1, _positionBox.Y))
                    {
                        neighbor.Add(Singleton.Instance._bubbleTable[i, j]);
                    }
                    // right
                    else if (Singleton.Instance._bubbleTable[i, j]._positionBox 
                        == new Vector2(_positionBox.X + 1, _positionBox.Y))
                    {
                        neighbor.Add(Singleton.Instance._bubbleTable[i, j]);
                    }
                    // bottom left
                    else if (Singleton.Instance._bubbleTable[i, j]._positionBox 
                        == new Vector2(_positionBox.X -((_positionBox.Y + 1) % 2), _positionBox.Y + 1))
                    {
                        neighbor.Add(Singleton.Instance._bubbleTable[i, j]);
                    }
                    // bottom right
                    else if (Singleton.Instance._bubbleTable[i, j]._positionBox 
                        == new Vector2(_positionBox.X + 1 - ((_positionBox.Y + 1) % 2), _positionBox.Y + 1))
                    {
                        neighbor.Add(Singleton.Instance._bubbleTable[i, j]);
                    }
                }
            }
        }

        public List<Bubble> FindSameColor()
        {
            List<Bubble> SameColor = new List<Bubble>();
            SameColor.Add(this); // Add itself(_bubbleTable[itself, itself])
            this.CanDestroy = true;

            FindSameColor(SameColor, this);

            return SameColor;
        }
        public void FindSameColor(List<Bubble> SameColor, Bubble bubble)
        {
            bubble.FindNeighbor();
            foreach(Bubble _bubble in bubble.neighbor)
            {
                if(color == _bubble.color && !SameColor.Contains(_bubble))
                {
                    _bubble.CanDestroy = true;
                    SameColor.Add(_bubble);
                    FindSameColor(SameColor, _bubble);
                }
            }
        }

        public List<Bubble> FindConnectedBubble()
        {
            List<Bubble> ConnectedBubble = new List<Bubble>();
            ConnectedBubble.Add(this);
            FindConnectedBubble(ConnectedBubble, this);

            return ConnectedBubble;
        }
        public void FindConnectedBubble(List<Bubble> ConnectedBubble, Bubble bubble)
        {
            bubble.FindNeighbor();
            foreach(Bubble _bubble in bubble.neighbor)
            {
                if(!_bubble.CanDestroy && !ConnectedBubble.Contains(_bubble))
                {
                    ConnectedBubble.Add(_bubble);
                    if (_bubble._positionBox.Y == 0) break;
                    FindConnectedBubble(ConnectedBubble, _bubble);
                }
            }
        }

        public void PasteBubble(Bubble bubble)
        {
            List<Bubble> groupSameBubble = bubble.FindSameColor();
            List<Bubble> floatingBubble = new List<Bubble>();
            List<Bubble> connectedBubble = new List<Bubble>();

            if (groupSameBubble.Count >= 3)
            {
                //do else 
                FallingBubble = groupSameBubble;

                for (int i = 0; i < Singleton.Instance.GameBoard.GetLength(0); i++)
                {
                    for (int j = 0; j < Singleton.Instance.GameBoard.GetLength(1); j++)
                    {
                        if (groupSameBubble.Contains(Singleton.Instance._bubbleTable[i, j]))
                        {
                            Singleton.Instance._bubbleTable[i, j] = new Bubble(_bubbleTexture[5]);
                        }
                    }
                }
                
                foreach (Bubble _bubble in Singleton.Instance._bubbleTable)
                {
                    _bubble.CanDestroy = false;
                }

                bool isHitCeiling = false;
                foreach (Bubble _bubble in Singleton.Instance._bubbleTable)
                {
                    isHitCeiling = false;
                    if (_bubble._positionBox.Y > 0)
                    {
                        connectedBubble = _bubble.FindConnectedBubble();
                        foreach (Bubble burb in connectedBubble)
                        {
                            if(burb._positionBox.Y == 0)
                            {
                                isHitCeiling = true;
                                break;
                            }
                        }

                        if (!isHitCeiling)
                        {
                            _bubble.CanDestroy = true;
                            floatingBubble.Add(_bubble);
                        }
                    }
                }

                for (int i = 0; i < Singleton.Instance.GameBoard.GetLength(0); i++)
                {
                    for (int j = 0; j < Singleton.Instance.GameBoard.GetLength(1); j++)
                    {
                        if (floatingBubble.Contains(Singleton.Instance._bubbleTable[i, j]))
                        {
                            Singleton.Instance._bubbleTable[i, j] = new Bubble(_bubbleTexture[5]);
                        }
                    }
                }
                foreach (Bubble _bubble in floatingBubble)
                {
                    FallingBubble.Add(_bubble);
                }

            }
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
    } //class 
}
