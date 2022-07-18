using BlazorApp1.Client.Repository;
using Microsoft.AspNetCore.Components;
using static BlazorApp1.Client.Shared.MainLayout;

namespace BlazorApp1.Client.Pages
{
    public partial class Index
    {
        [Inject] IRepository Repository { get; set; }
        [CascadingParameter] AppState appState { get; set; }

        private Dictionary<string, object> parametersArbitraries = new Dictionary<string, object>()
        {
            {"placeholder", "nuevo placehodler"},
            { "disabled", "true"}
        };

        void Add(Film film)
        {
            Repository.Add(film);
        }

        public List<Film> getFilms()
        {
            return Repository.GetFilms();
        }

    }
}
