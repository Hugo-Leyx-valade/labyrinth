using Labyrinth.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinth.Crawl
{
    internal class RandomExplorer
    {

        public ICrawler _crawler;
        public RandomExplorer(ICrawler crawler)
        {
            _crawler = crawler;
        }

        private readonly Random _random = new();

        public bool GetOut(int n)
        {
            int i = 0;
            while (i < n)
            {
                // Vérifie si la tuile actuelle est "Outside"
                if (this._crawler.FacingTile is Outside)
                {
                    Console.WriteLine("🚪 Tu es sorti !");
                    return true;
                }

                // Choix aléatoire d’une action : 0 = Walk, 1 = TurnRight, 2 = TurnLeft
                int action = _random.Next(3);

                switch (action)
                {
                    case 0:
                        if (this._crawler.FacingTile.IsTraversable)
                        {
                            OnPositionChanged();
                            this._crawler.Walk();
                            i++;
                        }
                        break;
                    case 1:
                        OnDirectionChanged();
                        this._crawler.Direction.TurnRight();
                        break;
                    case 2:
                        OnDirectionChanged();
                        this._crawler.Direction.TurnLeft();
                        break;
                }
            }

            Console.WriteLine("❌ Nombre maximal de déplacements atteint sans sortie.");
            return false;
        }

        private void OnPositionChanged()
        {
            Console.WriteLine($"📍 Position actuelle : ({this._crawler.X}, {this._crawler.Y})");
        }

        private void OnDirectionChanged()
        {
            Console.WriteLine($"🧭 Direction actuelle : {this._crawler.Direction}");
        }
    }
}
