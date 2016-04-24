using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Particles;
using System;

namespace SpaceShooter.Dynamic.Ships
{
    public abstract class Ship : DynamicObject
    {
        public abstract int Score { get;}
        public override ObjectCategory Category { get { return ObjectCategory.Ship; } }
        protected virtual bool IsHealthbarVisible { get { return CurrentDurability < MaximumDurability; } }
        protected Point HealthbarSize { get { return new Vector2(HitRadius * 4, HitRadius / 4).ToPoint(); } }
        protected Point HealthbarPosition { get { return (Position + new Vector2(-HealthbarSize.X / 2, HitRadius * 2) - Camera.Position).ToPoint(); } }

        public Ship(Texture2D texture, Level level, Vector2 position, float durability, Faction faction)
            : base(texture, level, position, Vector2.Zero, durability, faction)
        {

        }

        public override void Draw(DrawEventArgs e)
        {
            base.Draw(e);
            if (IsHealthbarVisible)
                DrawHealthbar(e);
        }

        void DrawHealthbar(DrawEventArgs e)
        {
            Point coloredSize = new Point((int)(HealthbarSize.X * CurrentDurability / MaximumDurability), HealthbarSize.Y);
            Rectangle destination = new Rectangle(HealthbarPosition, HealthbarSize);
            Rectangle coloredDestination = new Rectangle(HealthbarPosition, coloredSize);
            e.SpriteBatch.Draw(Game.Assets.PixelTexture, destination, Color.DarkGray);
            e.SpriteBatch.Draw(Game.Assets.PixelTexture, coloredDestination, Color.Red);
        }

        public override void OnDeath(DeathEventArgs e)
        {
            base.OnDeath(e);
            Level.Session.Score += Score;
        }

        protected override void OnDeathEffects(DeathEventArgs e)
        {
            base.OnDeathEffects(e);
            SoundEffectInstance sound = Level.Game.Assets.ExplosionSound.CreateInstance();
            sound.Volume = (float)(0.5 + 0.5 * Level.Game.Random.NextDouble());
            sound.Play();
            ExplosionParticle explosion = new ExplosionParticle(Level, Game.Assets.ParticleTexture, Position, Velocity);
            Level.Particles.Add(explosion);
        }

        protected override void OnDamageEffects(DamageEventArgs e)
        {
            base.OnDamageEffects(e);
            int minParticles = (int)Math.Round(Math.Min(e.DamageAmount, CurrentDurability) / 10);
            int maxParticles = minParticles * 2;
            TimedParticle.Emit(Level, e.Collision.CollisionPosition, Color.White, 0.25, 1.0, 1024, minParticles, maxParticles);
        }
    }
}
