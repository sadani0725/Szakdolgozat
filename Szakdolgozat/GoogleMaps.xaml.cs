using System.Data;
using System.IO;

namespace Szakdolgozat;

public partial class GoogleMaps : ContentPage
{
    private static string ThisPage = "GoogleMaps";
    string filename;    //  Projekt neve kiterjesztés nélkül
    string subfile;     //  Projekt neve kiterjesztéssel
    string pathString;  //  Elérési útvonal
    static string apikeyPathString = System.IO.Path.Combine(@"c:\UrbanizationProjects", "ApiKey");         //  Alapértelmezett ApiKey hely Windows rendszereken (C: meghajtó)
    static string projectPathString = System.IO.Path.Combine(@"c:\UrbanizationProjects", "Projects");      //  Alapértelmezett mentési hely Windows rendszereken (C: meghajtó)
    string Apikey;      //  Google Api Key
    bool useProxySettings;  //  Menti, hogy szerenténk-e használni  Proxy beállításokat
    string address; //  Menti az IP címet
    string port;    //  Menti a portot
    ProjectList datas;  //  Projekt adatait tárolja (Név, Elérési út, Adatok)

    //  Inicializálás
    public GoogleMaps()
	{        
        InitializeComponent();
        MakeResponisve();
    }

    //  Inicializálás adatfogadással
    public GoogleMaps(ProjectList _datas)
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

    // Set Api Key gombra kattintva ez fut le
    private void SetApiKeyClicked(object sender, EventArgs e)
    {
        HelpButton.IsVisible = false;
        EditApiKey.IsVisible = true;
        SaveApiKeyButton.IsVisible = true;

        pathString = System.IO.Path.Combine(apikeyPathString, "apikey.txt");

        if (!System.IO.Directory.Exists(apikeyPathString))
        {
            System.IO.Directory.CreateDirectory(apikeyPathString);
            using (var fs = System.IO.File.CreateText(pathString))
            {

            }
        }
        else
        {
            StreamReader sr = new StreamReader(pathString);

            while (!sr.EndOfStream)
            {
                Apikey = sr.ReadLine();
            }

            if (Apikey != null)
            {
                EditApiKey.Text = Apikey.ToString();
            }

            sr.Close();

        }      
        
    }

    //  Save gombra kattintva ez fut le
    private void SaveNewApiKeyClicked(object sender, EventArgs e)
    {
        pathString = System.IO.Path.Combine(apikeyPathString, "apikey.txt");

        StreamWriter sw = new StreamWriter(pathString);

        sw.Write(EditApiKey.Text.ToString());
        sw.Close();

        HelpButton.IsVisible = true;
        EditApiKey.IsVisible = false;
        SaveApiKeyButton.IsVisible = false;
    }

    //  Visszaadja a Google Api Key-t
    public string GetApiKey() 
    {
        pathString = System.IO.Path.Combine(apikeyPathString, "apikey.txt");

        StreamReader sr = new StreamReader(pathString);

        while (!sr.EndOfStream)
        {
            Apikey = sr.ReadLine();
        }

        sr.Close();

        if (Apikey != null)
        {
            return Apikey;
        }
        else
        {
            return "Empty";
        }
            
    }

    // Download gombra kattintva ez fut le
    private void DownloadClicked(object sender, EventArgs e)
    {
        ImageBG.IsVisible = false;
        BrowseIMG.IsVisible = false;
        ButtonLayout.IsVisible = false;
        DownloadLayout.IsVisible = true;

        int allcount = 0;

        foreach (var item in datas.GetProjectList())
        {
            int count = 0;

            pathString = Path.Combine(datas.GetPathString(), datas.GetProjectName().Remove(datas.GetProjectName().Length - 4), item.Num.ToString(), "01.bmp");
            if (File.Exists(pathString))
            {
                count++;
            }

            pathString = Path.Combine(datas.GetPathString(), datas.GetProjectName().Remove(datas.GetProjectName().Length - 4), item.Num.ToString(), "02.bmp");
            if (File.Exists(pathString))
            {
                count++;
            }

            pathString = Path.Combine(datas.GetPathString(), datas.GetProjectName().Remove(datas.GetProjectName().Length - 4), item.Num.ToString(), "04.bmp");
            if (File.Exists(pathString))
            {
                count++;
            }

            string s = "";
            if (count == 0)
            {
                s = " - No Image";
                allcount++;
            }
            else if (count == 3) 
            {
                s = "";
            }
            else
            {
                s = " - Missing Image";
                allcount++;
            }

            Label l = new Label
            {
                Text = item.Num + ": " + item.Name + ": " + "(Lat: " + item.Latitude + ", Lng: " + item.Longitude + ")" + s,
                FontSize = 18
            };
            DownloadListLayout.Add(l);
        }

        PicNumLabel.Text = allcount.ToString();
  
    }

    // Start Downloading gombra kattintva ez fut le
    private void StartDownloadingClicked(object sender, EventArgs e)
    {
        //  interface meghívás
    }

    // Set gombra kattintva ez fut le
    private void SetClicked(object sender, EventArgs e)
    {
        useProxySettings = UseProxyCheckBox.IsChecked;
        address = addressEntry.Text.ToString();
        port = portEntry.Text.ToString();

        pathString = Path.Combine(apikeyPathString, "proxydatas.txt");
        using (var fs = System.IO.File.CreateText(pathString))
        {
            fs.Write(address + "\n" + port + "\n" + useProxySettings.ToString());
        }
    }

    public void GetProxyDatas()
    {
        StreamReader sr = new StreamReader(Path.Combine(apikeyPathString, "proxydatas.txt"));

        List<string> proxyDatas = new List<string>();

        while (!sr.EndOfStream)
        {
            proxyDatas.Add(sr.ReadLine());
        }
        sr.Close();

        address = proxyDatas[0].ToString();
        port = proxyDatas[1].ToString();
        if (proxyDatas[2].ToString() == "True")
        {
            useProxySettings = true;
        }
        else
        {
            useProxySettings = false;
        }
    }

    //  Help gombra kattintva ez fut le
    private async void HelpClicked(object sender, EventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://developers.google.com/maps/documentation/embed/get-api-key");
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            
        }
    }

    //  Browse gombra kattintva ez fut le
    private void BrowseClicked(object sender, EventArgs e)
    {
        if (datas == null)
        {
            DisplayAlert("Error", "Open a Project!", "OK");
        }
        else
        {
            ImageBG.IsVisible = false;
            DownloadLayout.IsVisible = false;
            BrowseIMG.IsVisible = true;
            ButtonLayout.IsVisible = true;
            ButtonLayout.Clear();

            foreach (var item in datas.GetProjectList())
            {

                Button b = new Button
                {
                    Text = item.Num + ", " + item.Name,
                    BackgroundColor = Color.Parse("AliceBlue"),
                    TextColor = Color.Parse("Black")                   
                };
                b.Clicked += new EventHandler(this.ImageShowClicked);
                ButtonLayout.Add(b);
            }

        }     
       
    }

    //  Az egyes képek megjelenítéséért felelõs gombok függvénye
    protected void ImageShowClicked(object sender, EventArgs e)
    {
        Button button = sender as Button;
        pathString = System.IO.Path.Combine(datas.GetPathString(), filename, button.Text.Substring(0,1), "01.bmp");
        BrowseIMG.Source = pathString;
    }

}