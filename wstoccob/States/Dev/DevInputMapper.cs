using System.Collections.Generic;
using System.ComponentModel.Design;
using Microsoft.Xna.Framework.Input;
using wstoccob.Engine.Input;

namespace wstoccob.States.Dev
{
    public class DevInputMapper : BaseInputMapper
    {
        public override IEnumerable<BaseInputCommand> GetKeyboardState(KeyboardState state)
        {
            var commands = new List<DevInputCommand>();
            if (state.IsKeyDown(Keys.Escape))
            {
                commands.Add(new DevInputCommand.DevQuit());
            }
            if (state.IsKeyDown(Keys.Space))
            {
                commands.Add(new DevInputCommand.DevShoot());
            }
            return commands;
        }
        
    }
}