using System.Dynamic;
using Microsoft.Xna.Framework;

namespace wstoccob.Engine.Particles.EmitterTypes
{
    public abstract class EmitterParticleState
    {
        public abstract int MinLifeSpan { get; }
        public abstract int MaxLifeSpan { get; }

        public abstract float Velocity { get; }
        public abstract float VelocityDeviation { get; }
        public abstract float Acceleration { get; }
        public abstract Vector2 Gravity { get; }
        
        public abstract float Opacity { get; }
        public abstract float OpacityDeviation { get; }
        public abstract float OpacityFadingRate { get; }

        public abstract float Rotation { get; }
        public abstract float RotationDeviation { get; }

        public abstract float Scale { get; }
        public abstract float ScaleDeviation { get; }
        
        
    }
}