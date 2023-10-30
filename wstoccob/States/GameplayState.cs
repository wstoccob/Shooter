using wstoccob.Enum;
using wstoccob.States.Base;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using wstoccob.Objects;

namespace wstoccob.States
{
    public class GameplayState : BaseGameState
    {
        private const string PlayerFighter = "fighter";
        private const string BackgroundTexture = "Barren";
        public override void LoadContent(ContentManager contentManager)
        {
            AddGameObject(new SplashImage(LoadTexture(BackgroundTexture)));
            AddGameObject(new SplashImage(LoadTexture(PlayerFighter)));
        }
        public override void HandleInput()
        {
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Escape))
            {
                NotifyEvent(Events.GAME_QUIT);
            }
        }
    }
}