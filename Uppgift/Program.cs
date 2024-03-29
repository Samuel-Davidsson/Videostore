﻿using System;
using System.Collections.Generic;
using System.Linq;
using Uppgift.Entites;

namespace Uppgift
{
    class Program
    {
        private static bool keepRunning = true;
        private static bool IsMember = false;
        private static int specialRentDealForPatron = 0;
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
            while (fullName.Length <= 1)
            {
                Console.WriteLine("Please type first and last name again: Samuel Davidsson");
                name = Console.ReadLine();
                fullName = name.Split();
            }

            foreach (var customer in customerList)
            {
                if (customer.FirstName.ToLower() == fullName[0].ToLower() && customer.LastName.ToLower() == fullName[1].ToLower())
                {
                    Console.WriteLine($"Totalcost for {customer.FirstName} {customer.LastName} is {customer.TotalCost}");
                }
                else
                {
                    Console.WriteLine($"A customer with name {fullName[0]} {fullName[1]} does not exist");
                }
            }
        }

        private static void ShowTotalCostForCustomers()
        {
            foreach (var customer in customerList)
            {
                Console.WriteLine($"Totalcost for {customer.FirstName} {customer.LastName} is {customer.TotalCost}");
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
                specialRentDealForPatron = 4;
            }

            var customer = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                IsMember = IsMember,
                PatronFixedCostFourMovies = specialRentDealForPatron
            };

            customerList.Add(customer);

            customer.Movies = AddMoviesToCustomer(customer);

            customer.TotalCost = CalculateTotalCost(customer);

            Console.WriteLine($"Totalcost for {customer.FirstName} {customer.LastName} is {customer.TotalCost}");
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

            if (customer.IsMember == true && customer.Movies.Count >= 4)
            {
                customer.TotalCost = DiscountForRentingFourOrMoreMoviesAsPatron(customer);
            }
            else if (customer.IsMember == true)
            {
                customer.TotalCost = CalculateTotalCostForPatronCustomer(customer);
            }
            else
            {
                customer.TotalCost = CalculateTotalCostRegularCustomer(customer);
            }
            return (Math.Round(customer.TotalCost));
        }

        private static double DiscountForRentingFourOrMoreMoviesAsPatron(Customer customer)
        {
            var movies = customer.Movies.OrderByDescending(x => x.MovieType);
            Movie[] arrayOfMovies = movies.ToArray();

            for (int i = 0; i < arrayOfMovies.Length; i++)
            {
                if (i < specialRentDealForPatron)
                {
                    continue;
                }
                else
                {
                    var memberDiscount = arrayOfMovies[i].CalculatePatronPrice(arrayOfMovies[i].Discount, arrayOfMovies[i].Price);
                    customer.TotalCost += arrayOfMovies[i].Price - memberDiscount;
                }
            }
            var totalCostWithFixedCost = customer.TotalCost + fixedCost;
            return totalCostWithFixedCost;
        }

        private static double CalculateTotalCostForPatronCustomer(Customer customer)
        {
            foreach (var movie in customer.Movies)
            {
                var memberDiscount = movie.CalculatePatronPrice(movie.Discount, movie.Price);
                customer.TotalCost += movie.Price - memberDiscount;
            } 
            return customer.TotalCost;
        }

        private static double CalculateTotalCostRegularCustomer(Customer customer)
        {
            foreach (var movie in customer.Movies)
            {
                customer.TotalCost += movie.Price;
            }
            return customer.TotalCost;
        }
    }
}
