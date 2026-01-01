using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BlackJack_Game
{
    internal class Program
    {
        // Entry point - One Call Method
        static void Main(string[] args)
        {       
            StartSystem();
        }

        //  Main game loop - runs the entire blackjack game
        static void StartSystem()
        {
            Console.OutputEncoding = Encoding.UTF8; //  Enable UTF-8 for card symbols

            GraphicsPack graphics = new GraphicsPack();
            Player player = new Player();

            // Show welcome screen and rules
            graphics.PrintWelcomeScreen(player.GetMoneyBalance(), player.GetMinimumBet());
            graphics.PrintInstructions();

            bool keepPlaying = true;

            // Keep playing rounds until player quits or runs out of money
            while (keepPlaying && player.GetMoneyBalance() > 0)
            {
                //  Check if player has money
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

                //  Display current balance
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n╔════════════════════════════════════════╗");
                Console.WriteLine($"║   Current Balance: ${player.GetMoneyBalance()}");
                Console.WriteLine("╚════════════════════════════════════════╝");
                Console.ResetColor();

                //  Get bet amount from player
                int bet = GetBetAmount(player, player.GetMinimumBet());

                if (bet == 0)
                {
                    break; // Player chose to quit
                }

                //  Create new deck and dealer for this round
                Deck deck = new Deck();
                Computer_Dealer dealer = new Computer_Dealer();

                //  Reset hands for new round
                player.ResetHand();
                dealer.ResetHand();

                //  Initial deal - 2 cards each
                player.DrawCard(deck);
                dealer.DrawCard(deck);
                player.DrawCard(deck);
                dealer.DrawCard(deck);

                //  Show initial hands (with animation!)
                DisplayInitialHands(player, dealer, graphics);

                // Player's turn
                bool playerBusted = PlayerTurn(player, dealer, deck, graphics);

                // Dealer's turn (only if player didn't bust)
                bool dealerBusted = false;
                if (!playerBusted)
                {
                    dealerBusted = DealerTurn(dealer, deck, graphics);
                }

                //  Show final hands
                DisplayFinalHands(player, dealer, graphics);

                //  Determine winner and update balance
                DetermineWinner(player, dealer, bet, playerBusted, dealerBusted);

                //  Ask to play again
                if (player.GetMoneyBalance() > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("\n\nPlay another round? (y/n): ");
                    Console.ResetColor();
                    string choice = Console.ReadLine().ToLower();
                    keepPlaying = (choice == "y" || choice == "yes");
                }
                else
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }

            //  Game over screen
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

        //  Gets valid bet amount from player
        static int GetBetAmount(Player player, int minimumBet)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"\nEnter your bet amount (${minimumBet} - ${player.GetMoneyBalance()}) or 0 to quit: $");
       
                Console.ForegroundColor = ConsoleColor.Yellow;
                if (int.TryParse(Console.ReadLine(), out int bet))
                {
                    if (bet == 0)
                    {
                        return 0; // Player wants to quit
                    }
                    else if (bet >= minimumBet && bet <= player.GetMoneyBalance()) // Valid bet range
                    {
                        return bet; // Valid bet!
                    }
                    Console.ResetColor();
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

        //  Shows the initial deal (2 cards each, dealer's 2nd card hidden)
        static void DisplayInitialHands(Player player, Computer_Dealer dealer, GraphicsPack graphics)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n═══════════════════════ INITIAL DEAL ═══════════════════════\n");
            Console.ResetColor();

            //  Show dealer's cards (second card is hidden)
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("DEALER'S HAND:");
            Console.ResetColor();
            graphics.PrintCardsHorizontally(dealer.GetComputerHand().GetCards(), hideSecondCard: true, delayMs: 1500); // print with delay

            Console.WriteLine("\n");

            //  Show player's cards (all visible)
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("YOUR HAND:");
            Console.ResetColor();
            graphics.PrintCardsHorizontally(player.GetPlayerHand().GetCards(), hideSecondCard: false, delayMs: 1500); // print with delay

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n═══ Total: {player.GetPlayerHand().GetTotal()} ═══");
            Console.ResetColor();

            Thread.Sleep(1000); // Pause to let player see the cards
        }

        // 🎮 Player's turn - hit or stand
        static bool PlayerTurn(Player player, Computer_Dealer dealer, Deck deck, GraphicsPack graphics)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n═══════════════════════ YOUR TURN ═══════════════════════\n");
                Console.ResetColor();

                //  Show dealer's cards (second card still hidden)
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("DEALER'S HAND:");
                Console.ResetColor();
                graphics.PrintCardsHorizontally(dealer.GetComputerHand().GetCards(), hideSecondCard: true, delayMs: 0);

                Console.WriteLine("\n");

                //  Show player's cards
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("YOUR HAND:");
                Console.ResetColor();
                graphics.PrintCardsHorizontally(player.GetPlayerHand().GetCards(), hideSecondCard: false, delayMs: 0);

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"\n═══ Total: {player.GetPlayerHand().GetTotal()} ═══");
                Console.ResetColor();

                //  Check if player busted
                if (player.GetPlayerHand().GetTotal() > 21)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n💥 BUST! You went over 21!");
                    Console.ResetColor();
                    Thread.Sleep(4500);
                    return true; // Player busted
                }

                //  Ask player to hit or stand
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n[H]it or [S]tand? ");
                Console.ResetColor();
                string choice = Console.ReadLine().ToLower();

                if (choice == "h" || choice == "hit")
                {
                    player.DrawCard(deck); // Draw another card
                }
                else if (choice == "s" || choice == "stand")
                {
                    return false; // Player stands, didn't bust
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

        // Dealer's turn - must hit until 17 or higher
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

            // Dealer must hit until 17+
            while (dealer.GetComputerHand().GetTotal() < 17)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Dealer has {dealer.GetComputerHand().GetTotal()}. Dealer must hit...\n");
                Console.ResetColor();
                dealer.DrawCard(deck);
                Thread.Sleep(3000);
            }

            // Check if dealer busted
            if (dealer.GetComputerHand().GetTotal() > 21)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("💥 Dealer BUSTS!");
                Console.ResetColor();
                Thread.Sleep(3000);
                return true; // Dealer busted
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Dealer stands at {dealer.GetComputerHand().GetTotal()}");
            Console.ResetColor();
            Thread.Sleep(3500);
            return false; // Dealer didn't bust
        }

        // Shows final hands of both player and dealer
        static void DisplayFinalHands(Player player, Computer_Dealer dealer, GraphicsPack graphics)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n═══════════════════════ FINAL HANDS ═══════════════════════\n");
            Console.ResetColor();

            // Show dealer's final hand (all cards revealed)
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("DEALER'S FINAL HAND:");
            Console.ResetColor();
            graphics.PrintCardsHorizontally(dealer.GetComputerHand().GetCards(), hideSecondCard: false, delayMs: 2000);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n═══ Dealer Total: {dealer.GetComputerHand().GetTotal()} ═══");
            Console.ResetColor();

            Console.WriteLine("\n");

            // Show player's final hand
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("YOUR FINAL HAND:");
            Console.ResetColor();
            graphics.PrintCardsHorizontally(player.GetPlayerHand().GetCards(), hideSecondCard: false, delayMs: 2000);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n═══ Your Total: {player.GetPlayerHand().GetTotal()} ═══");
            Console.ResetColor();
        }

        // Determines winner and updates player's balance
        static void DetermineWinner(Player player, Computer_Dealer dealer, int bet, bool playerBusted, bool dealerBusted)
        {
            Console.WriteLine("\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("═══════════════════════ RESULT ═══════════════════════");
            Console.ResetColor();

            int playerTotal = player.GetPlayerHand().GetTotal();
            int dealerTotal = dealer.GetComputerHand().GetTotal();

            // Player busted
            if (playerBusted)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n💔 YOU BUSTED! Dealer wins!");
                Console.WriteLine($"You lost ${bet}");
                Console.ResetColor();
                player.LoseBet(bet);
            }
            // Dealer busted
            else if (dealerBusted)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n🎉 DEALER BUSTED! You win!");
                Console.WriteLine($"You won ${bet}!");
                Console.ResetColor();
                player.WinBet(bet);
            }
            // Player has higher total
            else if (playerTotal > dealerTotal)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n🎉 YOU WIN!");
                Console.WriteLine($"You won ${bet}!");
                Console.ResetColor();
                player.WinBet(bet);
            }
            // Dealer has higher total
            else if (playerTotal < dealerTotal)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n💔 DEALER WINS!");
                Console.WriteLine($"You lost ${bet}");
                Console.ResetColor();
                player.LoseBet(bet);
            }
            // Tie (Push)
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n🤝 IT'S A PUSH (TIE)!");
                Console.WriteLine("Your bet is returned");
                Console.ResetColor();
            }

            // Show new balance
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nNew Balance: ${player.GetMoneyBalance()}");
            Console.ResetColor();

         
        }
    }
}
