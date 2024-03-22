using System;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace MyApp
{
    class Game
    {
        static public int currentCard = 0; // Variable is for controlling the array that hold the player's cards
        static public int currentDealerCard = 0; // Variable is for controlling the array that adds cards to the dealer's hand
        static public int cardsLeft = 52; // Variable adjusts to the amount of cards left in the deck
        static int blackJack = 21; // Simple variable, used to just eliminate magic numbers
        static public int[] hand = new int[5]; // Player's hand
        static public int[] dealerHand = new int[9]; // Dealer's hand
        static List<int> deck = new List<int>(); // Deck of cards
        public static void CreateDeck()
        {
            for (int cardValue = 2; cardValue <= 11; cardValue++)
            {
                deck.Add(cardValue);
                deck.Add(cardValue);
                deck.Add(cardValue);
                deck.Add(cardValue);

                if (cardValue == 10) // For adding 12 extra tens to the deck (jacks, queens, and kings)
                {
                    for (int j = 0; j <= 11; j++)
                    {
                        deck.Add(cardValue);
                    }
                }     
            }          
        }

        //-----------------------------------------------------------

        public static void ShuffleDeck()
        {
            deck.Clear();
            CreateDeck();
        }

        //-----------------------------------------------------------

        public static void Call()
        {              
            Console.WriteLine("The dealer will now draw cards.\n-------------------------");

            while (dealerHand.Sum() <= hand.Sum())
            {
                Hit(true);
            }

            if (dealerHand.Sum() > hand.Sum() && dealerHand.Sum() <= blackJack)
            {
                Console.WriteLine("GAME OVER. Dealer is closer.");
            }
            else
            {
                Console.WriteLine("YOU WIN! Dealer busts.");
            }
        }

        //-----------------------------------------------------------

        public static void Hit(bool dealerOrPlayer) // False is the player, true is the dealer
        {
            Random rng = new Random(); // Spooky scary randomness
            Thread.Sleep(2000);

            int randomCard = rng.Next(0, cardsLeft); // Draws a random card

            if (dealerOrPlayer == false) // Player draw card
            {
                hand[currentCard] = deck[randomCard]; // Adds the randomly drawn card to your hand

                if (hand.Sum() > blackJack && randomCard == 11)
                {
                    currentCard = 1; // All aces are 11 by default, but if one causes you to bust, its value is set to 1 instead
                }

                Console.WriteLine(hand[currentCard] + "\nCurrent total - " + hand.Sum());
                currentCard++;

                if (hand.Sum() == blackJack)
                {
                    Console.WriteLine("YOU WIN! Your total is exactly 21!");
                    Program.gameActive = false;
                }
                else if (hand.Sum() > blackJack)
                {
                    Console.WriteLine("GAME OVER. You busted.");
                    Program.gameActive = false;
                }
                else if (hand[4] > 0 && hand.Sum() <= blackJack)
                {
                    Console.WriteLine("YOU WIN! You managed to draw five cards without busting!");
                    Program.gameActive = false;
                }
            }
            else if (dealerOrPlayer == true) // Dealer draw card
            {
                dealerHand[currentDealerCard] = deck[randomCard]; // Adds the randomly drawn card to the dealer's hand

                if (dealerHand.Sum() > blackJack && randomCard == 11)
                {
                    currentDealerCard = 1; // All aces are 11 by default, but if one causes the dealer to bust, its value is set to 1 instead
                }
                
                Console.WriteLine(dealerHand[currentDealerCard] + "\nCurrent dealer total - " + dealerHand.Sum());
                currentDealerCard++;
            }

            deck.Remove(randomCard);
            cardsLeft--;
        }
    }

    //-----------------------------------------------------------

    internal class Program
    {
        static bool validChoiceMade; // For testing if the player has made a valid choice using the Console.ReadLines
        static public bool gameActive = true; // (/^.^)/ ~ _|__|_
        static string playerChoice = "hello there handsome";
        static void Main(string[] args)
        {
            StartGame();

            while (gameActive == true)
            {
                Choice();

                if (playerChoice == "hit")
                {
                    Game.Hit(false);
                }
                else if (playerChoice == "call")
                {
                    gameActive = false;
                    Game.Call();
                }

                if (gameActive == false)
                {
                    PlayAgain();
                }
            }

            Console.WriteLine("-------------------------\nPress any key to close the window.");
            Console.ReadKey();
        }

        //-----------------------------------------------------------

        static void PlayAgain()
        {
            validChoiceMade = false;
            Console.WriteLine("-------------------------\nWould you like to play again? YES or NO?");
            
            while (validChoiceMade == false)
            {
                playerChoice = Console.ReadLine().ToLower(); // This had better not return null, looking at you Jeremy

                if (playerChoice == "yes" || playerChoice == "no")
                {
                    validChoiceMade = true;
                }
                else
                {
                    Console.WriteLine("Try again.");
                }
            }

            if (playerChoice == "yes")
            {
                Game.ShuffleDeck();
                Game.cardsLeft = 52;
                Game.currentCard = 0;
                Game.currentDealerCard = 0;
                Array.Clear(Game.hand, 0, Game.hand.Length);
                Array.Clear(Game.dealerHand, 0, Game.dealerHand.Length);
                
                Console.WriteLine("You will now be dealt two cards.\n-------------------------");

                Game.Hit(false);
                Game.Hit(false);

                gameActive = true;
            }
        }

        //-----------------------------------------------------------

        public static void Choice() // Gets the player's choice
        {
            validChoiceMade = false;

            Console.WriteLine("HIT or CALL?");

            while (validChoiceMade == false)
            {
                playerChoice = Console.ReadLine().ToLower(); // This one had also better not return null. His fault -> Jeremy

                if (playerChoice == "call" || playerChoice == "hit")
                {
                    validChoiceMade = true;
                }
                else
                {
                    Console.WriteLine("Try again.");
                }
            }
        }

        //-----------------------------------------------------------

        static void StartGame()
        {
            Console.WriteLine("Welcome to TERMINAL-JACK!\n-------------------------\nPress any key to continue.");
            Console.ReadKey();
            Console.WriteLine("\n-------------------------\nYou will now be dealt two cards.");
            Game.CreateDeck();
            Game.Hit(false);
            Game.Hit(false);
        }
    }
}