using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Dynamic
{
    public struct Durability
    {
        public float Current { get { return current; } set { current = Math.Min(value, maximum); } }
        public float Maximum { get { return maximum; } set { maximum = value; Current = current; } }
        public float Both { set { maximum = value;  current = value; } }

        float current;
        float maximum;
    }
}
