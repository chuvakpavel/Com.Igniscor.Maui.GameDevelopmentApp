namespace HungerHydra.Abstractions;

public abstract class BasePage : ContentPage
{
    public void InvokeAppearing()
    {
        OnAppearing();
    }

    public void InvokeDisappearing()
    {
        OnDisappearing();
    }
}