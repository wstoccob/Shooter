using wstoccob.Engine.Input;

namespace wstoccob.States.Dev
{
    public class DevInputCommand : BaseInputCommand
    {
        public class DevQuit : DevInputCommand { }
        public class DevShoot : DevInputCommand { }
    }
}