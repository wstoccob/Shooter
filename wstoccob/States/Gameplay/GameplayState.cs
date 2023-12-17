using System;
using System.Collections.Generic;
using wstoccob.Engine.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using wstoccob.Input;
using wstoccob.Objects;
using wstoccob.Engine.Input;
using wstoccob.Engine.Objects;
using wstoccob.Particles;
using wstoccob.States.Gameplay;

namespace wstoccob.States
{
    public class GameplayState : BaseGameState
    {
        private const string PlayerFighter = "Fighter";
        private const string BackgroundTexture = "Barren";
        private const string BulletTexture = "bullet";
        private const string MissileTexture = "Missile05";
        private const string ExhaustTexture = "Cloud001";
        private const string ChopperTexture = "Chopper";
        private const string ExplosionTexture = "explosion";

        private const int MaxExplosionAge = 600; // 10 seconds: 10s * 60 frames
        private const int ExplosionActiveLength = 75; // 1.2 seconds
        
        private PlayerSprite _playerSprite;
        
        private Texture2D _bulletTexture;
        private Texture2D _missileTexture;
        private Texture2D _exhaustTexture;
        private Texture2D _chopperTexture;
        private Texture2D _explosionTexture;
        
        private bool _isShootingMissile;
        private bool _isShootingBullets;
        private TimeSpan _lastBulletShotAt;
        private TimeSpan _lastMissileShotAt;

        private List<BulletSprite> _bulletList;
        private List<MissileSprite> _missileList;
        private List<ExplosionEmitter> _explosionList = new List<ExplosionEmitter>();
        private List<ChopperSprite> _enemyList = new List<ChopperSprite>();

        private ChopperGenerator _chopperGenerator;
        
        public override void LoadContent()
        {
            _playerSprite = new PlayerSprite(LoadTexture(PlayerFighter));
            _bulletTexture = LoadTexture(BulletTexture);
            _bulletList = new List<BulletSprite>();
            _missileTexture = LoadTexture(MissileTexture);
            _exhaustTexture = LoadTexture(ExhaustTexture);
            _missileList = new List<MissileSprite>();
            _explosionTexture = LoadTexture(ExplosionTexture);
            _chopperTexture = LoadTexture(ChopperTexture);
            _chopperGenerator = new ChopperGenerator(_chopperTexture, 4, AddChopper);
            _chopperGenerator.GenerateChoppers();
            
            AddGameObject(new TerrainBackground(LoadTexture(BackgroundTexture)));
            AddGameObject(_playerSprite);
            
            var playerXPos = _viewportWidth / 2 - _playerSprite.Width / 2;
            var playerYPos = _viewportHeight - _playerSprite.Height - 30;
            _playerSprite.Position = new Vector2(playerXPos, playerYPos);

            var track1 = LoadSound("FutureAmbient_1").CreateInstance();
            var track2 = LoadSound("FutureAmbient_2").CreateInstance();
            var track3 = LoadSound("FutureAmbient_3").CreateInstance();
            var track4 = LoadSound("FutureAmbient_4").CreateInstance();
            _soundManager.SetSoundtrack(new List<SoundEffectInstance>() {track1, track2, track3, track4});

            var bulletSound = LoadSound("bulletSound");
            _soundManager.RegisterSound(new GameplayEvents.PlayerShoots(), bulletSound);
            
            var missileSound = LoadSound("missile");
            _soundManager.RegisterSound(new GameplayEvents.PlayerShootsMissile(), missileSound, 0.4f, -0.2f, 0.0f);
        }

        public override void UpdateGameState(GameTime gameTime)
        {
            foreach (var bullet in _bulletList)
            {
                bullet.MoveUp();
            }

            foreach (var missile in _missileList)
            {
                missile.Update(gameTime);
            }
            if (gameTime.TotalGameTime - _lastBulletShotAt > TimeSpan.FromSeconds(0.2))
            {
                _isShootingBullets = false;
            }

            if (gameTime.TotalGameTime - _lastMissileShotAt > TimeSpan.FromSeconds(1.0))
            {
                _isShootingMissile = false;
            }

            foreach (var chopper in _enemyList)
            {
                chopper.Update();
            }

            _bulletList = CleanObjects(_bulletList);
            _missileList = CleanObjects(_missileList);
            _enemyList = CleanObjects(_enemyList);
        }

