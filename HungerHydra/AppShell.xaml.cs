using HungerHydra.Views;

namespace HungerHydra
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(GamePage), typeof(GamePage));
        }
    }
}
