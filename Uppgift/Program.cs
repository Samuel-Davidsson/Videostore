using System;
using System.Collections.Generic;
using System.Linq;
using Uppgift.Entites;

namespace Uppgift
{
    class Program
    {
        private static bool keepRunning = true;
        private static bool IsMember = false;
        private static double dvdCost = 29;
        private static double dvdDiscount = 0.10;
        private static double blueRayCost = 39;
        private static double blueRayDiscount = 0.15;
        private static double fixedCost = 100;
        private static double totalCost = 0;
        private static List<Customer> customerList = new List<Customer>(); 

        static void Main(string[] args)
        {
            var program = new Program();
            while (keepRunning)
            {
                PrintMenu();
                char command = GetCommand();
                ExcuteCommand(command);
            }
        }

        private static void ExcuteCommand(char command)
        {
            switch (command)
            {
                case '1':
                    AddCustomer();
                    break;
                case '2':
                    ShowTotalCostByName();
                    break;
                case '3':
                    ShowTotalCostForCustomers();              
                    break;
                case '4':
                    keepRunning = false;
                    break;
                default:
                    Console.WriteLine("---------------------------");
                    Console.WriteLine("Unknown command");
                    Console.WriteLine("---------------------------");
                    break;
            }
        }

        private static char GetCommand()
        {
            var command = char.ToLower(Console.ReadKey().KeyChar);
            Console.WriteLine();
            return command;
        }

        private static void PrintMenu()
        {
            Console.WriteLine("------------------------------------------------------------------------------------------------------");
            Console.WriteLine("1='New Customer', 2='Get totalcost for a specific customer', 3='Totalcost for all customers', 4='Quit'");
            Console.WriteLine("------------------------------------------------------------------------------------------------------");
        }

        private static void ShowTotalCostByName()
        {
            Console.WriteLine("Type the name of the customer example: Samuel Davidsson");
            var name = Console.ReadLine();
            string[] fullName = name.Split();

            foreach (var customer in customerList)
            {
                if (customer.FirstName == fullName[0] && customer.LastName == fullName[1])
                {
                    double totalCost = CalculateTotalCost(customer);
                    Console.WriteLine($"Totalcost for {customer.FirstName} {customer.LastName} is {totalCost}");
                }
            }
        }

        private static void ShowTotalCostForCustomers()
        {
            foreach (var customer in customerList)
            {
                double totalCost = CalculateTotalCost(customer);
                Console.WriteLine($"Totalcost for {customer.FirstName} {customer.LastName} is {totalCost}");
            }
        }

        private static void AddCustomer()
        {
            Console.WriteLine("Enter Firstname");
            var firstName = Console.ReadLine();
            Console.WriteLine("Enter Lastname");
            var lastName = Console.ReadLine();
            Console.WriteLine("Are the customer a member y/n?");
            char c = Console.ReadKey().KeyChar;
            Console.WriteLine();

            if (IsMember = (char.ToLower(c) == 'y'))
            {
                IsMember = true;
            }

            var customer = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                IsMember = IsMember
            };

            customerList.Add(customer);

            customer.Movies = AddMoviesToCustomer();
           
            double totalCost = CalculateTotalCost(customer);

            Console.WriteLine($"Totalcost for {customer.FirstName} {customer.LastName} is {totalCost}");
        }

        private static List<Movie> AddMoviesToCustomer()
        {
            Console.WriteLine("How many movies of type DVD do you wanna rent?");
            var dvdCountFromUser = Console.ReadLine();
            while (dvdCountFromUser == string.Empty)
            {
                Console.WriteLine("Please enter a number");
                dvdCountFromUser = Console.ReadLine();
            }

            Console.WriteLine("How many movies of type BLUERAY do you wanna rent?");
            var blueRayCountFromUser = Console.ReadLine();
            while (blueRayCountFromUser == string.Empty)
            {
                Console.WriteLine("Please enter a number");
                blueRayCountFromUser = Console.ReadLine();
            }

            var movies = new List<Movie>();
            int dvdCount = Convert.ToInt32(dvdCountFromUser);

            for (int i = 1; i <= dvdCount; i++)
            {
                var movie = new Movie
                {
                    GuidId = Guid.NewGuid().ToString(),
                    MovieType = MovieType.DVD,
                    Price = dvdCost
                };
                movies.Add(movie);
            }

            int blueRayCount = Convert.ToInt32(blueRayCountFromUser);

            for (int i = 1; i <= blueRayCount; i++)
            {
                var movie = new Movie
                {
                    GuidId = Guid.NewGuid().ToString(),
                    MovieType = MovieType.BlueRay,
                    Price = blueRayCost
                };
                movies.Add(movie);
            }
            return movies;

        }

        private static double CalculateTotalCost(Customer customer)
        {
            double totalCost;

            if (customer.IsMember == true && customer.Movies.Count >= 4)
            {
                totalCost = DiscountForRentingFourOrMoreMovies(customer);
            }
            else if (customer.IsMember == true)
            {
                totalCost = CalculateTotalCostForPatronCustomer(customer);
            }
            else
            {
                totalCost = CalculateTotalCostRegularCustomer(customer);
            }
            return totalCost;
        }

        private static double DiscountForRentingFourOrMoreMovies(Customer customer)
        {
            var movies = customer.Movies.OrderByDescending(x => x.MovieType);
            Movie[] arrayOfMovies = movies.ToArray();

            for (int i = 0; i < arrayOfMovies.Length; i++)
            {
                if (i < 4)
                {
                    continue;
                }
                else if (customer.IsMember == true && arrayOfMovies[i].MovieType == MovieType.DVD)
                {
                    var memberDiscount = dvdDiscount * dvdCost;
                    totalCost += dvdCost - memberDiscount;
                }
                else if (customer.IsMember == true && arrayOfMovies[i].MovieType == MovieType.BlueRay)
                {
                    var memberDiscount = blueRayDiscount * blueRayCost;
                    totalCost += blueRayCost - memberDiscount;
                }
            }
            var totalCostWithFixedCost = totalCost + fixedCost;
            return Math.Round(totalCostWithFixedCost);
        }

        private static double CalculateTotalCostForPatronCustomer(Customer customer)
        {
            foreach (var movie in customer.Movies)
            {
                if (customer.IsMember == true && movie.MovieType == MovieType.DVD)
                {
                    var memberDiscount = dvdDiscount * dvdCost;
                    totalCost += dvdCost - memberDiscount;
                }
                else if (customer.IsMember == true && movie.MovieType == MovieType.BlueRay)
                {
                    var memberDiscount = blueRayDiscount * blueRayCost;
                    totalCost += blueRayCost - memberDiscount;
                }
            }
            return totalCost;
        }

        private static double CalculateTotalCostRegularCustomer(Customer customer)
        {
            foreach (var movie in customer.Movies)
            {
                if (customer.IsMember == false && movie.MovieType == MovieType.DVD)
                {
                    totalCost += dvdCost;
                }
                else if (customer.IsMember == false && movie.MovieType == MovieType.BlueRay)
                {
                    totalCost += blueRayCost;
                }
            }
            return Math.Round(totalCost);
        }
    }
}
