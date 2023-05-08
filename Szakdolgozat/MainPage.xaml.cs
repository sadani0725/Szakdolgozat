namespace Szakdolgozat;

public partial class MainPage : ContentPage
{

    //  A Page működéséhez szükséges adatok
	private static string ThisPage = "Project"; 
    string filename;    //  Projekt neve kiterjesztés nélkül
    string subfile;     //  Projekt neve kiterjesztéssel
    string pathString;  //  Elérési útvonal
    static string projectPathString = System.IO.Path.Combine(@"c:\UrbanizationProjects", "Projects");      //  Alapértelmezett mentési hely Windows rendszereken (C: meghajtó)
    static string exportedProjectsPathString = System.IO.Path.Combine(@"c:\UrbanizationProjects", "Exported Projects"); //  Alapértelmezett mentési hely Windows rendszereken (C: meghajtó)
    static string apikeyPathString = System.IO.Path.Combine(@"c:\UrbanizationProjects", "ApiKey");         //  Alapértelmezett ApiKey hely Windows rendszereken (C: meghajtó)
    ProjectList datas;  //  Projekt adatait tárolja (Név, Elérési út, Adatok)
    List<KeyValuePair<CheckBox, ProjectType>> RemoveList;     //  Hozzárendeli a CheckBoxokat a melletük lévő helyhez

    //  Inicializálás
    public MainPage()
	{
		InitializeComponent();

        if (!System.IO.Directory.Exists(projectPathString))
        {
            System.IO.Directory.CreateDirectory(projectPathString);
        }

        if (!System.IO.Directory.Exists(exportedProjectsPathString))
        {
            System.IO.Directory.CreateDirectory(exportedProjectsPathString);
        }

        if (!System.IO.Directory.Exists(apikeyPathString))
        {
            System.IO.Directory.CreateDirectory(apikeyPathString);
            using (var fs = System.IO.File.CreateText(Path.Combine(apikeyPathString, "apikey.txt")))
            {

            }
            using (var fs = System.IO.File.CreateText(Path.Combine(apikeyPathString, "proxydatas.txt")))
            {

            }
        }       
    }

