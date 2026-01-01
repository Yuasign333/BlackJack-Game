using System;
using System.Text;
using System.Threading;

namespace BlackJack_Game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            StartSystem();
        }

        static void StartSystem()
        {
            GraphicsPack graphics = new GraphicsPack();
            Player player = new Player();

            graphics.PrintWelcomeScreen(player.GetMoneyBalance(), player.GetMinimumBet());
            graphics.PrintInstructions();

            bool keepPlaying = true;

            while (keepPlaying && player.GetMoneyBalance() > 0)
            {
                // Check if player has money
                if (player.GetMoneyBalance() <= 0)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n╔════════════════════════════════════════╗");
                    Console.WriteLine("║     YOU'RE OUT OF MONEY! GAME OVER     ║");
                    Console.WriteLine("╚════════════════════════════════════════╝");
                    Console.ResetColor();
                    break;
                }

                // Display current balance
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n╔════════════════════════════════════════╗");
                Console.WriteLine($"║   Current Balance: ${player.GetMoneyBalance()}");
                Console.WriteLine("╚════════════════════════════════════════╝");
                Console.ResetColor();

                // Get bet amount
                int bet = GetBetAmount(player, player.GetMinimumBet());

                if (bet == 0)
                {
                    break; // Player chose to quit
                }

                // Create new deck and dealer for each round
                Deck deck = new Deck();
                Computer_Dealer dealer = new Computer_Dealer();

                // Reset hands
                player.ResetHand();
                dealer.ResetHand();

                // Initial deal
                player.DrawCard(deck);
                dealer.DrawCard(deck);
                player.DrawCard(deck);
                dealer.DrawCard(deck);

                // Show initial hands
                DisplayInitialHands(player, dealer, graphics);

                // Player's turn
                bool playerBusted = PlayerTurn(player, dealer, deck, graphics);

                // Dealer's turn (only if player didn't bust)
                bool dealerBusted = false;
                if (!playerBusted)
                {
                    dealerBusted = DealerTurn(dealer, deck, graphics);
                }

                // Show final hands
                DisplayFinalHands(player, dealer, graphics);

                // Determine winner and update balance
                DetermineWinner(player, dealer, bet, playerBusted, dealerBusted);

                // Ask to play again
                if (player.GetMoneyBalance() > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("\n\nPlay another round? (y/n): ");
                    Console.ResetColor();
                    string choice = Console.ReadLine().ToLower();
                    keepPlaying = (choice == "y" || choice == "yes");
                }
            }

            // Game over screen
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n╔════════════════════════════════════════╗");
            Console.WriteLine("║       THANKS FOR PLAYING BLACKJACK!    ║");
            Console.WriteLine($"║       Final Balance: ${player.GetMoneyBalance()}");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static int GetBetAmount(Player player, int minimumBet)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"\nEnter your bet amount (${minimumBet} - ${player.GetMoneyBalance()}) or 0 to quit: $");
                Console.ResetColor();

                if (int.TryParse(Console.ReadLine(), out int bet))
                {
                    if (bet == 0) return 0;
                    if (bet >= minimumBet && bet <= player.GetMoneyBalance())
                    {
                        return bet;
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Invalid bet! Must be between ${minimumBet} and ${player.GetMoneyBalance()}");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please enter a valid number!");
                    Console.ResetColor();
                }
            }
        }

        static void DisplayInitialHands(Player player, Computer_Dealer dealer, GraphicsPack graphics)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n═══════════════════════ INITIAL DEAL ═══════════════════════\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("DEALER'S HAND:");
            Console.ResetColor();
            graphics.PrintCard(dealer.GetComputerHand().GetCards()[0]);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n[Hidden Card]");
            Console.ResetColor();

            Console.WriteLine("\n");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("YOUR HAND:");
            Console.ResetColor();
            foreach (Cards card in player.GetPlayerHand().GetCards())
            {
                if (card != null)
                {
                    graphics.PrintCard(card);
                    Console.WriteLine();
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"═══ Total: {player.GetPlayerHand().GetTotal()} ═══");
            Console.ResetColor();

            Thread.Sleep(1500);
        }

        static bool PlayerTurn(Player player, Computer_Dealer dealer, Deck deck, GraphicsPack graphics)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n═══════════════════════ YOUR TURN ═══════════════════════\n");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("DEALER'S HAND:");
                Console.ResetColor();
                graphics.PrintCard(dealer.GetComputerHand().GetCards()[0]);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n[Hidden Card]");
                Console.ResetColor();

                Console.WriteLine("\n");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("YOUR HAND:");
                Console.ResetColor();
                foreach (Cards card in player.GetPlayerHand().GetCards())
                {
                    if (card != null)
                    {
                        graphics.PrintCard(card);
                        Console.WriteLine();
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"═══ Total: {player.GetPlayerHand().GetTotal()} ═══");
                Console.ResetColor();

                if (player.GetPlayerHand().GetTotal() > 21)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n💥 BUST! You went over 21!");
                    Console.ResetColor();
                    Thread.Sleep(2000);
                    return true;
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n[H]it or [S]tand? ");
                Console.ResetColor();
                string choice = Console.ReadLine().ToLower();

                if (choice == "h" || choice == "hit")
                {
                    player.DrawCard(deck);
                }
                else if (choice == "s" || choice == "stand")
                {
                    return false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid choice! Press any key to continue...");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            }
        }

        static bool DealerTurn(Computer_Dealer dealer, Deck deck, GraphicsPack graphics)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n═══════════════════════ DEALER'S TURN ═══════════════════════\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Dealer reveals hidden card...\n");
            Console.ResetColor();
            Thread.Sleep(1500);

            while (dealer.GetComputerHand().GetTotal() < 17)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Dealer has {dealer.GetComputerHand().GetTotal()}. Dealer must hit...\n");
                Console.ResetColor();
                dealer.DrawCard(deck);
                Thread.Sleep(1500);
            }

            if (dealer.GetComputerHand().GetTotal() > 21)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("💥 Dealer BUSTS!");
                Console.ResetColor();
                Thread.Sleep(2000);
                return true;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Dealer stands at {dealer.GetComputerHand().GetTotal()}");
            Console.ResetColor();
            Thread.Sleep(2000);
            return false;
        }

        static void DisplayFinalHands(Player player, Computer_Dealer dealer, GraphicsPack graphics)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n═══════════════════════ FINAL HANDS ═══════════════════════\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("DEALER'S FINAL HAND:");
            Console.ResetColor();
            foreach (Cards card in dealer.GetComputerHand().GetCards())
            {
                if (card != null)
                {
                    graphics.PrintCard(card);
                    Console.WriteLine();
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"═══ Dealer Total: {dealer.GetComputerHand().GetTotal()} ═══");
            Console.ResetColor();

            Console.WriteLine("\n");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("YOUR FINAL HAND:");
            Console.ResetColor();
            foreach (Cards card in player.GetPlayerHand().GetCards())
            {
                if (card != null)
                {
                    graphics.PrintCard(card);
                    Console.WriteLine();
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"═══ Your Total: {player.GetPlayerHand().GetTotal()} ═══");
            Console.ResetColor();
        }

        static void DetermineWinner(Player player, Computer_Dealer dealer, int bet, bool playerBusted, bool dealerBusted)
        {
            Console.WriteLine("\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("═══════════════════════ RESULT ═══════════════════════");
            Console.ResetColor();

            int playerTotal = player.GetPlayerHand().GetTotal();
            int dealerTotal = dealer.GetComputerHand().GetTotal();

            if (playerBusted)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n💔 YOU BUSTED! Dealer wins!");
                Console.WriteLine($"You lost ${bet}");
                Console.ResetColor();
                player.LoseBet(bet);
            }
            else if (dealerBusted)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n🎉 DEALER BUSTED! You win!");
                Console.WriteLine($"You won ${bet}!");
                Console.ResetColor();
                player.WinBet(bet);
            }
            else if (playerTotal > dealerTotal)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n🎉 YOU WIN!");
                Console.WriteLine($"You won ${bet}!");
                Console.ResetColor();
                player.WinBet(bet);
            }
            else if (playerTotal < dealerTotal)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n💔 DEALER WINS!");
                Console.WriteLine($"You lost ${bet}");
                Console.ResetColor();
                player.LoseBet(bet);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n🤝 IT'S A PUSH (TIE)!");
                Console.WriteLine("Your bet is returned");
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nNew Balance: ${player.GetMoneyBalance()}");
            Console.ResetColor();
        }
    }
}