using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Game
{
    internal class Cards
    {
        // Fields for suit, rank, and value
        private string suit;
        private string rank;
        private int value;

        // Constructor
        public Cards(string suit, string rank, int value)
        {
            this.suit = suit;
            this.rank = rank;
            this.value = value;
        }

        //Getters Method
        public string GetSuit() // Suit is the type of card (Hearts, Diamonds, Clubs, Spades)
        {
          return suit;
        }
        public string GetRank() // Rank is the number or face of the card (2-10, Jack, Queen, King, Ace)
        {
          return rank;
        }
        public int GetValue() // Value is the numerical value of the card in Blackjack
        {
          return value;
        }
        public string DisplayCard() // Display the card as a string
        {
          return $"{rank} of {suit}";
        }
    }
}