    //  Inicializálás adatfogadással
    public MainPage(ProjectList _datas)
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
        if (datas == null)
        {
            DisplayAlert("Error", "Open a Project!", "OK");
        }
        else
        {
            await Navigation.PushModalAsync(new MainPage(datas));
        }       
    }

    //  Menüben a Google Maps gombra kattintáskor ez fut le
    private async void GoogleMapsClicked(object sender, EventArgs e)
    {
        if (ThisPage == "GoogleMaps")
        {
            return;
        }
        if (datas == null)
        {
            DisplayAlert("Error", "Open a Project!", "OK");
        }
        else
        {
            await Navigation.PushModalAsync(new GoogleMaps(datas));
        }
    }

    //  Menüben az Urbanization Score gombra kattintáskor ez fut le
    private async void UrbanizationScoreClicked(object sender, EventArgs e)
    {
        if (ThisPage == "UrbanizationScore")
        {
            return;
        }
        if (datas == null)
        {
            DisplayAlert("Error", "Open a Project!", "OK");
        }
        else
        {
            await Navigation.PushModalAsync(new UrbanizationScore(datas));
        }
    }

    //  Menüben az Annotation gombra kattintáskor ez fut le
    private async void AnnotationClicked(object sender, EventArgs e)
    {
        if (ThisPage == "Annotation")
        {
            return;
        }
        if (datas == null)
        {
            DisplayAlert("Error", "Open a Project!", "OK");
        }
        else
        {
            await Navigation.PushModalAsync(new Annotation(datas));
        }
    }

    //  Menüben az Import / Export gombra kattintáskor ez fut le
    private async void ImportExportClicked(object sender, EventArgs e)
    {
        if (ThisPage == "ImportExport")
        {
            return;
        }
        if (datas == null)
        {
            DisplayAlert("Error", "Open a Project!", "OK");
        }
        else
        {
            await Navigation.PushModalAsync(new ImportExport(datas));
        }
    }

    //  FrontEnd értékéket módosít
    public void MakeResponsive()
    {
        EntryScrollArea.WidthRequest = Application.Current.MainPage.Width - 300;
        EntryScrollArea.HeightRequest = Application.Current.MainPage.Height / 4;

        EntryArea.WidthRequest = Application.Current.MainPage.Width - 300;
        RemoveSelectedLocation.Margin = new Thickness(-10, Application.Current.MainPage.Width / 63, 0, 0);
        LocationName.WidthRequest = Application.Current.MainPage.Width / 8;
        LatitudeName.WidthRequest = Application.Current.MainPage.Width / 8;
        LongitudeName.WidthRequest = Application.Current.MainPage.Width / 8;
        AreaSize.WidthRequest = Application.Current.MainPage.Width / 8;
        Zoom.WidthRequest = Application.Current.MainPage.Width / 8;
        entryProjectPath.WidthRequest = Application.Current.MainPage.Width / 1.6;
        AddLocationBTN.HeightRequest = Application.Current.MainPage.Height / 22;
        ConvertAngleBTN.HeightRequest = Application.Current.MainPage.Height / 22;
        RemoveSelectedLocation.HeightRequest = Application.Current.MainPage.Height / 22;
        RemoveAllLocation.HeightRequest = Application.Current.MainPage.Height / 22;
        NameLayout.Margin = new Thickness(Application.Current.MainPage.Width / 32, 5, 0, 5);
        LatitLayout.Margin = new Thickness(Application.Current.MainPage.Width / 32, 5, 0, 5);
        LongitLayout.Margin = new Thickness(Application.Current.MainPage.Width / 32, 5, 0, 5);
        AreaSizeLayout.Margin = new Thickness(Application.Current.MainPage.Width / 32, 5, 0, 5);
        ZoomLayout.Margin = new Thickness(Application.Current.MainPage.Width / 32, 5, 0, 5);
        AddLocationBTN.FontSize = (Application.Current.MainPage.Height / 22) / 2.922;
        ConvertAngleBTN.FontSize = (Application.Current.MainPage.Height / 22) / 2.922;
        RemoveSelectedLocation.FontSize = (Application.Current.MainPage.Height / 22) / 2.922;
        RemoveAllLocation.FontSize = (Application.Current.MainPage.Height / 22) / 2.922;
        LongitudeConvertLabel.FontSize = (Application.Current.MainPage.Height / 22) / 2.922;
        LatitudeConvertLabel.FontSize = (Application.Current.MainPage.Height / 22) / 2.922;
        ConvertBtn.FontSize = (Application.Current.MainPage.Height / 22) / 2.922;
        HourEntryLong.FontSize = (Application.Current.MainPage.Height / 22) / 2.922;
        MinEntryLong.FontSize = (Application.Current.MainPage.Height / 22) / 2.922;
        SecEntryLong.FontSize = (Application.Current.MainPage.Height / 22) / 2.922;
        HourEntryLat.FontSize = (Application.Current.MainPage.Height / 22) / 2.922;
        MinEntryLat.FontSize = (Application.Current.MainPage.Height / 22) / 2.922;
        SecEntryLat.FontSize = (Application.Current.MainPage.Height / 22) / 2.922;
    }

    //  New Project gombra kattintva ez fut le
    private void NewProjectClicked(object sender, EventArgs e) 
    {
        bgImage.IsVisible = true;
        layoutProjectName.IsVisible = false;
        layoutProjectPath.IsVisible = false;
        textLocation.IsVisible = false;
        layoutTextAttributes.IsVisible = false;
        layoutEntryArea.IsVisible = false;

        openProjectBTN.IsVisible = false;
        saveProjectBTN.IsVisible = false;
        projectSettingsBTN.IsVisible = false;
        helpBTN.IsVisible = false;
        getFilename.IsVisible = true;
        createProjectBTN.IsVisible = true;
        backBTN.IsVisible = true;

    }

    //  New Project kisegítő függvénye
    private void SaveNewProject(object sender, EventArgs e) 
    {

        if (getFilename.Text != null)
        {
            filename = getFilename.Text.ToString();

            pathString = System.IO.Path.Combine(projectPathString, filename);

            if (!System.IO.Directory.Exists(pathString)) 
            {
                bgImage.IsVisible = false;
                layoutProjectName.IsVisible = true;
                layoutProjectPath.IsVisible = true;
                textLocation.IsVisible = true;
                layoutTextAttributes.IsVisible = true;
                layoutEntryArea.IsVisible = true;

                System.IO.Directory.CreateDirectory(pathString);

                subfile = filename + ".csv";
                pathString = System.IO.Path.Combine(projectPathString, subfile);

                using (var fs = System.IO.File.CreateText(pathString))
                {
                    
                }

                openProjectBTN.IsVisible = true;
                saveProjectBTN.IsVisible = true;
                projectSettingsBTN.IsVisible = true;
                helpBTN.IsVisible = true;
                getFilename.IsVisible = false;
                createProjectBTN.IsVisible = false;
                backBTN.IsVisible = false;
                getFilename.Text = "";

                pathString = System.IO.Path.Combine(projectPathString);
                datas = new ProjectList(subfile, pathString);

                entryProjectName.Text = filename;
                entryProjectPath.Text = datas.GetPathString();

                MakeResponsive();
                ShowEntryArea();
                
            }
            else
            {
                DisplayAlert("Error", "File " + filename + " already exists.", "OK");
            }
                      
        }
        else
        {
            DisplayAlert("Error", "Filename field can not be empty!", "OK");
        }  
    }

    //  Add Location gombra kattintva ez fut le
    private void AddLocationClicked(object sender, EventArgs e) 
    {
        int max = 0;
        foreach (var item in datas.GetProjectList())
        {
            if (item.Num > max)
            {
                max = item.Num;
            }
        }

        if (LocationName.Text == "" || LatitudeName.Text == "" || LongitudeName.Text == "" || AreaSize.Text == "" || Zoom.Text == "")
        {
            DisplayAlert("Error", "Some fields are empty!", "OK");
        }
        else
        {

            subfile = filename + ".csv";

            try
            {
                datas.AddNewProjectType(new ProjectType(max + 1, Convert.ToDouble(LatitudeName.Text.ToString()), Convert.ToDouble(LongitudeName.Text.ToString()), LocationName.Text.ToString(), Convert.ToDouble(AreaSize.Text.ToString()), Convert.ToDouble(Zoom.Text.ToString())));
                LocationName.Text = "";
                LatitudeName.Text = "";
                LongitudeName.Text = "";
                AreaSize.Text = "";
                Zoom.Text = "";
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "The formation of the input data was incorrect!", "OK");
            }

            ShowEntryArea();

        }
        
    }

    //  Convert Angle gombra kattintva ez fut le
    private void ConvertAngleClicked(object sender, EventArgs e)
    {
        AddLocationBTN.IsVisible = false;
        ConvertAngleBTN.IsVisible = false;       
        LatitudeLayout.IsVisible = true;
        LongitudeLayout.IsVisible = true;
        ConvertBtn.IsVisible = true;
    }

    //  Convert gombra kattintva ez fut le
    private void ConvertClicked(object sender, EventArgs e)
    {
        LatitudeName.Text = ((HourEntryLat.Text == "" ? 0.00 : Convert.ToDouble(HourEntryLat.Text)) + (MinEntryLat.Text == "" ? 0.00 : (Convert.ToDouble(MinEntryLat.Text) / 60)) + (SecEntryLat.Text == "" ? 0.00 : (Convert.ToDouble(SecEntryLat.Text) / 3600))).ToString("##0.######");
        LongitudeName.Text = ((HourEntryLong.Text == "" ? 0.00 : Convert.ToDouble(HourEntryLong.Text)) + (MinEntryLong.Text == "" ? 0.00 : (Convert.ToDouble(MinEntryLong.Text) / 60)) + (SecEntryLong.Text == "" ? 0.00 : (Convert.ToDouble(SecEntryLong.Text) / 3600))).ToString("##0.######");
        AddLocationBTN.IsVisible = true;
        ConvertAngleBTN.IsVisible = true;
        LatitudeLayout.IsVisible = false;
        LongitudeLayout.IsVisible = false;
        ConvertBtn.IsVisible = false;
        HourEntryLat.Text = "";
        MinEntryLat.Text = "";
        SecEntryLat.Text = "";
        HourEntryLong.Text = "";
        MinEntryLong.Text = "";
        SecEntryLong.Text = "";
    }

    //  Remove Selected Location gombra kattintva ez fut le
    private void RemoveSelectedLocationClicked(object sender, EventArgs e)
    {
        foreach (var item in RemoveList)
        {
            if (item.Key.IsChecked == true)
            {
                datas.RemoveFromProjectList(item.Value);
            }
        }
        ShowEntryArea();
    }

    //  Remove All Locations gombra kattintva ez fut le
    private void RemoveAllLocationsClicked(object sender, EventArgs e)
    {
        datas.ClearProjectList();
        ShowEntryArea();
    }

    //  Back gombra kattintva ez fut le
    private void Back(object sender, EventArgs e) 
    {
        openProjectBTN.IsVisible = true;
        saveProjectBTN.IsVisible = true;
        projectSettingsBTN.IsVisible = true;
        helpBTN.IsVisible = true;
        getFilename.IsVisible = false;
        createProjectBTN.IsVisible = false;
        backBTN.IsVisible = false;
        getFilename.Text = "";
    }

    //  Open Project gombra kattintva ez fut le
    private void OpenProjectClicked(object sender, EventArgs e) 
    {
        MakeResponsive();

        var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".csv" } }
                });

        PickOptions options = new()
        {
            PickerTitle = "Please select a comic file",
            FileTypes = customFileType,
        };

        var result = PickAndShow(options);
        
    }

    // Az Open Project kisegítő függvénye
    public async Task<FileResult> PickAndShow(PickOptions options)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(options);

            if (result != null)
            {
                subfile = result.FileName;
                filename = result.FileName.Remove(result.FileName.Length - 4);
                pathString = result.FullPath.ToString();

                datas = new ProjectList(subfile, pathString.Remove(pathString.Length - subfile.Length));

                string path = @"" + result.FullPath.ToString();

                string[] lines = System.IO.File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    string[] subs = line.Split(';');

                    datas.AddNewProjectType(new ProjectType(Convert.ToInt32(subs[0].ToString()), Convert.ToDouble(subs[1].ToString().Replace('.',',')), Convert.ToDouble(subs[2].ToString().Replace('.', ',')), subs[3].ToString(), Convert.ToDouble(subs[4].ToString().Replace('.', ',')), Convert.ToDouble(subs[5].ToString().Replace('.', ','))));
                }



                bgImage.IsVisible = false;
                layoutProjectName.IsVisible = true;
                layoutProjectPath.IsVisible = true;
                textLocation.IsVisible = true;
                layoutTextAttributes.IsVisible = true;
                layoutEntryArea.IsVisible = true;

                entryProjectName.Text = filename;
                entryProjectPath.Text = datas.GetPathString();

                ShowEntryArea();

                pathString = System.IO.Path.Combine(datas.GetPathString(), datas.GetProjectName().Remove(datas.GetProjectName().Length - 4), "pcaResult.csv");

                if (System.IO.File.Exists(pathString))
                {
                    lines = System.IO.File.ReadAllLines(pathString);                   
                    foreach (string line in lines)
                    {
                        string[] subs = line.Split(';');
                        if (subs[0] == "Picture_name")
                        {

                        }
                        else
                        {
                            datas.AddNewPCAResult(new PCAResultType(Convert.ToInt32(subs[0].Split("\\")[3].ToString()), subs[0].ToString(), Convert.ToInt32(subs[1].ToString()), Convert.ToInt32(subs[2].ToString()), Convert.ToInt32(subs[3].ToString()), Convert.ToDouble(subs[4].Replace('.', ',').ToString()), Convert.ToDouble(subs[5].Replace('.', ',').ToString()), Convert.ToDouble(subs[6].Replace('.', ',').ToString()), subs[7].ToString()));
                        }
                    }
                }
            }
            HourEntryLat.Text = "";
            MinEntryLat.Text = "";
            SecEntryLat.Text = "";
            HourEntryLong.Text = "";
            MinEntryLong.Text = "";
            SecEntryLong.Text = "";
            LocationName.Text = "";
            LatitudeName.Text = "";
            LongitudeName.Text = "";
            AreaSize.Text = "";
            Zoom.Text = "";

            return result;
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", "The formation of the Project file is not accteptable!", "OK");
        }

        return null;
    }

    //  Help gombra kattintva ez fut le
    private void HelpClicked(object sender, EventArgs e) 
    {
        bgImage.IsVisible = true;
        layoutProjectName.IsVisible = false;
        layoutProjectPath.IsVisible = false;
        textLocation.IsVisible = false;
        layoutTextAttributes.IsVisible = false;
        layoutEntryArea.IsVisible = false;
    }

    //  Save Project gombra kattintva ez fut le
    private void SaveProjectClicked(object sender, EventArgs e)
    {
        if (filename == null)
        {
            DisplayAlert("Error", "Open a Project!", "OK");
        }
        else
        {
            if (datas != null)
            {
                pathString = System.IO.Path.Combine(datas.GetPathString(), subfile);

                string s = "";
                foreach (var item in datas.GetProjectList())
                {
                    s += item.Num + ";" + item.Latitude + ";" + item.Longitude + ";" + item.Name + ";" + item.Size + ";" + item.Zoom + "\n";
                }

                using (var fs = System.IO.File.CreateText(pathString))
                {
                    fs.Write(s);
                }

                foreach (var item in datas.GetProjectList())
                {
                    int count = 0;
                    foreach (var item2 in System.IO.Directory.GetDirectories(System.IO.Path.Combine(datas.GetPathString(), datas.GetProjectName().Remove(datas.GetProjectName().Length - 4))))
                    {
                        if (item.Num == Convert.ToInt32(item2.Split("\\")[4].ToString()))
                        {
                            count++;
                        }
                    }
                    if (count == 0)
                    {
                        System.IO.Directory.CreateDirectory(System.IO.Path.Combine(datas.GetPathString(), datas.GetProjectName().Remove(datas.GetProjectName().Length - 4), item.Num.ToString()));
                    }
                }

                foreach (var item in System.IO.Directory.GetDirectories(System.IO.Path.Combine(datas.GetPathString(), datas.GetProjectName().Remove(datas.GetProjectName().Length - 4))))
                {
                    int count = 0;
                    foreach (var item2 in datas.GetProjectList())
                    {
                        if (item2.Num == Convert.ToInt32(item.ToString().Split("\\")[4].ToString()))
                        {                           
                            count++;
                        }
                    }
                    if (count == 0)
                    {
                        foreach (var item3 in System.IO.Directory.GetDirectories(item))
                        {
                            System.IO.Directory.Delete(item3);
                        }
                        foreach (var item3 in System.IO.Directory.GetFiles(item))
                        {
                            System.IO.File.Delete(item3);
                        }
                        System.IO.Directory.Delete(item);
                    }
                }
                DisplayAlert("Success", "The Project is Saved!", "OK");
            }
            else
            {
                DisplayAlert("Error", "The Project is Empty!", "OK");
            }
        }   
       
    }

    //  Project Settings gombra kattintva ez fut le
    private void SettingsClicked(object sender, EventArgs e)
    {
        if (filename == null)
        {
            DisplayAlert("Error", "Open a Project!", "OK");
        }
        else
        {
            MakeResponsive();
            bgImage.IsVisible = false;
            layoutProjectName.IsVisible = true;
            layoutProjectPath.IsVisible = true;
            textLocation.IsVisible = true;
            layoutTextAttributes.IsVisible = true;
            layoutEntryArea.IsVisible = true;

            entryProjectName.Text = filename;
            entryProjectPath.Text = datas.GetPathString();

            if (datas != null)
            {
                ShowEntryArea();
            }
        }    

    }

    //  Megjeleníti a Project Settings-ben a területi adatokat
    public void ShowEntryArea()
    {
        EntryArea.Clear();
        RemoveList = new List<KeyValuePair<CheckBox, ProjectType>>();

        foreach (var item in datas.GetProjectList())
        {
            StackLayout s = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Margin = 5,
            };

            Label l = new Label
            {
                Text = item.Num + ";" + item.Latitude + ";" + item.Longitude + ";" + item.Name + ";" + item.Size + ";" + item.Zoom,
                Margin = 5
            };

            CheckBox cb = new CheckBox
            {
                BackgroundColor = Color.Parse("AliceBlue"),
                Color = Color.Parse("Skyblue"),
            };
            s.Add(cb);
            s.Add(l);
            EntryArea.Add(s);
            RemoveList.Add(new KeyValuePair<CheckBox, ProjectType>(cb, item));
        }
    }
}