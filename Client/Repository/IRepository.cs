namespace BlazorApp1.Client.Repository
{
    public interface IRepository
    {
        List<Film> GetFilms();

        void Add(Film film);

    }
}
