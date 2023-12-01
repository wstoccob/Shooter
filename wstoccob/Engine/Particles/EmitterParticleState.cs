using Microsoft.Xna.Framework;
using wstoccob.Engine.Particles.EmitterTypes;

namespace wstoccob.Engine.Particles
{
    public abstract class EmitterParticleState
    {
        public RandomNumberGenerator _rnd = new RandomNumberGenerator();
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

        public int GenerateLifespan()
        {
            return _rnd.NextRandom(MinLifeSpan, MaxLifeSpan);
        }

        public float GenerateVelocity()
        {
            return _rnd.NextRandom(Velocity, VelocityDeviation);
        }

        public float GenerateOpacity()
        {
            return GenerateFloat(Opacity, OpacityDeviation);
        }

        public float GenerateRotation()
        {
            return GenerateFloat(Rotation, RotationDeviation);
        }

        public float GenerateScale()
        {
            return GenerateFloat(Scale, ScaleDeviation);
        }

        protected float GenerateFloat(float startN, float deviation)
        {
            var halfDeviation = deviation / 2.0f;
            return _rnd.NextRandom(startN - halfDeviation, startN + halfDeviation);
        }
    }
}