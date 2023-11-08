using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using wstoccob.Objects.Base;

namespace wstoccob.Objects
{
    public class TerrainBackground : BaseGameObject
    {
        public const float SCROLLING_SPEED = 2.0f;

        public TerrainBackground(Texture2D texture)
        {
            _texture = texture;
            _position = new Vector2(0, 0);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            var viewport = spriteBatch.GraphicsDevice.Viewport;
            var sourceRectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);
            for (int nbVertical = -1; nbVertical < viewport.Height / _texture.Height + 1; nbVertical++)
            {
                var y = (int)_position.Y + nbVertical * _texture.Height;
                for (int nbHorizontal = 0; nbHorizontal < viewport.Width / _texture.Width + 1; nbHorizontal++)
                {
                    var x = (int)_position.X + nbHorizontal * _texture.Width;
                    var destinationRectangle = new Rectangle(x, y, _texture.Width, _texture.Height);
                    spriteBatch.Draw(_texture, destinationRectangle, sourceRectangle, Color.White);
                }
            }

            _position.Y = (int)(_position.Y + SCROLLING_SPEED) % _texture.Height;
        }


    }
}