using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlackJack_Game
{
    internal class GraphicsPack
    {
        // Shows the welcome screen with game title and starting info
        public void PrintWelcomeScreen(int startingBalance, int minimumBet)
        {
            Console.Clear();

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(20, 9);
            Console.WriteLine("♠️♥️♦️♣️ A BlackJack Game Demo ♠️♥️♦️♣️");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(100, 10);
            Console.WriteLine(@"
                     _______   __                      __          _____                      __       
                    |       \ |  \                    |  \        |     \                    |  \      
                    | $$$$$$$\| $$  ______    _______ | $$   __    \$$$$$  ______    _______ | $$   __ 
                    | $$__/ $$| $$ |      \  /       \| $$  /  \     | $$ |      \  /       \| $$  /  \
                    | $$    $$| $$  \$$$$$$\|  $$$$$$$| $$_/  $$__   | $$  \$$$$$$\|  $$$$$$$| $$_/  $$
                    | $$$$$$$\| $$ /      $$| $$      | $$   $$|  \  | $$ /      $$| $$      | $$   $$ 
                    | $$__/ $$| $$|  $$$$$$$| $$_____ | $$$$$$\| $$__| $$|  $$$$$$$| $$_____ | $$$$$$\ 
                    | $$    $$| $$ \$$    $$ \$$     \| $$  \$$\\$$    $$ \$$    $$ \$$     \| $$  \$$\
                     \$$$$$$$  \$$  \$$$$$$$  \$$$$$$$ \$$   \$$ \$$$$$$   \$$$$$$$  \$$$$$$$ \$$   \$$
");

            Console.SetCursorPosition(42, 22);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("╔═══════════════════════════════════╗");
            Console.SetCursorPosition(42, 23);
            Console.WriteLine($"║  Starting Balance: ${startingBalance,-14}║");
            Console.SetCursorPosition(42, 24);
            Console.WriteLine($"║  Minimum Bet: ${minimumBet,-19}║");
            Console.SetCursorPosition(42, 25);
            Console.WriteLine("╚═══════════════════════════════════╝");
            Console.SetCursorPosition(46, 27);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            Console.SetCursorPosition(50, 29);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Loading");

            for (int i = 0; i < 3; i++)
            {
                Console.Write(".");
                Thread.Sleep(300);
            }

            Console.ResetColor();
            Console.Clear();
        }

        // Shows the game rules and instructions
        public void PrintInstructions()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n╔════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                 ♠️ BLACKJACK GAME RULES ♠️                      ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
            Console.ResetColor();

            Thread.Sleep(500);

            string[] instructions = new string[]
            {
                "📋 OBJECTIVE:",
                "   Get as close to 21 as possible without going over (busting).",
                "",
                "🎴 CARD VALUES:",
                "   • Number cards (2-10): Face value",
                "   • Face cards (J, Q, K): Worth 10 points",
                "   • Ace: Worth 11 points (or 1 if it would cause a bust)",
                "",
                "🎮 HOW TO PLAY:",
                "   1. Place your bet ($10 minimum)",
                "   2. You and the dealer each receive 2 cards",
                "   3. One dealer card is hidden",
                "   4. Choose to HIT (draw a card) or STAND (keep current hand)",
                "   5. If you go over 21, you BUST and lose",
                "   6. Dealer must hit until reaching 17 or higher",
                "",
                "💰 WINNING:",
                "   • Beat the dealer's hand = Win your bet amount!",
                "   • Dealer busts = You win!",
                "   • Same total = PUSH (bet returned)",
                "   • Dealer's hand wins = Lose your bet",
                "",
                "💵 You start with $1000. Play until you're rich or broke!",
                "",
                "Good luck and have fun! 🍀"
            };

            foreach (string line in instructions)
            {
                if (line.StartsWith("📋") || line.StartsWith("🎴") || line.StartsWith("🎮") || line.StartsWith("💰"))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else if (line.Contains("•") || line.Contains("="))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else if (line.StartsWith("   "))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }

                Console.WriteLine(line);
                Thread.Sleep(300);
            }

            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\nPress any key to start playing...");
            Console.ResetColor();
            Console.ReadKey();
        }

        // Prints a single card 
        public void PrintCard(Cards card)
        {
            if (card == null) return;

            string rank = card.GetRank();
            string suit = GetSuitSymbol(card.GetSuit());

            Console.WriteLine("┌─────────┐");
            Console.WriteLine($"│{rank,-2}       │");
            Console.WriteLine("│         │");
            Console.WriteLine($"│    {suit}    │");
            Console.WriteLine("│         │");
            Console.WriteLine($"│       {rank,-2}│");
            Console.WriteLine("└─────────┘");
        }

        // Prints multiple cards horizontally with optional hiding of the second card
        public void PrintCardsHorizontally(Cards[] cards, bool hideSecondCard = false, int delayMs = 500)
        {
            if (cards == null || cards.Length == 0) return;

            // Each card is 7 lines tall
            int cardHeight = 7;
            string[] cardLines = new string[cardHeight];

            // Loop through each card
            for (int i = 0; i < cards.Length; i++)
            {
                // Skip null cards
                if (cards[i] == null) continue;

                // ⏱️ Animation: Wait before showing next card
                if (i > 0 && delayMs > 0)
                {
                    Thread.Sleep(delayMs);
                }

                string rank = cards[i].GetRank();
                string suit = GetSuitSymbol(cards[i].GetSuit());

                //  Check if this card should be hidden (face-down)
                if (i == 1 && hideSecondCard)
                {
                    // Create a face-down card with shaded pattern
                    string[] hiddenCard = new string[]
                    {
                        "┌─────────┐",
                        "│░░░░░░░░░│",
                        "│░░░░░░░░░│",
                        "│░░░░░░░░░│",
                        "│░░░░░░░░░│",
                        "│░░░░░░░░░│",
                        "└─────────┘"
                    };

                    // Add this card to our display, line by line
                    for (int line = 0; line < cardHeight; line++)
                    {
                        if (string.IsNullOrEmpty(cardLines[line]))
                            cardLines[line] = hiddenCard[line];
                        else
                            cardLines[line] += "  " + hiddenCard[line]; // 2 spaces between cards
                    }
                }
                else
                {
                    // 🃏 Create a normal face-up card
                    string[] currentCard = new string[]
                    {
                        "┌─────────┐",
                        $"│{rank,-2}       │",
                        "│         │",
                        $"│    {suit}    │",
                        "│         │",
                        $"│       {rank,-2}│",
                        "└─────────┘"
                    };

                    // Add this card to our display, line by line
                    for (int line = 0; line < cardHeight; line++)
                    {
                        if (string.IsNullOrEmpty(cardLines[line]))
                            cardLines[line] = currentCard[line];
                        else
                            cardLines[line] += "  " + currentCard[line]; // 2 spaces between cards
                    }
                }

                //  Display the cards (moves cursor back up if not first card)
                Console.SetCursorPosition(0, Console.CursorTop - (i > 0 ? cardHeight : 0));
                foreach (string line in cardLines)
                {
                    Console.WriteLine(line);
                }
            }
        }

        // Prints a single face-down card
        public void PrintHiddenCard()
        {
            Console.WriteLine("┌─────────┐");
            Console.WriteLine("│░░░░░░░░░│");
            Console.WriteLine("│░░░░░░░░░│");
            Console.WriteLine("│░░░░░░░░░│");
            Console.WriteLine("│░░░░░░░░░│");
            Console.WriteLine("│░░░░░░░░░│");
            Console.WriteLine("└─────────┘");
        }

        // Converts suit name to symbol (♥ ♦ ♣ ♠)
        private string GetSuitSymbol(string suit)
        {
            switch (suit)
            {
                case "Hearts": return "♥";
                case "Diamonds": return "♦";
                case "Clubs": return "♣";
                case "Spades": return "♠";
                default: return "?";
            }
        }
    }
}
