using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Game
{
    internal class Computer_Dealer
    {
        // fields for hand
        private Hand Computerhand;

        // Constructor to initialize player
        public Computer_Dealer()
        {
            Computerhand = new Hand();
        }

        // Method to draw a card from the deck
        public void DrawCard(Deck deck)
        {
            Computerhand.AddCard(deck.DrawCard()); // Draw a card from the deck and add to player's hand
        }

        // Method to reset hand for a new round of blackjack
        public void ResetHand()
        {
            Computerhand = new Hand();
        }

        // Getter for hand
        public Hand GetComputerHand()
        {
            return Computerhand;
        }

       
    }
}
