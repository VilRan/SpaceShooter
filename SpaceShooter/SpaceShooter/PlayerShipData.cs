using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public class PlayerShipData
    {
        public Durability Durability;

        public PlayerShipData()
        {
            Durability = new Durability(100);
        }
    }
}
