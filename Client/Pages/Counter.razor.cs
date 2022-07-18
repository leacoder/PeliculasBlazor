using Microsoft.AspNetCore.Components;
using static BlazorApp1.Client.Shared.MainLayout;

namespace BlazorApp1.Client.Pages
{
    public partial class Counter
    {
        [CascadingParameter] protected AppState appState { get; set; }

        private int currentCount = 0;

        private void IncrementCount()
        {
            currentCount++;
            singletonService.Value++;
        }
    }
}
