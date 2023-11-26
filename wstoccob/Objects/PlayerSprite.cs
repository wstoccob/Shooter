using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using wstoccob.Engine.Objects;


namespace wstoccob.Objects
{
    public class PlayerSprite : BaseGameObject
    {
        private const float PLAYER_SPEED = 10.0f;
        public PlayerSprite(Texture2D texture)
        {
            _texture = texture;
        }

        public void MoveLeft()
        {
            Position = new Vector2(Position.X - PLAYER_SPEED, Position.Y);
        }

        public void MoveRight()
        {
            Position = new Vector2(Position.X + PLAYER_SPEED, Position.Y);
        }

        
    }
}