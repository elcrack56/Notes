using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Notes.ViewModels;

internal class AboutViewModelDL
{
    public string TituloDL => AppInfo.Name;
    public string VersionDL => AppInfo.VersionString;
    public string MasInfoUrlDL => "https://aka.ms/maui";
    public string MensajeDL => "Esta aplicación está escrita en XAML y C# con .NET MAUI.";
    public ICommand ShowMoreInfoCommandDL { get; }

    public AboutViewModelDL()
    {
        ShowMoreInfoCommandDL = new AsyncRelayCommand(ShowMoreInfoDL);
    }

    async Task ShowMoreInfoDL() =>
        await Launcher.Default.OpenAsync(MasInfoUrlDL);
}