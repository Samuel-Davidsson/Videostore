using System;
using System.Collections.Generic;
using System.Text;

namespace Uppgift.Entites
{
    public class Dvd : Movie
    {
        public override MovieType MovieType
        {
            get
            {
                return MovieType.DVD;
            }
        }

        public override double Price
        {
            get
            {
                return 29;
            }
        }

        public override double Discount
        {
            get
            {
                return 0.10;
            }
        }

        public override double CalculatePatronPrice(double Discount, double Price)
        {
            return Discount * Price;
        }
    }
}


