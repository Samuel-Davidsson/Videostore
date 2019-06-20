namespace Uppgift.Entites
{
    public class Movie
    {
        public string GuidId { get; set; }
        public MovieType MovieType { get; set; }
        public double Price { get; set; }
    }
    public enum MovieType
    {
        DVD,
        BlueRay
    }
}