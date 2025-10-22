using System;
using Labyrinth.Crawl;

namespace Labyrinth
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var map = """
                +--+
                | x|
                + -+
                """;

            try
            {
                var labyrinth = new Labyrinth(map);
                Console.WriteLine(labyrinth.ToString());

                // Crée un crawler et un explorateur aléatoire, puis tente de sortir
                var crawler = labyrinth.NewCrawler();
                var explorer = new RandomExplorer(crawler);
                Console.WriteLine("Début de l'exploration aléatoire...");
                explorer.GetOut(5); // nombre maximal d'actions
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erreur lors de l'initialisation : {ex.Message}");
            }
        }
    }
}