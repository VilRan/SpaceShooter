﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Particles
{
    class BackgroundParticle : Particle
    {
        float distance;

        public override bool IsRemoving { get { return false; } }

        public BackgroundParticle(Texture2D texture, float distance)
            : base (texture)
        {
            this.distance = distance;
        }

        public override void Draw(DrawEventArgs e)
        {
            Velocity = e.Level.Camera.Velocity * distance;
            if (Position.X < e.Level.PlayArea.Left)
                Position.X = e.Level.PlayArea.Right;
            base.Draw(e);
        }
    }
}
