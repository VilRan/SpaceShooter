﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.Particles
{
    class ExplosionParticle : TimedParticle
    {
        public ExplosionParticle(Level level,Texture2D texture, Vector2 position, Vector2 velocity)
            : base(texture, position, velocity, Color.OrangeRed, 0.7)
        {
            TimedParticle.Emit(level, position, Color.White, 0.2, 0.8, 800, 100, 500);
            TimedParticle.Emit(level, position, Color.Red, 0.2, 0.5, 200, 300, 800);
            TimedParticle.Emit(level, position, Color.OrangeRed, 0.2, 0.4, 200, 100, 300);
        }
        
    }
}
