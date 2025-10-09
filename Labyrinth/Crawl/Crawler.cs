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

        public int X { get; private set; }

        /// <summary>
        /// Gets the current Y position.
        /// </summary>
        public int Y { get; private set; }

        public Tile[,] tiles { get; }

        /// <summary>
        /// Gets the current direction.
        /// </summary>
        public Direction Direction { get; }

        public Inventory inventory { get; private set; } = new MyInventory();

        /// <summary>
        /// Gets the tile in front of the crawler.
        /// </summary>
        public Tile FacingTile
        {
            get
            {
                int newX = X + this.Direction.DeltaX;
                int newY = Y + this.Direction.DeltaY;

                Tile nextTile;

                // Vérifie que la case est dans les bornes
                if (newX >= 0 && newX < tiles.GetLength(0) &&
                    newY >= 0 && newY < tiles.GetLength(1))
                {
                    nextTile = tiles[newX, newY];
                }
                else
                {
                    // En dehors du labyrinthe → renvoie la tuile spéciale
                    nextTile = Outside.Singleton;
                }

                return nextTile;

            }
        }

        /// <summary>
        /// Pass the tile in front of the crawler and move into it.
        /// </summary>
        /// <returns>An inventory of the collectable items in the place reached.</returns>
        public Inventory Walk()
        {
            Tile next = this.FacingTile;

            // Si c’est un Outside → on ne bouge pas
            if (next is Outside)
            {
                Console.WriteLine("Impossible d'avancer, le crawler est au bord du labyrinthe !");
                return this.inventory; // vide
            }

            // Si la tuile est traversable, on se déplace
            if (next.IsTraversable)
            {
                this.X += this.Direction.DeltaX;
                this.Y += this.Direction.DeltaY;

                Console.WriteLine($"Crawler avance vers ({X}, {Y})");

                // Si la tuile contient des objets → créer un inventaire
                return this.inventory = next.Pass();
            }
            else
            {
                throw new InvalidOperationException("La tuile en face du crawler n'est pas traversable !");
            }
        }
    }
}
