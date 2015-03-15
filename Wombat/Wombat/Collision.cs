using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace Wombat
{
    class Collision
    {
        
        public bool IsAboveAC(Rectangle collsionHitBox, Vector2 playervector)
        {
            return IsOnUpperSideOfLine(GetBottomRightCorner(collsionHitBox), GetTopLeftCorner(collsionHitBox), playervector);
        }
        public bool IsAboveDB(Rectangle collsionHitBox, Vector2 playervector)
        {
            return IsOnUpperSideOfLine(GetTopRightCorner(collsionHitBox), GetBottomLeftCorner(collsionHitBox), playervector);
        }

        public bool RectangleCollisionTop(Rectangle playerHitBox, Rectangle collsionHitBox, Vector2 velocity)
        {


            if (playerHitBox.Left < collsionHitBox.Right && playerHitBox.Right > collsionHitBox.Left &&
                playerHitBox.Bottom + velocity.Y > collsionHitBox.Top && playerHitBox.Top < collsionHitBox.Top &&
                        IsAboveAC(collsionHitBox, GetBottomRightCorner(playerHitBox)) &&
                        IsAboveDB(collsionHitBox, GetBottomLeftCorner(playerHitBox)))
            {
                return true;
            }
            return false;
        }

        public bool RectangleCollisionBottom(Rectangle playerHitBox, Rectangle collsionHitBox, Vector2 velocity)
        {

            if (playerHitBox.Left < collsionHitBox.Right && playerHitBox.Right > collsionHitBox.Left &&
                playerHitBox.Top + velocity.Y < collsionHitBox.Bottom && playerHitBox.Bottom > collsionHitBox.Bottom &&
             !IsAboveAC(collsionHitBox, GetTopLeftCorner(playerHitBox)) &&
                        !IsAboveDB(collsionHitBox, GetTopRightCorner(playerHitBox)))
            {
                return true;
            }
            return false;
        }
        public bool RectangleCollisionLeft(Rectangle playerHitBox, Rectangle collsionHitBox, Vector2 velocity)
        {


            if (playerHitBox.Right + velocity.X > collsionHitBox.Left && playerHitBox.Left + velocity.X < collsionHitBox.Left &&
                   IsAboveDB(collsionHitBox, GetTopRightCorner(playerHitBox)) &&
                        !IsAboveAC(collsionHitBox, GetBottomRightCorner(playerHitBox)))
            {
                return true;
            }
            return false;
        }
        public bool RectangleCollisionRight(Rectangle playerHitBox, Rectangle collsionHitBox, Vector2 velocity)
        {
            if (playerHitBox.Left + velocity.X < collsionHitBox.Right && playerHitBox.Right+ velocity.X  > collsionHitBox.Right &&
                IsAboveAC(collsionHitBox, GetTopLeftCorner(playerHitBox)) &&
                        !IsAboveDB(collsionHitBox, GetBottomLeftCorner(playerHitBox)))
            {
                return true;
            }
            return false;
        }
        public Vector2 GetCenter(Rectangle rect)
        {
            return new Vector2(rect.Center.X, rect.Center.Y);
        }


        public Vector2 GetTopLeftCorner(Rectangle rect)
        {
            return new Vector2(rect.X, rect.Y);
        }
        public Vector2 GetTopRightCorner(Rectangle rect)
        {
            return new Vector2(rect.X + rect.Width, rect.Y);
        }
        public Vector2 GetBottomRightCorner(Rectangle rect)
        {
            return new Vector2(rect.X + rect.Width, rect.Y + rect.Height);
        }
        public Vector2 GetBottomLeftCorner(Rectangle rect)
        {
            return new Vector2(rect.X, rect.Y + rect.Height);
        }
        public bool IsOnUpperSideOfLine(Vector2 corner1, Vector2 oppositeCorner, Vector2 playerHitBoxCenter)
        {
            return ((oppositeCorner.X - corner1.X) * (playerHitBoxCenter.Y - corner1.Y) - (oppositeCorner.Y - corner1.Y) * (playerHitBoxCenter.X - corner1.X)) > 0;
        }
    }
}
