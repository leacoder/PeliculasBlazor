namespace BlazorApp1.Client.Repository
{
    public class Repository : IRepository
    {
        public List<Film> Films { get; set; }
        public void Add(Film film)
        {
            Films.Add(film);
        }

        public List<Film> GetFilms()
        {
            Films = new List<Film>()
            {
                new Film() { Title = "Spiderman: Far from home", DateLaunched = new DateTime(2019,7,2)},
                new Film() { Title = "Moana", DateLaunched = new DateTime(2011,3,2)},
                new Film() { Title = "Inception", DateLaunched = new DateTime(2022,7,2)}


            };
            return Films;
        }
    }
}
