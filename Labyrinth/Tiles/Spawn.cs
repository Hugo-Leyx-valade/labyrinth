using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinth.Tiles
{
    internal class Spawn: Room
    {

        public int X { get; set; }
        public int Y { get; set; }
        public Spawn(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
