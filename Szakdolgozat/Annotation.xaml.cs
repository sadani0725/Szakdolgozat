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

    //  Men�ben a Project gombra kattint�skor ez fut le
    private async void ProjectClicked(object sender, EventArgs e)
    {
        if (ThisPage == "Project")
        {
            return;
        }
        await Navigation.PushModalAsync(new MainPage(datas));
    }

    //  Men�ben a Google Maps gombra kattint�skor ez fut le
    private async void GoogleMapsClicked(object sender, EventArgs e)
    {
        if (ThisPage == "GoogleMaps")
        {
            return;
        }
        await Navigation.PushModalAsync(new GoogleMaps(datas));
    }

    //  Men�ben az Urbanization Score gombra kattint�skor ez fut le
    private async void UrbanizationScoreClicked(object sender, EventArgs e)
    {
        if (ThisPage == "UrbanizationScore")
        {
            return;
        }
        await Navigation.PushModalAsync(new UrbanizationScore(datas));
    }

    //  Men�ben az Annotation gombra kattint�skor ez fut le
    private async void AnnotationClicked(object sender, EventArgs e)
    {
        if (ThisPage == "Annotation")
        {
            return;
        }
        await Navigation.PushModalAsync(new Annotation(datas));
    }

    //  Men�ben az Import / Export gombra kattint�skor ez fut le
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
        //  Labelek megjelen�t�se
    }

    //  Show Labels gombra kattintva ez fut le
    private void RecalculateIndicesClicked(object sender, EventArgs e)
    {
        //  Az index sz�m�t� interface megh�v�sa
    }
}