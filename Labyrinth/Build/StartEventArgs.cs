using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinth.Build
{
    public class  StartEventArgs : EventArgs
    {

        public int X { get; }
        public int Y { get; }
        public StartEventArgs(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

}