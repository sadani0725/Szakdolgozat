namespace Szakdolgozat;

public partial class UrbanizationScore : ContentPage
{
    private static string ThisPage = "UrbanizationScore";
    string filename;    //  Projekt neve kiterjesztés nélkül
    string subfile;     //  Projekt neve kiterjesztéssel
    string pathString;  //  Elérési útvonal
    static string projectPathString = System.IO.Path.Combine(@"c:\UrbanizationProjects", "Projects");      //  Alapértelmezett mentési hely Windows rendszereken (C: meghajtó)
    ProjectList datas;  //  Projekt adatait tárolja (Név, Elérési út, Adatok)

    //  Inicializálás
    public UrbanizationScore()
	{
		InitializeComponent();
        MakeResponisve();

    }

    //  Inicializálás adatfogadással
    public UrbanizationScore(ProjectList _datas)
    {   
        InitializeComponent();
        if (_datas != null)
        {
            datas = _datas;
            filename = _datas.GetProjectName().Remove(_datas.GetProjectName().Length - 4);
            subfile = _datas.GetProjectName();
            pathString = _datas.GetPathString();
        }
        MakeResponisve();
    }

    //  FrontEnd értékéket módosít
    public void MakeResponisve()
    {
        BrowseIMG.HeightRequest = Application.Current.MainPage.Width / 2.5;
        BrowseIMG.WidthRequest = Application.Current.MainPage.Width / 2.5;
        ButtonListScroll.HeightRequest = Application.Current.MainPage.Height * 0.65;
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

    //  Calculate Scores gombra kattintva ez fut le
    private void CalculateScoresClicked(object sender, EventArgs e)
    {
        //  Az index számító interface meghívása
    }

    //  Show Results gombra kattintva ez fut le
    private void ShowResultsClicked(object sender, EventArgs e)
    {
        if (datas == null)
        {
            DisplayAlert("Error", "Open a Project!", "OK");
        }
        else
        {
            pathString = System.IO.Path.Combine(datas.GetPathString(), filename, "pcaResult.csv");

            if (!System.IO.File.Exists(pathString))
            {
                DisplayAlert("Error", "Calculate First!", "OK");
            }
            else
            {
                ImageBG.IsVisible = false;
                BrowseIMG.IsVisible = true;
                ButtonLayout.IsVisible = true;
                ButtonLayout.Clear();

                List<Button> ButtonList = new List<Button>();

                foreach (var item in datas.GetPCAList())
                {

                    Button b = new Button
                    {
                        Text = item.Num + "; " + item.PCAValue + "; " + item.Name,
                        BackgroundColor = Color.Parse("AliceBlue"),
                        TextColor = Color.Parse("Black")
                    };
                    b.Clicked += new EventHandler(this.ImageShowClicked);
                    ButtonList.Add(b);
                }

                List<Button> SortedButtonList = ButtonList.OrderBy(o => Convert.ToDouble(o.Text.Split(';')[1])).ToList();

                foreach (var item in SortedButtonList)
                {
                    ButtonLayout.Add(item);
                }
            }
        }
    }

    //  Az egyes képek megjelenítéséért felelõs gombok függvénye
    protected void ImageShowClicked(object sender, EventArgs e)
    {
        Button button = sender as Button;
        pathString = System.IO.Path.Combine(datas.GetPathString(), filename, button.Text.Split(";")[0], "01.bmp");
        BrowseIMG.Source = pathString;
    }
}