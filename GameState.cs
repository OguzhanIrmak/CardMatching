using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMatching
{
    public class GameState
    {
        private List<Card> cards;
        private readonly string[] imagePaths =
        {
            "C:\\Users\\oguzh\\source\\repos\\CardMatching\\Images\\daisy.png",
            "C:\\Users\\oguzh\\source\\repos\\CardMatching\\Images\\flower.png",
            "C:\\Users\\oguzh\\source\\repos\\CardMatching\\Images\\lavender.png",
            "C:\\Users\\oguzh\\source\\repos\\CardMatching\\Images\\leaf.png",
            "C:\\Users\\oguzh\\source\\repos\\CardMatching\\Images\\mushroom.png",
            "C:\\Users\\oguzh\\source\\repos\\CardMatching\\Images\\sunflower.png",
            "C:\\Users\\oguzh\\source\\repos\\CardMatching\\Images\\tree (1).png",
            "C:\\Users\\oguzh\\source\\repos\\CardMatching\\Images\\tree.png",
      };

        public GameState() 
        {
            InitializeCards();
            ShuffleCards();
        }

        private void InitializeCards()
        {
            cards = new List<Card>();
            int cardNumber = 1;
            foreach (var imagePath in imagePaths)
            {
                // we add each card twice

                cards.Add(new Card { Id = cardNumber++, ImagePath = imagePath, IsMatched = false });
                cards.Add(new Card { Id = cardNumber++, ImagePath = imagePath, IsMatched = false });
            }
        }

        private void ShuffleCards()
        {
            // I don't know how it works but I couldn't get it to work using random.
            cards = cards.OrderBy(x => Guid.NewGuid()).ToList();
        }

        public List<Card> GetCards()
        {
            return cards;
        }
    }
}
