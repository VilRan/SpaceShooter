﻿using Microsoft.Xna.Framework;
using SpaceShooter.Dynamic.Ships;

namespace SpaceShooter.Dynamic.PowerUps
{
    class RepairKit : PowerUp
    {
        public RepairKit(Level level, Vector2 position, Vector2 velocity)
            : base(level.Game.Assets.BulletTexture, level, position, velocity)
        {

        }

        public override void OnCollision(Collision e)
        {
            e.Other.Repair(200);
        }
    }
}