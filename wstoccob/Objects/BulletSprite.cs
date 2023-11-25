﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using wstoccob.Objects.Base;

namespace wstoccob.Objects
{
    public class BulletSprite : BaseGameObject
    {
        private const float BULLET_SPEED = 10.0f;

        public BulletSprite(Texture2D texture)
        {
            _texture = texture;
        }

        public void MoveUp()
        {
            Position = new Vector2(Position.X, Position.Y - BULLET_SPEED);
        }
    }
}