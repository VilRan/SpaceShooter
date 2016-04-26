using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Dynamic.Ships;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Dynamic.PowerUps
{
    class Coin : PowerUp
    {
        public Coin(Level level, Vector2 position, Vector2 velocity) 
            : base(level.Game.Assets.CoinTexture, level, position, velocity)
        {

        }

        public override void OnCollision(Collision e)
        {
            PlayerShip playerShip = e.Other as PlayerShip;
            if (playerShip != null)
            {
                Player player = playerShip.Player;
                player.Money += 10;
            }
        }
    }
}
