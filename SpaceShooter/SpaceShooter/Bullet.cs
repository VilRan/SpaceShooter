using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    class Bullet : GameObject
    {
        const float speed = 1024;

        public Bullet(Texture2D texture, Vector2 position)
            : base(texture)
        {
            HP = 10;
            Position = position;
            Velocity = new Vector2(speed, 0);
            Faction = Faction.Player;
        }

        public override void OnCollision(GameObject other)
        {
            other.HP -= 100;
        }
    }
}
