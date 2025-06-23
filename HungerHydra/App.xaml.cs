using HungerHydra.Abstractions;

namespace HungerHydra
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            if (Current != null) Current.UserAppTheme = AppTheme.Light;
        }

        protected override void OnSleep()
        {
            try
            {
                //Workaround: call OnDisappearing of the page after hide/resume app
                if (Shell.Current.Navigation?.NavigationStack?.Last() is BasePage lastPage)
                    lastPage.InvokeDisappearing();
            }
            catch (Exception)
            {
                //ignore
            }
        }
    }
}