        public override void HandleInput(GameTime gameTime)
        {
            InputManager.GetCommands(cmd =>
            {
                if (cmd is GameplayInputCommand.GameExit)
                {
                    NotifyEvent(new BaseGameStateEvent.GameQuit());
                }
                if (cmd is GameplayInputCommand.PlayerMoveLeft)
                {
                    _playerSprite.MoveLeft();
                    KeepPlayerInBounds();
                }
                if (cmd is GameplayInputCommand.PlayerMoveRight)
                {
                    _playerSprite.MoveRight();
                    KeepPlayerInBounds();
                }

                if (cmd is GameplayInputCommand.PlayerShoots)
                {
                    Shoot(gameTime);
                }
            });
        }
        private void Shoot(GameTime gameTime)
        {
            if (!_isShootingBullets)
            {
                CreateBullets();
                _isShootingBullets = true;
                _lastBulletShotAt = gameTime.TotalGameTime;
                NotifyEvent(new GameplayEvents.PlayerShoots());
            }

            if(!_isShootingMissile)
            {
                CreateMissile();
                _isShootingMissile = true;
                _lastMissileShotAt = gameTime.TotalGameTime;
                NotifyEvent(new GameplayEvents.PlayerShootsMissile());
            }
        }
        private void CreateBullets()
        {
            var bulletSpriteLeft = new BulletSprite(_bulletTexture);
            var bulletSpriteRight = new BulletSprite(_bulletTexture);
            
            var bulletY = _playerSprite.Position.Y + 30;
            var bulletLeftX = _playerSprite.Position.X + _playerSprite.Width / 2 - 40;
            var bulletRightX = _playerSprite.Position.X + _playerSprite.Width / 2 + 10;

            bulletSpriteLeft.Position = new Vector2(bulletLeftX, bulletY);
            bulletSpriteRight.Position = new Vector2(bulletRightX, bulletY);
            _bulletList.Add(bulletSpriteLeft);
            _bulletList.Add(bulletSpriteRight);
            AddGameObject(bulletSpriteLeft);
            AddGameObject(bulletSpriteRight);
        }

        private void CreateMissile()
        {
            var missileSprite = new MissileSprite(_missileTexture, _exhaustTexture);
            missileSprite.Position = new Vector2(_playerSprite.Position.X + 33, _playerSprite.Position.Y - 25);
            _missileList.Add(missileSprite);
            AddGameObject(missileSprite);
        }

        private void AddChopper(ChopperSprite chopper)
        {
            chopper.OnObjectChanged += _chopperSprite_OnObjectChanged;
            _enemyList.Add(chopper);
            AddGameObject(chopper);
        }

        private void _chopperSprite_OnObjectChanged(object sender, BaseGameStateEvent e)
        {
            
        }

        private void AddExplosion(Vector2 position)
        {
            var explosion = new ExplosionEmitter(_explosionTexture, position);
            AddGameObject(explosion);
            _explosionList.Add(explosion);
        }

        private void UpdateExplosions(GameTime gameTime)
        {
            foreach (var explosion in _explosionList)
            {
                explosion.Update(gameTime);
                if (explosion.Age > ExplosionActiveLength)
                {
                    explosion.Deactivate();
                }

                if (explosion.Age > MaxExplosionAge)
                {
                    RemoveGameObject(explosion);
                }
            }
        }

        private List<T> CleanObjects<T>(List<T> objectList) where T : BaseGameObject
        {
            List<T> listOfItemsToKeep = new List<T>();
            foreach (T item in objectList)
            {
                var stillOnScreen = item.Position.Y > -50;
                if (stillOnScreen)
                {
                    listOfItemsToKeep.Add(item);
                }
                else
                {
                    RemoveGameObject(item);
                }
            }

            return listOfItemsToKeep;
        }
        protected override void SetInputManager()
        {
            InputManager = new InputManager(new GameplayInputMapper());
        }
        
        private void KeepPlayerInBounds()
        {
            if (_playerSprite.Position.X < 0)
            {
                _playerSprite.Position = new Vector2(0, _playerSprite.Position.Y);
            }

            if (_playerSprite.Position.X > _viewportWidth - _playerSprite.Width)
            {
                _playerSprite.Position = new Vector2(_viewportWidth - _playerSprite.Width, _playerSprite.Position.Y);
            }

            if (_playerSprite.Position.Y < 0)
            {
                _playerSprite.Position = new Vector2(_playerSprite.Position.X, 0);
            }

            if (_playerSprite.Position.Y > _viewportHeight - _playerSprite.Height)
            {
                _playerSprite.Position = new Vector2(_playerSprite.Position.X, _viewportHeight - _playerSprite.Height);
            }
        }
    }
}