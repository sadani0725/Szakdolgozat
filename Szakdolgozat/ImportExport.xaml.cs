using System.Diagnostics;
using System.IO.Compression;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Szakdolgozat;

public partial class ImportExport : ContentPage
{
    private static string ThisPage = "ImportExport";
    string filename;    //  Projekt neve kiterjesztés nélkül
    string subfile;     //  Projekt neve kiterjesztéssel
    string pathString;  //  Elérési útvonal
    string projectPathString = System.IO.Path.Combine(@"c:\UrbanizationProjects", "Projects");      //  Alapértelmezett mentési hely Windows rendszereken (C: meghajtó)
    string exportedProjectsPathString = System.IO.Path.Combine(@"c:\UrbanizationProjects", "Exported Projects"); //  Alapértelmezett mentési hely Windows rendszereken (C: meghajtó)
    ProjectList datas;  //  Projekt adatait tárolja (Név, Elérési út, Adatok)
    List<KeyValuePair<CheckBox, int>> ExportList;     //  Hozzárendeli a CheckBoxokat a melletük lévõ hely számához

    //  Inicializálás
    public ImportExport()
	{
		InitializeComponent();
	}

    //  Inicializálás adatfogadással
    public ImportExport(ProjectList _datas)
    {
        InitializeComponent();
        if (_datas != null)
        {
            datas = _datas;
            filename = _datas.GetProjectName().Remove(_datas.GetProjectName().Length - 4);
            subfile = _datas.GetProjectName();
            pathString = _datas.GetPathString();
        }     
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

    //  Import gombra kattintva ez fut le
    private void ExportClicked(object sender, EventArgs e)
    {
        if (datas == null)
        {
            DisplayAlert("Error", "Open a Project!", "OK");
        }
        else
        {
            ExportLayout.WidthRequest = Application.Current.MainPage.Width - Application.Current.MainPage.Height + 90;
            ExportSelectedLocationButton.Margin = Application.Current.MainPage.Height - 350;
            ImageBG.IsVisible = false;
            ExportLayout.IsVisible = true;
            ExportLayout.Clear();
            SelectButtonsLayout.IsVisible = true;

            ExportList = new List<KeyValuePair<CheckBox, int>>();

            foreach (var item in datas.GetProjectList())
            {
                double PCA = 0;

                foreach (var item2 in datas.GetPCAList())
                {
                    if (item2.Name == item.Name)
                    {
                        PCA = Convert.ToDouble(item2.PCAValue);
                    }                   
                }

                StackLayout s = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Margin = 5,                 
                };
              
                Label l = new Label
                {
                    Text = item.Name + ": " + PCA.ToString() + "(Lat: " + item.Latitude + ", Lng: " + item.Longitude + ")",
                    Margin= 5
                };

                CheckBox cb = new CheckBox
                {
                    BackgroundColor = Color.Parse("AliceBlue"),
                    Color = Color.Parse("Skyblue"),
                };
                s.Add(cb);
                s.Add(l);
                ExportLayout.Add(s);
                ExportList.Add(new KeyValuePair<CheckBox, int>(cb, item.Num));
            }

        }
    }   

    //  Select All gombra kattintva ez fut le
    private void SelectAllClicked(object sender, EventArgs e)
    {
        foreach (var item in ExportList)
        {
            item.Key.IsChecked = true;
        }
    }

    //  Select None gombra kattintva ez fut le
    private void SelectNoneClicked(object sender, EventArgs e)
    {
        foreach (var item in ExportList)
        {
            item.Key.IsChecked = false;
        }
    }

    //  Export Selected Locations gombra kattintva ez fut le
    private void ExportSelectedLocationsClicked(object sender, EventArgs e)
    {
        ExportLayout.WidthRequest = Application.Current.MainPage.Width - Application.Current.MainPage.Height + 110;
        ExportFinalLayout.Margin = Application.Current.MainPage.Height - 390;

        ExportSelectedLocationButton.IsVisible = false;
        ExportFinalLayout.IsVisible = true;
    }

    //  Export Selected Locations gombra kattintva ez fut le
    private void SaveExportClicked(object sender, EventArgs e)
    {
        if (!System.IO.Directory.Exists(exportedProjectsPathString))
        {
            System.IO.Directory.CreateDirectory(exportedProjectsPathString);
        }

        pathString = System.IO.Path.Combine(exportedProjectsPathString, GetSaveName.Text.ToString());
        

        if (System.IO.File.Exists(pathString))
        {
            DisplayAlert("Error", "File " + GetSaveName.Text.ToString() + " already exists.", "OK");
        }
        else
        {
            System.IO.Directory.CreateDirectory(pathString);
            pathString = System.IO.Path.Combine(exportedProjectsPathString, GetSaveName.Text.ToString(), GetSaveName.Text.ToString(), "/");
            System.IO.Directory.CreateDirectory(pathString);
            subfile = GetSaveName.Text.ToString() + ".csv";
            pathString = System.IO.Path.Combine(exportedProjectsPathString, GetSaveName.Text.ToString(), subfile);

            string s = "";

            foreach (var item in ExportList)
            {
                if (item.Key.IsChecked == true)
                {                   
                    foreach (var item2 in datas.GetProjectList())
                    {
                        if (item2.Num == item.Value)
                        {
                            s += item2.Num + ";" + item2.Latitude + ";" + item2.Longitude + ";" + item2.Name + ";" + item2.Size + ";" + item2.Zoom + "\n";

                            string from = System.IO.Path.Combine(projectPathString, datas.GetProjectName().Remove(datas.GetProjectName().Length - 4), item2.Num.ToString());
                            string to = System.IO.Path.Combine(exportedProjectsPathString, GetSaveName.Text.ToString(), GetSaveName.Text.ToString(), item2.Num.ToString());

                            Copy(from, to);
                        }                      
                    }

                    using (var fs = System.IO.File.CreateText(pathString))
                    {
                        fs.Write(s);
                    }
                  
                }
            }
            ExportLayout.WidthRequest = Application.Current.MainPage.Width - Application.Current.MainPage.Height + 90;
            ExportSelectedLocationButton.Margin = Application.Current.MainPage.Height - 350;
            ExportSelectedLocationButton.IsVisible = true;
            ExportFinalLayout.IsVisible = false;

            foreach (var item in ExportList)
            {
                item.Key.IsChecked = false;
            }
        }
        
    }

    // A Save Export kisegítõ függvénye
    public static void Copy(string sourceDirectory, string targetDirectory)
    {
        var diSource = new DirectoryInfo(sourceDirectory);
        var diTarget = new DirectoryInfo(targetDirectory);

        CopyAll(diSource, diTarget);
    }

    // A Save Export kisegítõ függvénye
    public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
    {
        Directory.CreateDirectory(target.FullName);

        foreach (FileInfo fi in source.GetFiles())
        {
            Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
            fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
        }

        foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
        {
            DirectoryInfo nextTargetSubDir =
                target.CreateSubdirectory(diSourceSubDir.Name);
            CopyAll(diSourceSubDir, nextTargetSubDir);
        }
    }

    //  Export Selected Locations gombra kattintva ez fut le
    private void BackExportClicked(object sender, EventArgs e)
    {
        ExportLayout.WidthRequest = Application.Current.MainPage.Width - Application.Current.MainPage.Height + 90;
        ExportSelectedLocationButton.Margin = Application.Current.MainPage.Height - 350;
        ExportSelectedLocationButton.IsVisible = true;
        ExportFinalLayout.IsVisible = false;
    }

    //  Export gombra kattintva ez fut le
    private void ImportClicked(object sender, EventArgs e)
    {

    }
}