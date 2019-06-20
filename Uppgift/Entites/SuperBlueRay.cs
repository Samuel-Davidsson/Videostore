using System;
using System.Collections.Generic;
using System.Text;

namespace Uppgift.Entites
{
    public class SuperBlueRay : Movie
    {
        public override MovieType MovieType
        {
            get
            {
                return MovieType.SuperBlueRay;
            }
        }

        public override double Price
        {
            get
            {
                return 49;
            }
        }
    }
}
