using Labyrinth.Crawl;
using Labyrinth.Tiles;
using System.Numerics;
using System.Text;

namespace Labyrinth
{
    public class Labyrinth
    {
        /// <summary>
        /// Labyrinth with walls, doors and collectable items.
        /// </summary>
        /// <param name="ascii_map">A multiline string with '+', '-' or '|' for walls, '/' for doors and 'k' for key locations</param>
        /// <exception cref="ArgumentException">Thrown when string argument reveals inconsistent map sizes or characters.</exception>
        /// <exception cref="NotSupportedException">Thrown for multiple doors (resp. key locations) before key locations (resp. doors).</exception>
        public Labyrinth(string ascii_map)
        {
            _tiles = Build.AsciiParser.Parse(ascii_map);
            if (_tiles.GetLength(0) < 3 || _tiles.GetLength(1) < 3)
            {
                throw new ArgumentException("Labyrinth must be at least 3x3");
            }
            this.spawn = _tiles.Cast<Tile>().OfType<Spawn>().LastOrDefault();

            if (this.spawn == null)
            {
                throw new ArgumentException("Labyrinth must have exactly one spawn point 'x'");
            }

        }

        /// <summary>
        /// Labyrinth width (number of columns).
        /// </summary>
        public int Width { get; private init; }

        /// <summary>
        /// Labyrinth height (number of rows).
        /// </summary>
        public int Height { get; private init; }

        /// <summary>
        /// An ascii representation of the labyrinth.
        /// </summary>
        /// <returns>Formatted string</returns>
        /// 

        private Spawn spawn { get; set; }
        public override string ToString()
        {
            var res = new StringBuilder();

            for (int y = 0; y < _tiles.GetLength(1); y++)
            {
                for (int x = 0; x < _tiles.GetLength(0); x++)
                {
                    res.Append(_tiles[x, y] switch
                    {
                        Spawn => 'x',
                        Room => ' ',
                        Wall => '#',
                        Door => '/',
                        _ => throw new NotSupportedException("Unknown tile type")
                    });
                }
                res.AppendLine();
            }
            return res.ToString();
        }
        
        public ICrawler NewCrawler() => new Crawler(spawn.X, spawn.Y,_tiles);
        private readonly Tile[,] _tiles;
    }
}
