using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using wstoccob.Engine.Objects;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using wstoccob.Engine.Input;
using wstoccob.Engine.Sound;

namespace wstoccob.Engine.States
{
    public abstract class BaseGameState
    {
        private const string FallbackTexture = "Empty";
        private const string EmptySound = "emptySound";
        
        private ContentManager _contentManager;
        protected SoundManager _soundManager = new SoundManager();
        
        protected int _viewportHeight;
        protected int _viewportWidth;

        protected bool _debug = false;
        
        private readonly List<BaseGameObject> _gameObjects = new List<BaseGameObject>();
        
        protected InputManager InputManager { get; set; }
        
        protected Texture2D LoadTexture(string textureName)
        {
            var texture = _contentManager.Load<Texture2D>(textureName);
            return texture ?? _contentManager.Load<Texture2D>(FallbackTexture);
        }

        protected SoundEffect LoadSound(string soundName)
        {
            var soundEffect = _contentManager.Load<SoundEffect>(soundName);
            return soundEffect ?? _contentManager.Load<SoundEffect>(EmptySound);
        }
        public void Initialize(ContentManager contentManager, int viewportWidth, int viewportHeight)
        {
            _contentManager = contentManager;
            _viewportWidth = viewportWidth;
            _viewportHeight = viewportHeight;
            
            SetInputManager();
        }
        
        public abstract void LoadContent();
        public abstract void HandleInput(GameTime gameTime);
        public abstract void UpdateGameState(GameTime gameTime);
        
        public event EventHandler<BaseGameState> OnStateSwitched;
        public event EventHandler<BaseGameStateEvent> OnEventNotification;
        protected abstract void SetInputManager();
        
        public void UnloadContent()
        {
            _contentManager.Unload();
        }

        public void Update(GameTime gameTime)
        {
            UpdateGameState(gameTime);
            _soundManager.PlaySoundtrack();
        }
        protected void SwitchState(BaseGameState gameState)
        {
            OnStateSwitched?.Invoke(this, gameState);
        }
        protected void AddGameObject(BaseGameObject gameObject)
        {
            _gameObjects.Add(gameObject);
        }

        protected void RemoveGameObject(BaseGameObject gameObject)
        {
            _gameObjects.Remove(gameObject);
        }
        public void Render(SpriteBatch spriteBatch)
        {
            foreach (var gameObject in _gameObjects.OrderBy(a => a.zIndex))
            {
                gameObject.Render(spriteBatch);
                if (_debug)
                {
                    gameObject.RenderBoundingBoxes(spriteBatch);
                }
            }
        }
        protected void NotifyEvent(BaseGameStateEvent gameEvent)
        {
            OnEventNotification?.Invoke(this, gameEvent);
            foreach (var gameObject in _gameObjects)
            {
                gameObject.OnNotify(gameEvent);
            }
            _soundManager.OnNotify(gameEvent);
        }
    }
}