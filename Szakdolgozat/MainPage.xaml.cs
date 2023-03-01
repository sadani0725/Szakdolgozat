using Microsoft.VisualBasic.FileIO;

namespace Szakdolgozat;

public partial class MainPage : ContentPage
{

    //  A Page működéséhez szükséges adatok
	private static string ThisPage = "Project"; 
    string filename;    //  Projekt neve kiterjesztés nélkül
    string subfile;     //  Projekt neve kiterjesztéssel
    string pathString;  //  Elérési útvonal
    string projectPathString = System.IO.Path.Combine(@"c:\UrbanizationProjects", "Projects");      //  Alapértelmezett mentési hely Windows rendszereken (C: meghajtó)
    string exportedProjectsPathString = System.IO.Path.Combine(@"c:\UrbanizationProjects", "Exported Projects"); //  Alapértelmezett mentési hely Windows rendszereken (C: meghajtó)
    string apikeyPathString = System.IO.Path.Combine(@"c:\UrbanizationProjects", "ApiKey");         //  Alapértelmezett ApiKey hely Windows rendszereken (C: meghajtó)
    ProjectList datas;  //  Projekt adatait tárolja (Név, Elérési út, Adatok)

    //  Inicializálás
    public MainPage()
	{
		InitializeComponent();

        if (!System.IO.Directory.Exists(projectPathString))
        {
            System.IO.Directory.CreateDirectory(pathString);
        }

        if (!System.IO.Directory.Exists(exportedProjectsPathString))
        {
            System.IO.Directory.CreateDirectory(exportedProjectsPathString);
        }

        if (!System.IO.Directory.Exists(apikeyPathString))
        {
            System.IO.Directory.CreateDirectory(apikeyPathString);
            using (var fs = System.IO.File.CreateText(pathString))
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

    //  New Project gombra kattintva ez fut le
    private void NewProjectClicked(object sender, EventArgs e) 
    {
        bgImage.IsVisible = true;
        layoutProjectName.IsVisible = false;
        layoutProjectPath.IsVisible = false;
        textLocation.IsVisible = false;
        layoutTextAttributes.IsVisible = false;
        layoutEntryAttributes.IsVisible = false;
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
                layoutEntryAttributes.IsVisible = true;
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

                entryProjectName.Text = filename;
                entryProjectPath.Text = pathString;

                datas = new ProjectList(subfile, pathString);
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
        
        if (LocationName.Text == "" || LatitudeName.Text == "" || LongitudeName.Text == "" || AreaSize.Text == "" || Zoom.Text == "")
        {
            DisplayAlert("Error", "Some fields are empty!", "OK");
        }
        else
        {

            subfile = filename + ".csv";

            datas.AddNewProjectType(new ProjectType(datas.GetProjectList().Count + 1 , Convert.ToInt32(LatitudeName.Text.ToString()), Convert.ToInt32(LongitudeName.Text.ToString()), LocationName.Text.ToString(), Convert.ToInt32(AreaSize.Text.ToString()), Convert.ToInt32(Zoom.Text.ToString())));

            string s = "";
            foreach (var item in datas.GetProjectList())
            {
                s += item.Num + ";" + item.Latitude + ";" + item.Longitude + ";" + item.Name + ";" + item.Size + ";" + item.Zoom + "\n";
            }
            entryArea.Text = s;

        }
        
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

                datas = new ProjectList(subfile, pathString);

                string path = @"" + result.FullPath.ToString();

                string[] lines = System.IO.File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    string[] subs = line.Split(';');

                    datas.AddNewProjectType(new ProjectType(Convert.ToInt32(subs[0].ToString()), Convert.ToInt32(subs[1].ToString()), Convert.ToInt32(subs[2].ToString()), subs[3].ToString(), Convert.ToInt32(subs[4].ToString()), Convert.ToInt32(subs[5].ToString())));
                }



                bgImage.IsVisible = false;
                layoutProjectName.IsVisible = true;
                layoutProjectPath.IsVisible = true;
                textLocation.IsVisible = true;
                layoutTextAttributes.IsVisible = true;
                layoutEntryAttributes.IsVisible = true;
                layoutEntryArea.IsVisible = true;

                entryProjectName.Text = filename;
                entryProjectPath.Text = pathString;

                string s = "";
                foreach (var item in datas.GetProjectList())
                {
                    s += item.Num + ";" + item.Latitude + ";" + item.Longitude + ";" + item.Name + ";" + item.Size + ";" + item.Zoom + "\n";
                }
                entryArea.Text = s;

                pathString = System.IO.Path.Combine(projectPathString, filename, "pcaResult.csv");

                if (System.IO.File.Exists(pathString))
                {
                    lines = System.IO.File.ReadAllLines(pathString);                   
                    foreach (string line in lines)
                    {
                        string[] subs = line.Split(';');

                        datas.AddNewPCAResult(new PCAResultType(Convert.ToInt32(subs[0][subs[0].Length-8].ToString()), Convert.ToInt32(subs[1].ToString()), Convert.ToInt32(subs[2].ToString()), Convert.ToInt32(subs[3].ToString()), Convert.ToDouble(subs[4].Replace('.', ',').ToString()), Convert.ToDouble(subs[5].Replace('.', ',').ToString()), Convert.ToDouble(subs[6].Replace('.', ',').ToString()), subs[7].ToString()));
                    }
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", "The formation of the Project file is not accteptable!", "OK");
            DisplayAlert("Error", ex.Message, "OK");
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
        layoutEntryAttributes.IsVisible = false;
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
            if (entryArea.Text != "" && datas != null)
            {
                pathString = System.IO.Path.Combine(projectPathString, subfile);

                string s = "";
                foreach (var item in datas.GetProjectList())
                {
                    s += item.Num + ";" + item.Latitude + ";" + item.Longitude + ";" + item.Name + ";" + item.Size + ";" + item.Zoom + "\n";
                }

                using (var fs = System.IO.File.CreateText(pathString))
                {
                    fs.Write(s);
                }
                //DisplayAlert("Success", "Project created successfully!", "OK");

                for (int i = 0; i < datas.GetProjectList().Count; i++)
                {
                    pathString = System.IO.Path.Combine(projectPathString, filename);
                    pathString = System.IO.Path.Combine(pathString, (i+1).ToString());
                    if (!System.IO.Directory.Exists(pathString))
                    {
                        System.IO.Directory.CreateDirectory(pathString);
                    }
                }
                

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
            bgImage.IsVisible = false;
            layoutProjectName.IsVisible = true;
            layoutProjectPath.IsVisible = true;
            textLocation.IsVisible = true;
            layoutTextAttributes.IsVisible = true;
            layoutEntryAttributes.IsVisible = true;
            layoutEntryArea.IsVisible = true;

            entryProjectName.Text = filename;
            entryProjectPath.Text = pathString;

            if (datas != null)
            {
                string s = "";
                foreach (var item in datas.GetProjectList())
                {
                    s += item.Num + ";" + item.Latitude + ";" + item.Longitude + ";" + item.Name + ";" + item.Size + ";" + item.Zoom + "\n";
                }
                entryArea.Text = s;
            }
        }    

    }
}