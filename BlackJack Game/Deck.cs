using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Game
{
    internal class Deck
    {
        // fields for cards and current index
        private Cards[] cards;
        private int currentIndex;
        private Random shuffler;

        // Constructor to initialize deck
        public Deck()
        {
            // initialize fields
            cards = new Cards[52];
            shuffler = new Random();
            currentIndex = 0;

            // Method calls to create and shuffle deck
            CreateDeck();
            Shuffle();
        }

        // Create all 52 card instances
        private void CreateDeck()
        {
            string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
            string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

            int index = 0;

            for (int s = 0; s < suits.Length; s++)
            {
                for (int r = 0; r < ranks.Length; r++)
                {
                    int value;

                    if (ranks[r] == "J" || ranks[r] == "Q" || ranks[r] == "K")
                        value = 10;
                    else if (ranks[r] == "A")
                        value = 11;
                    else
                        value = int.Parse(ranks[r]);

                    cards[index] = new Cards(suits[s], ranks[r], value);
                    index++;
                }
            }
        }

        // Shuffle the deck 
        private void Shuffle()
        {
            for (int currentIndex = 0; currentIndex < cards.Length; currentIndex++)
            {
                int randomIndex = shuffler.Next(cards.Length); // get random index

                // swap cards
                Cards tempCard = cards[currentIndex];
                cards[currentIndex] = cards[randomIndex];
                cards[randomIndex] = tempCard;
            }
        }


        // Draw one card
        public Cards DrawCard()
        {
            Cards card = cards[currentIndex];
            currentIndex++;
            return card;
        }

    }
}
