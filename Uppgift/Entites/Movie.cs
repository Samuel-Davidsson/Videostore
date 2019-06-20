using System;

namespace Uppgift.Entites
{
    public class Movie
    {
        public virtual string GuidId { get; set; }
        public virtual MovieType MovieType { get; set; }
        public virtual double Price { get; set; }
        public virtual double Discount { get; set; }


        public virtual double CalculatePatronPrice(double Discount, double Price)
        {
            return Discount * Price;
        }
    }

    public enum MovieType
    {
        DVD,
        BlueRay,
        SuperBlueRay
    }
}