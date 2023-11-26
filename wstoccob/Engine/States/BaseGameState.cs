using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using wstoccob.Objects.Base;
using wstoccob.Enum;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using wstoccob.Input.Base;

namespace wstoccob.States.Base
{
    public abstract class BaseGameState
    {
        private const string FallbackTexture = "Empty";
        
        private ContentManager _contentManager;
        protected int _viewportHeight;
        protected int _viewportWidth;
        
        private readonly List<BaseGameObject> _gameObjects = new List<BaseGameObject>();
        
        protected InputManager InputManager { get; set; }
        
        protected Texture2D LoadTexture(string textureName)
        {
            var texture = _contentManager.Load<Texture2D>(textureName);
            return texture ?? _contentManager.Load<Texture2D>(FallbackTexture);
        }
        public void Initialize(ContentManager contentManager, int viewportWidth, int viewportHeight)
        {
            _contentManager = contentManager;
            _viewportWidth = viewportWidth;
            _viewportHeight = viewportHeight;
            
            SetInputManager();
        }
        
        public abstract void LoadContent(ContentManager contentManager);
        public virtual void Update(GameTime gameTime) { }
        public abstract void HandleInput(GameTime gameTime);
        
        public event EventHandler<BaseGameState> OnStateSwitched;
        public event EventHandler<Events> OnEventNotification;
        protected abstract void SetInputManager();
        
        public void UnloadContent()
        {
            _contentManager.Unload();
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
            }
        }
        public void NotifyEvent(Events eventType, object argument = null)
        {
            OnEventNotification?.Invoke(this, eventType);
            foreach (var gameObject in _gameObjects)
            {
                gameObject.OnNotify(eventType);
            }
        }
    }
}