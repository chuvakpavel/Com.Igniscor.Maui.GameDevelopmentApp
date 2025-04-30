using System.Windows.Input;
using HungerHydra.Models;

namespace HungerHydra.Views.Support;

public partial class NavigationButtonsView : ContentView
{
    #region Bindable Properties

    #region ItemsSource Property

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
        nameof(ItemsSource),
        typeof(IEnumerable<NavigationButtonItem>),
        typeof(NavigationButtonsView));

    public IEnumerable<NavigationButtonItem>? ItemsSource
    {
        get => (IEnumerable<NavigationButtonItem>?)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    #endregion ItemsSource Property

    #region Command Property

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command),
        typeof(ICommand),
        typeof(NavigationButtonsView));

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    #endregion Command Property

    #endregion Bindable Properties

    public NavigationButtonsView()
	{
		InitializeComponent();
	}
}