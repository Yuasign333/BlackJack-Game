using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Game
{
    internal class Hand
    {
        // fields for cards and count of cards in hand
        private Cards[] cards;
        private int CardOnHand;

        // Constructor to initialize hand
        public Hand()
        {
            cards = new Cards[10]; // let's set it to max 10 cards in hand

            CardOnHand = 0; // no cards initially
        }

        // Add a card to the hand
        public void AddCard(Cards card)
        {
            cards[CardOnHand] = card;

            CardOnHand++; // this means we have one more card now
        }

        // Ace logic: since an Ace can be worth 1 or 11, this method ensures
        // the hand total is calculated correctly without exceeding 21.
        public int GetTotal()
        {
            int total = 0;
            int aceCount = 0;

            for (int i = 0; i < CardOnHand; i++)
            {
                total += cards[i].GetValue(); // sum up card values

                if (cards[i].GetRank() == "A") // count Aces
                    aceCount++;
            }

            // If busted and has Ace, reduce Ace once
            if (total > 21 && aceCount > 0)
            {
                total -= 10; // count one Ace as 1 instead of 11
            }
            else
            {
              // else Ace remains 11
            }
            return total;
        }

        // Display all cards in hand
        public void ShowHand() 
        {
            for (int i = 0; i < CardOnHand; i++)
            {
                Console.WriteLine(cards[i].DisplayCard());
            }
        }

        // Getter for cards in hand
        public Cards[] GetCards()
        {
            return cards;
        }

    }
}
