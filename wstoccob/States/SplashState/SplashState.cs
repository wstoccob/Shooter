using System.Diagnostics;
using wstoccob.States.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using wstoccob.Input;
using wstoccob.Input.Base;
using wstoccob.Objects;
using wstoccob.States;

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
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Enter))
            {
                SwitchState(new GameplayState());
            }
        }

        protected override void SetInputManager()
        {
            InputManager = new InputManager(new SplashInputMapper());
        }
    }
}