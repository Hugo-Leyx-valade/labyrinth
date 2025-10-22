using Labyrinth.Crawl;
using System.Text;
using System;
using System.Text;
using Labyrinth.Crawl;

namespace Labyrinth
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var map = """
                +-------- +
                | x  |    |
                |    +--+ |
                |    |  | |
                |    +--+ |
                |         |
                +---------+
                """;

            try
            {

                var labyrinth = new Labyrinth(map);
                var crawler = labyrinth.NewCrawler();

                Console.OutputEncoding = Encoding.UTF8;
                Console.CursorVisible = false;

                var mapString = labyrinth.ToString();
                var lines = mapString.Split('\n', StringSplitOptions.None);
                for (int row = 0; row < lines.Length; row++)
                {
                    var line = lines[row].TrimEnd('\r');
                    Console.SetCursorPosition(0, row);
                    Console.Write(line);
                    lines[row] = line;
                }

                int prevX = crawler.X, prevY = crawler.Y;
                Direction prevDir = crawler.Direction;

                static string ArrowFor(Direction dir) =>
                    dir == Direction.North ? "⬆"
                  : dir == Direction.South ? "⬇"
                  : dir == Direction.West ? "⬅"
                  : "⮕"; // East

                void DrawArrow(int x, int y, Direction dir)
                {
                    EraseAt(prevX, prevY);
                    var line = lines[y];
                    Console.SetCursorPosition(x, y);
                    Console.Write(ArrowFor(dir));
                }

                void EraseAt(int x, int y)
                {
                    var line = lines[y];
                    Console.SetCursorPosition(x, y);
                    Console.Write(line[x]);
                }

                RandomExplorer.ChangePosition += (_, e) =>
                {
                    EraseAt(prevX, prevY);
                    DrawArrow(e.X, e.Y, e.Direction);
                    prevX = e.X;
                    prevY = e.Y;
                    prevDir = e.Direction;
                };

                RandomExplorer.ChangeDirection += (_, e) =>
                {
                    DrawArrow(e.X, e.Y, e.Direction);
                    prevDir = e.Direction;
                };

                var explorer = new RandomExplorer(crawler);
                Console.SetCursorPosition(0, lines.Length + 1);
                Console.WriteLine("Début de l'exploration aléatoire...");
                explorer.GetOut(100); // nombre maximal d'actions

                Console.CursorVisible = true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erreur lors de l'initialisation : {ex.Message}");
            }
        }
    }
}