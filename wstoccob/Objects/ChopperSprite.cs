﻿
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using wstoccob.Engine.Objects;
using wstoccob.Engine.States;
using wstoccob.States.Gameplay;

namespace wstoccob.Objects
{
    public class ChopperSprite : BaseGameObject
    {
        private const float Speed = 4.0f;
        private const float BladeSpeed = 0.2f;
        // The chopper that we want from the texture
        private const int ChopperStartX = 0;
        private const int ChopperStartY = 0;
        private const int ChopperWidth = 44;
        private const int ChopperHeight = 98;
        // The blades on the texture
        private const int BladesStartX = 133;
        private const int BladesStartY = 98;
        private const int BladesWidth = 94;
        private const int BladesHeight = 94;
        // Rotation center of the blades
        private const float BladesCenterX = 47.5f;
        private const float BladesCenterY = 47.5f;
        // Position of the blades on the chopper
        private const int ChopperBladePosX = ChopperWidth / 2;
        private const int ChopperBladePosY = 34;
        
        private float _angle = 0.0f;
        private Vector2 _direction = new Vector2(0, 0);
        
        private int _age = 0;
        private int _life = 40;

        private int _hitAt = 0;
        
        private List<(int, Vector2)> _path;
        

        public ChopperSprite(Texture2D texture, List<(int, Vector2)> path)
        {
            _texture = texture;
            _path = path;
        }

        public void Update()
        {
            foreach (var p in _path)
            {
                int pAge = p.Item1;
                Vector2 pDirection = p.Item2;
                if (_age > pAge)
                {
                    _direction = pDirection;
                }
            }

            Position = Position + (_direction * Speed);
            _age++;
        }

        public override void OnNotify(BaseGameStateEvent gameEvent)
        {
            switch (gameEvent)
            {
                case GameplayEvents.ChopperHitBy m:
                    JustHit(m.HitBy);
                    SendEvent(new GameplayEvents.EnemyLostLife(_life));
                    break;
            }
        }

        private void JustHit(IGameObjectWithDamage o)
        {
            _hitAt = 0;
            _life -= o.Damage;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            var chopperRect = new Rectangle(ChopperStartX, ChopperStartY, ChopperWidth, ChopperHeight);
            var chopperDestRectangle = new Rectangle(_position.ToPoint(), new Point(ChopperWidth, ChopperHeight));
            var color = Color.White;

            var bladesRect = new Rectangle(BladesStartX, BladesStartY, BladesWidth, BladesHeight);
            var bladesDestRect = new Rectangle(_position.ToPoint(), new Point(BladesWidth, BladesHeight));
            
            spriteBatch.Draw(_texture, chopperDestRectangle, chopperRect, color, MathHelper.Pi, 
                new Vector2(ChopperBladePosX, ChopperBladePosY), SpriteEffects.None, 0f);
            spriteBatch.Draw(_texture, bladesDestRect, bladesRect, Color.White, _angle, 
                new Vector2(BladesCenterX, BladesCenterY), SpriteEffects.None, 0f);
            _angle += BladeSpeed;
        }
    }
}