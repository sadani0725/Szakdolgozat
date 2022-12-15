namespace Szakdolgozat;

public partial class ImportExport : ContentPage
{
    private static string ThisPage = "ImportExport";
    public ImportExport()
	{
		InitializeComponent();
	}

    private async void ProjectClicked(object sender, EventArgs e)
    {
        if (ThisPage == "Project")
        {
            return;
        }
        await Navigation.PushAsync(new MainPage());
    }

    private async void GoogleMapsClicked(object sender, EventArgs e)
    {
        if (ThisPage == "GoogleMaps")
        {
            return;
        }
        await Navigation.PushAsync(new GoogleMaps());
    }

    private async void UrbanizationScoreClicked(object sender, EventArgs e)
    {
        if (ThisPage == "UrbanizationScore")
        {
            return;
        }
        await Navigation.PushAsync(new UrbanizationScore());
    }

    private async void AnnotationClicked(object sender, EventArgs e)
    {
        if (ThisPage == "Annotation")
        {
            return;
        }
        await Navigation.PushAsync(new Annotation());
    }

    private async void ImportExportClicked(object sender, EventArgs e)
    {
        if (ThisPage == "ImportExport")
        {
            return;
        }
        await Navigation.PushAsync(new ImportExport());
    }
}