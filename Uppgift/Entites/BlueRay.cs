namespace Uppgift.Entites
{
    public class BlueRay : Movie
    {
        public override MovieType MovieType
        {
            get
            {
                return MovieType.BlueRay;
            }
        }

        public override double Price
        {
            get
            {
                return 39;
            }
        }

        public override double Discount
        {
            get
            {
                return 0.15;
            }
        }
    }
}
