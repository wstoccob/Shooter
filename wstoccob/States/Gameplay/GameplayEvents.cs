using Microsoft.Xna.Framework;
using wstoccob.Engine.Objects;
using wstoccob.Engine.States;

namespace wstoccob.States.Gameplay
{
    public class GameplayEvents : BaseGameStateEvent
    {
        public class PlayerShoots : GameplayEvents { }
        public class PlayerShootsMissile : GameplayEvents { }

        public class PlayerDies : GameplayEvents { }

        public class ChopperHitBy : GameplayEvents
        {
            public IGameObjectWithDamage HitBy { get; private set; }

            public ChopperHitBy(IGameObjectWithDamage gameObject)
            {
                HitBy = gameObject;
            }
            public ChopperHitBy() { }
        }

        public class EnemyLostLife : GameplayEvents
        {
            public int CurrentLife { get; private set; }

            public EnemyLostLife(int currentLife)
            {
                CurrentLife = currentLife;
            }
        }
    }
}