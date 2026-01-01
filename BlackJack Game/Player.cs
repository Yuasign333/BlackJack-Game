using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Game
{
    internal class Player
    {
        // Fields for hand and money
        private Hand Playerhand;
        private int MoneyBalance;
        private int MinimumBet;

        // Constructor to initialize player with starting balance
        public Player()
        {
            Playerhand = new Hand();

            MoneyBalance = 1000; // Starting balance of $1000 ( can be adjusted as needed )

            MinimumBet = 10; // Minimum bet amount of $10 ( can be adjusted as needed )
        }

        // Method to draw a card from the deck and add to player's hand
        public void DrawCard(Deck deck)
        {
            Playerhand.AddCard(deck.DrawCard());
        }

        // Method called when player wins a bet - adds winnings to balance
        public void WinBet(int amount)
        {
            MoneyBalance += amount;
        }

        // Method called when player loses a bet - deducts from balance
        public void LoseBet(int amount)
        {
            MoneyBalance -= amount;
        }

        // Method to reset hand for a new round of blackjack
        public void ResetHand()
        {
            Playerhand = new Hand();
        }

        // Method to check if player can afford a bet
        public bool CanAffordBet(int amount)
        {
            return MoneyBalance >= amount;
        }

        // Method to check if player is broke
        public bool IsBroke()
        {
            return MoneyBalance <= 0;
        }

        // Getter for current money balance
        public int GetMoneyBalance()
        {
            return MoneyBalance;
        }

        // Getter for minimum bet amount
        public int GetMinimumBet()
        {
            return MinimumBet;
        }

        // Getter for player's hand
        public Hand GetPlayerHand()
        {
            return Playerhand;
        }

    }
}