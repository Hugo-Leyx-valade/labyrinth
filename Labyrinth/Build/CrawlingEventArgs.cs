using Labyrinth.Crawl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinth.Build
{
    public class CrawlingEventArgs: EventArgs
    {

        public int X { get; }
        public int Y { get; }

        public Direction Direction { get; set; }
        public CrawlingEventArgs(int x, int y, Direction direction)
        {
            this.X = x;
            this.Y = y;
            this.Direction = direction;
        }
    }
}
