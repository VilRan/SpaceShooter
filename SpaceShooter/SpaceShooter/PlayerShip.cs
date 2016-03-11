using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceShooter
{
    class PlayerShip : GameObject
    {
        const float maxSpeed = 256;

        double ReloadTimer = 0.0;

        public PlayerShip(Texture2D texture)
            : base(texture)
        {
            HP = 100;
            Position = new Vector2(64, 384);
            Faction = Faction.Player;
        }

        public override void Update(GameTime gameTime, Level level)
        {
            if (ReloadTimer > 0)
                ReloadTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            Velocity = Vector2.Zero;

            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Up))
                Velocity += new Vector2(0, -maxSpeed);
            if (keyboard.IsKeyDown(Keys.Down))
                Velocity += new Vector2(0, maxSpeed);
            if (keyboard.IsKeyDown(Keys.Left))
                Velocity += new Vector2(-maxSpeed, 0);
            if (keyboard.IsKeyDown(Keys.Right))
                Velocity += new Vector2(maxSpeed, 0);
            if (keyboard.IsKeyDown(Keys.Space) && ReloadTimer <= 0)
            {
                Bullet bullet = new Bullet(level.Game.Assets.BulletTexture, Position);
                level.SpawnObject(bullet);
                ReloadTimer += 0.1;
            }

            base.Update(gameTime, level);
        }
    }
}
