using Labyrinth.Items;
using Labyrinth.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinth.Crawl
{
    internal class Crawler : ICrawler
    {
        public Crawler(int X, int Y, Tile[,] tiles)
        {
            this.X = X;
            this.Y = Y;
            this.Direction = Direction.North;
            this.tiles = tiles;
        }

        public int X { get; }

        /// <summary>
        /// Gets the current Y position.
        /// </summary>
        public int Y { get; }

        public Tile[,] tiles { get; }

        /// <summary>
        /// Gets the current direction.
        /// </summary>
        public Direction Direction { get; }

        /// <summary>
        /// Gets the tile in front of the crawler.
        /// </summary>
        public Tile FacingTile { 
            get {
                return tiles[X + this.Direction.DeltaX, Y + this.Direction.DeltaY];
                
            } 
        }

        /// <summary>
        /// Pass the tile in front of the crawler and move into it.
        /// </summary>
        /// <returns>An inventory of the collectable items in the place reached.</returns>
        public Inventory Walk()
        {
            return new MyInventory();

        }
    }
}
