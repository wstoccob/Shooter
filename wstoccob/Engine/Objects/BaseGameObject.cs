using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using wstoccob.Engine.States;

namespace wstoccob.Engine.Objects
{
    public class BaseGameObject
    {
        protected Texture2D _texture;
        protected Vector2 _position = Vector2.One;
        public int zIndex;
        public event EventHandler<BaseGameStateEvent> OnObjectChanged; 
        
        public int Width
        {
            get { return _texture.Width; }
        }
        public int Height
        {
            get { return _texture.Height; }
        }

        public virtual Vector2 Position 
        { 
            get { return _position; } 
            set { _position = value; } 
        }

        public void SendEvent(BaseGameStateEvent e)
        {
            OnObjectChanged?.Invoke(this, e);
        }
        public virtual void OnNotify(BaseGameStateEvent gameEvent) { }

        public virtual void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }
    }
}