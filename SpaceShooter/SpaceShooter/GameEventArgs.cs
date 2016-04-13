﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public class DrawEventArgs
    {
        public readonly Level Level;
        public readonly GameTime GameTime;
        public readonly SpriteBatch SpriteBatch;

        public DrawEventArgs(Level level, GameTime gameTime, SpriteBatch spriteBatch)
        {
            Level = level;
            GameTime = gameTime;
            SpriteBatch = spriteBatch;
        }
    }
    
    public class UpdateEventArgs
    {
        public readonly GameTime GameTime;

        public double ElapsedSeconds { get { return GameTime.ElapsedGameTime.TotalSeconds; } }

        public UpdateEventArgs(GameTime gameTime)
        {
            GameTime = gameTime;
        }
    }

    public class CollisionEventArgs
    {
        public readonly DynamicObject Other;
        public readonly float TimeOfCollision;

        public CollisionEventArgs(DynamicObject other, float timeOfCollision)
        {
            Other = other;
            TimeOfCollision = timeOfCollision;
        }
    }

    public class DamageEventArgs
    {
        public readonly CollisionEventArgs Collision;
        public readonly float DamageAmount;

        public DamageEventArgs(CollisionEventArgs collision, float damageAmount)
        {
            Collision = collision;
            DamageAmount = damageAmount;
        }
    }

    public class DeathEventArgs
    {
        public DeathEventArgs()
        {

        }
    }
}