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
        public int color, fy, fx, spaceCount, bubbleDropTime = 0;
        public float elapsedTime = 0;
        public static int index;

        //test
        public Vector2 tmpPosition = Vector2.Zero;
        public double tmpDistance = 0, tmpRadius = 0;
        public bool tmpB = false;
        public float dxTmp = 0, dyTmp = 0;

        public Texture2D[] _bubbleTexture;


        public bool IsHitTop; // Bubble Hit the Bubble or Ceiling
        public bool IsDead; // Bubble Hit Floor
        public bool IsWon; // Clear all bubble
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
            //Random the colour of bubble -> called by MainScene.Update()
            color = 0;

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

            if (Singleton.Instance.countdown == 0)
            {
                Singleton.Instance.posCeiling++;
                Singleton.Instance.countdown = 15;
                foreach (Bubble bubbleTable in Singleton.Instance._bubbleTable)
                {
                    bubbleTable._positionBox = new Vector2(bubbleTable._positionBox.X, bubbleTable._positionBox.Y + 1);

                    Rectangle boundary = new Rectangle(((int)(bubbleTable._positionBox.X) * Singleton.TILESIZE) + (Singleton.TILESIZE * 11) + (((int)(bubbleTable._positionBox.Y + Singleton.Instance.posCeiling) % 2) * (Singleton.TILESIZE / 2)),
                                    (int)((int)(bubbleTable._positionBox.Y) * Singleton.TILESIZE) + Singleton.TILESIZE, Singleton.TILESIZE, Singleton.TILESIZE);

                    bubbleTable.Position = new Vector2(boundary.X, boundary.Y);
                    
                }

                //Check if ceilling down (Y+1), is the bubble in the last line hit the ground (Y > GameBoard.GetLength(0))
                for (int i = 0; i < Singleton.Instance.GameBoard.GetLength(0); i++)
                {
                    for (int j = 0; j < Singleton.Instance.GameBoard.GetLength(1); j++)
                    {
                        if (Singleton.Instance._bubbleTable[i, j].color != 5)
                        {
                            if (Singleton.Instance._bubbleTable[i, j]._positionBox.Y + 1 > Singleton.Instance.GameBoard.GetLength(0))
                            {
                                IsDead = true;
                                break;
                            }
                        }    
                    }
                }
            }

            // Bubble Hit Ceiling
            if (Position.Y <= Singleton.TILESIZE + (Singleton.Instance.posCeiling * Singleton.TILESIZE))
            {
                _direction = Vector2.Zero;

                DetectCollision();

                IsHitTop = true;
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
                                //if collision causes dying, stop this loop
                                if (i+1 >= Singleton.Instance.GameBoard.GetLength(0))
                                {
                                    IsDead = true;
                                    break;
                                }
                                else
                                {
                                    DetectCollision();
                                    PasteBubble(Singleton.Instance._bubbleTable[fy, fx]);
                                    IsHitTop = true;

                                    //have to check all value contain in Game board is it equal -1 (space area), if so the player had clear all bubbles in game and WON
                                    //create condition here
                                    spaceCount = 0;
                                    for (int k = 0; k < Singleton.Instance.GameBoard.GetLength(0); k++)
                                    {
                                        for (int l = 0; l < Singleton.Instance.GameBoard.GetLength(1); l++)
                                        {
                                            if (Singleton.Instance.GameBoard[k, l] == -1) spaceCount++;

                                            //every values contained in Singleton.Instance.GameBoard = -1 (all bubble clear)
                                            if (spaceCount == (Singleton.Instance.GameBoard.GetLength(0) * Singleton.Instance.GameBoard.GetLength(1)))
                                                IsWon = true;
                                        }
                                    }
                                }
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
            fx = (int)((Position.X - (Singleton.TILESIZE * 11) + (Singleton.TILESIZE / 2) - (((fy + Singleton.Instance.posCeiling) % 2) * (Singleton.TILESIZE / 2))) / Singleton.TILESIZE);

            _positionBox = new Vector2(fx, fy);

            //refer to position on game board
            Rectangle boundary = new Rectangle((fx * Singleton.TILESIZE) + (Singleton.TILESIZE * 11) + (((fy + Singleton.Instance.posCeiling) % 2) * (Singleton.TILESIZE / 2)),
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
                        == new Vector2(_positionBox.X - ((_positionBox.Y + 1 + Singleton.Instance.posCeiling) % 2),
                        _positionBox.Y - 1))
                    {
                        neighbor.Add(Singleton.Instance._bubbleTable[i, j]);
                    }
                    // top right
                    else if (Singleton.Instance._bubbleTable[i, j]._positionBox
                        == new Vector2(_positionBox.X - ((_positionBox.Y + 1 + Singleton.Instance.posCeiling) % 2) + 1, 
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
                        == new Vector2(_positionBox.X -((_positionBox.Y + 1 + Singleton.Instance.posCeiling) % 2), _positionBox.Y + 1))
                    {
                        neighbor.Add(Singleton.Instance._bubbleTable[i, j]);
                    }
                    // bottom right
                    else if (Singleton.Instance._bubbleTable[i, j]._positionBox 
                        == new Vector2(_positionBox.X + 1 - ((_positionBox.Y + 1 + Singleton.Instance.posCeiling) % 2), _positionBox.Y + 1))
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
                    if (_bubble._positionBox.Y == Singleton.Instance.posCeiling) break;
                    FindConnectedBubble(ConnectedBubble, _bubble);
                }
            }
        }

        public void PasteBubble(Bubble bubble)
        {
            List<Bubble> groupSameBubble = bubble.FindSameColor();
            List<Bubble> floatingBubble = new List<Bubble>();
            List<Bubble> connectedBubble = new List<Bubble>();

            bubbleDropTime++; //

            if (groupSameBubble.Count >= 3)
            {
                Singleton.Instance.Score += groupSameBubble.Count * 50;
                //do else 
                FallingBubble = groupSameBubble;

                for (int i = 0; i < Singleton.Instance.GameBoard.GetLength(0); i++)
                {
                    for (int j = 0; j < Singleton.Instance.GameBoard.GetLength(1); j++)
                    {
                        if (groupSameBubble.Contains(Singleton.Instance._bubbleTable[i, j]))
                        {
                            Singleton.Instance._bubbleTable[i, j] = new Bubble(_bubbleTexture[5]);
                            Singleton.Instance._bubbleTable[i, j].color = 5;
                            Singleton.Instance._bubbleTable[fy, fx].color = 5;
                            Singleton.Instance.GameBoard[i, j] = -1;
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
                    if (_bubble._positionBox.Y > Singleton.Instance.posCeiling) // default == 0
                    {
                        connectedBubble = _bubble.FindConnectedBubble();
                        foreach (Bubble burb in connectedBubble)
                        {
                            if(burb._positionBox.Y == Singleton.Instance.posCeiling) // default == 0
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
                            Singleton.Instance._bubbleTable[i, j].color = 5;
                            Singleton.Instance.GameBoard[i, j] = -1;
                        }
                    }
                }
                foreach (Bubble _bubble in floatingBubble)
                {
                    FallingBubble.Add(_bubble);
                }     
            }
            // Bubble Drop Time
            //if(bubbleDropTime == 5)
            //{
            //    Singleton.Instance.posCeiling++;
            //    bubbleDropTime = 0;
            //    foreach(Bubble bubbleTable in Singleton.Instance._bubbleTable)
            //    {
            //        bubbleTable._positionBox = new Vector2(bubbleTable._positionBox.X, bubbleTable._positionBox.Y + 1);

            //        Rectangle boundary = new Rectangle(((int)(bubbleTable._positionBox.X) * Singleton.TILESIZE) + (Singleton.TILESIZE * 11) + (((int)(bubbleTable._positionBox.Y + Singleton.Instance.posCeiling) % 2) * (Singleton.TILESIZE / 2)),
            //            (int)((int)(bubbleTable._positionBox.Y) * Singleton.TILESIZE) + Singleton.TILESIZE, Singleton.TILESIZE, Singleton.TILESIZE);

            //        bubbleTable.Position = new Vector2(boundary.X, boundary.Y);
            //    }
            //}
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

