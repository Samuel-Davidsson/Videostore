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
        private static double fixedCost = 100;
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

            customer.Movies = AddMoviesToCustomer(customer);
           
            double totalCost = CalculateTotalCost(customer);

            Console.WriteLine($"Totalcost for {customer.FirstName} {customer.LastName} is {totalCost}");
        }

        private static ICollection<Movie> AddMoviesToCustomer(Customer customer)
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

            int dvdCount = Convert.ToInt32(dvdCountFromUser);

            AddDVDMoviesToList(dvdCount, customer.Movies);

            int blueRayCount = Convert.ToInt32(blueRayCountFromUser);

            AddBlueRayMoviesToList(blueRayCount, customer.Movies);

            return customer.Movies;

        }

        private static void AddDVDMoviesToList(int dvdCount, ICollection<Movie> movies)
        {
            for (int i = 1; i <= dvdCount; i++)
            {
                var movie = new Dvd
                {
                    GuidId = Guid.NewGuid().ToString()
                };
                movies.Add(movie);
            }
        }

        private static void AddBlueRayMoviesToList(int blueRayCount, ICollection<Movie> movies)
        {
            for (int i = 1; i <= blueRayCount; i++)
            {
                var movie = new BlueRay
                {
                    GuidId = Guid.NewGuid().ToString()
                };
                movies.Add(movie);
            }
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
                else
                {
                    var memberDiscount = arrayOfMovies[i].Discount * arrayOfMovies[i].Price;
                    customer.TotalCost += arrayOfMovies[i].Price - memberDiscount;
                }
            }
            var totalCostWithFixedCost = customer.TotalCost + fixedCost;
            return Math.Round(totalCostWithFixedCost);
        }

        private static double CalculateTotalCostForPatronCustomer(Customer customer)
        {
            foreach (var movie in customer.Movies)
            {
                if (customer.IsMember == true && movie.MovieType == MovieType.DVD)
                {
                    var memberDiscount = movie.Discount * movie.Price;
                    customer.TotalCost += movie.Price - memberDiscount;
                }
                else if (customer.IsMember == true && movie.MovieType == MovieType.BlueRay)
                {
                    var memberDiscount = movie.Discount * movie.Price;
                    customer.TotalCost += movie.Price - memberDiscount;
                }
            }
            return customer.TotalCost;
        }

        private static double CalculateTotalCostRegularCustomer(Customer customer)
        {
            foreach (var movie in customer.Movies)
            {
                if (customer.IsMember == false && movie.MovieType == MovieType.DVD)
                {
                    customer.TotalCost += movie.Price;
                }
                else if (customer.IsMember == false && movie.MovieType == MovieType.BlueRay)
                {
                    customer.TotalCost += movie.Price;
                }
            }
            return Math.Round(customer.TotalCost);
        }
    }
}
