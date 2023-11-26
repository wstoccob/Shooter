using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using wstoccob.Engine.Input;

namespace wstoccob.Engine.Input
{
    public class BaseInputMapper
    {
        public virtual IEnumerable<BaseInputCommand> GetKeyboardState(KeyboardState state)
        {
            return new List<BaseInputCommand>();
        }
        public virtual IEnumerable<BaseInputCommand> GetMouseState(MouseState state)
        {
            return new List<BaseInputCommand>();
        }
        public virtual IEnumerable<BaseInputCommand> GetGamePadState(GamePadState state)
        {
            return new List<BaseInputCommand>();
        }
    }
}