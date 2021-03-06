﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpaceShooter.Particles
{
    class TimedParticle : Particle
    {
        public Color StartColor = Color.White;
        public Color EndColor = Color.TransparentBlack;
        Color color;
        double lifespan;
        double life;

        public override bool IsRemoving { get { return life <= 0; } }
        public override ObjectCategory Category { get { return ObjectCategory.Projectile; } }
        protected override Color Color { get { return color; } }

        public TimedParticle(Texture2D texture, Vector2 position, Vector2 velocity, Color startColor, double lifespan)
            : base(texture, position, velocity)
        {
            StartColor = startColor;
            this.lifespan = lifespan;
            life = lifespan;
        }

        public override void Draw(DrawEventArgs e)
        {
            if (!e.Level.PlayArea.Contains(Position))
                life = 0;
            else
                life -= e.GameTime.ElapsedGameTime.TotalSeconds;

            color = Color.Lerp(EndColor, StartColor, (float)(life / lifespan));

            base.Draw(e);
        }

        public override GameObject Clone()
        {
            throw new NotImplementedException();
        }

        public static void Emit(Level level, Texture2D texture, Vector2 position, Color startColor, double minimumLifespan, double maximumLifespan, float maxSpeed, float minAngle = 0, float maxAngle = MathHelper.TwoPi)
        {
            SpaceShooterGame game = level.Game;
            if (game.Random.NextDouble() < game.Settings.ParticleDensity)
                emit(level, texture, position, startColor, minimumLifespan, maximumLifespan, maxSpeed, minAngle, maxAngle);
        }

        public static void Emit(Level level, Texture2D texture, Vector2 position, Color startColor, double minimumLifespan, double maximumLifespan, float maxSpeed, int minCount, int maxCount)
        {
            minCount = (int)Math.Round(minCount * level.Game.Settings.ParticleDensity);
            maxCount = (int)Math.Round(maxCount * level.Game.Settings.ParticleDensity);
            int n = level.Game.Random.Next(minCount, maxCount + 1);
            for (int i = 0; i < n; i++)
                emit(level, texture, position, startColor, minimumLifespan, maximumLifespan, maxSpeed);
        }

        static void emit(Level level, Texture2D texture, Vector2 position, Color startColor, double minimumLifespan, double maximumLifespan, float maxSpeed, float minAngle = 0, float maxAngle = MathHelper.TwoPi)
        {
            SpaceShooterGame game = level.Game;
            Random random = game.Random;
            double particleLifespan = minimumLifespan + (maximumLifespan - minimumLifespan) * random.NextDouble();
            TimedParticle particle = new TimedParticle(texture, position, VectorUtility.CreateRandom(random, maxSpeed, 0, maxAngle, minAngle), startColor, particleLifespan);
            level.Particles.Add(particle);
        }
    }
}
