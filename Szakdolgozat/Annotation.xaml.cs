namespace Szakdolgozat;

public partial class Annotation : ContentPage
{
    private static string ThisPage = "Annotation";
    ProjectList datas;
    public Annotation()
	{
		InitializeComponent();
	}

    public Annotation(ProjectList _datas)
    {
        InitializeComponent();
        datas = _datas;
    }

    //  Menüben a Project gombra kattintáskor ez fut le
    private async void ProjectClicked(object sender, EventArgs e)
    {
        if (ThisPage == "Project")
        {
            return;
        }
        await Navigation.PushModalAsync(new MainPage(datas));
    }

    //  Menüben a Google Maps gombra kattintáskor ez fut le
    private async void GoogleMapsClicked(object sender, EventArgs e)
    {
        if (ThisPage == "GoogleMaps")
        {
            return;
        }
        await Navigation.PushModalAsync(new GoogleMaps(datas));
    }

    //  Menüben az Urbanization Score gombra kattintáskor ez fut le
    private async void UrbanizationScoreClicked(object sender, EventArgs e)
    {
        if (ThisPage == "UrbanizationScore")
        {
            return;
        }
        await Navigation.PushModalAsync(new UrbanizationScore(datas));
    }

    //  Menüben az Annotation gombra kattintáskor ez fut le
    private async void AnnotationClicked(object sender, EventArgs e)
    {
        if (ThisPage == "Annotation")
        {
            return;
        }
        await Navigation.PushModalAsync(new Annotation(datas));
    }

    //  Menüben az Import / Export gombra kattintáskor ez fut le
    private async void ImportExportClicked(object sender, EventArgs e)
    {
        if (ThisPage == "ImportExport")
        {
            return;
        }
        await Navigation.PushModalAsync(new ImportExport(datas));
    }

    //  Show Labels gombra kattintva ez fut le
    private void ShowLabelsClicked(object sender, EventArgs e)
    {
        //  Labelek megjelenítése
    }

    //  Show Labels gombra kattintva ez fut le
    private void RecalculateIndicesClicked(object sender, EventArgs e)
    {
        //  Az index számító interface meghívása
    }
}