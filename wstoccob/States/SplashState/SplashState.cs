using wstoccob.Engine.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using wstoccob.Objects;
using wstoccob.States;
using wstoccob.Engine.Input;
using wstoccob.Input;

namespace wstoccob.State
{
    public class SplashState : BaseGameState
    {
        public override void LoadContent(ContentManager contentManager)
        {
            AddGameObject(new SplashImage(contentManager.Load<Texture2D>("splash")));
        }
        public override void HandleInput(GameTime gameTime)
        {
            InputManager.GetCommands(cmd =>
            {
                if (cmd is SplashInputCommand.GameSelect)
                {
                    SwitchState(new GameplayState());
                }
            });
        }

        public override void UpdateGameState(GameTime _) { }

        protected override void SetInputManager()
        {
            InputManager = new InputManager(new SplashInputMapper());
        }
    }
}