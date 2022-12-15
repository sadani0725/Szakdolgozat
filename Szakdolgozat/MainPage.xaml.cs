using Microsoft.VisualBasic.FileIO;

namespace Szakdolgozat;

public partial class MainPage : ContentPage
{
	private static string ThisPage = "Project";
    string folderName = @"c:\UrbanizationProjects";
    string filename;
    string pathString;
    string subfile;
    string[] fields;
    TextFieldParser parser;
    int num;
    List<string> datas = new List<string>();

    public MainPage()
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

    private void SaveNewProject(object sender, EventArgs e) 
    {

        if (getFilename.Text != null)
        {
            filename = getFilename.Text.ToString();

            pathString = System.IO.Path.Combine(folderName, "Projects");

            pathString = System.IO.Path.Combine(pathString, filename);

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
                pathString = System.IO.Path.Combine(folderName, "Projects", subfile);

                using (var fs = System.IO.File.CreateText(pathString))
                {
                    
                }

                num = 1;

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

    private void AddLocationClicked(object sender, EventArgs e) 
    {
        
        if (LocationName.Text == "" || LatitudeName.Text == "" || LongitudeName.Text == "" || AreaSize.Text == "" || Zoom.Text == "")
        {
            DisplayAlert("Error", "Some fields are empty!", "OK");
        }
        else
        {
            pathString = System.IO.Path.Combine(folderName, "Projects", filename);
            pathString = System.IO.Path.Combine(pathString, num.ToString());
            System.IO.Directory.CreateDirectory(pathString);
            subfile = filename + ".csv";

            List<string> localdatas = new List<string>();

            localdatas.Add(LocationName.Text.ToString());
            localdatas.Add(LatitudeName.Text.ToString());
            localdatas.Add(LongitudeName.Text.ToString());
            localdatas.Add(AreaSize.Text.ToString());
            localdatas.Add(Zoom.Text.ToString());

            datas.Add(LocationName.Text.ToString());
            datas.Add(LatitudeName.Text.ToString());
            datas.Add(LongitudeName.Text.ToString());
            datas.Add(AreaSize.Text.ToString());
            datas.Add(Zoom.Text.ToString());

            string s = "";
            foreach (var item in localdatas)
            {
                s += item + ";";
            }
            //s = s.Remove(s.Length-1);
            s += "\n";

            entryArea.Text += s;

            num++;
        }
        
    }

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

    public async Task<FileResult> PickAndShow(PickOptions options)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(options);

            if (result != null)
            {
                using (parser = new TextFieldParser(@"" + result.FullPath.ToString()))
                {
                    parser.HasFieldsEnclosedInQuotes = false;
                    parser.Delimiters = new[] { ";" };
                    while (!parser.EndOfData)
                    {
                        fields = parser.ReadFields();
                    }
                    
                }
            }

            foreach (var item in fields)
            {
                datas.Add(item);
            }

            filename = result.FileName;
            pathString = result.FullPath.ToString();

                bgImage.IsVisible = false;
                layoutProjectName.IsVisible = true;
                layoutProjectPath.IsVisible = true;
                textLocation.IsVisible = true;
                layoutTextAttributes.IsVisible = true;
                layoutEntryAttributes.IsVisible = true;
                layoutEntryArea.IsVisible = true;

                entryProjectName.Text = filename;
                entryProjectPath.Text = pathString;

                int i = 0;
                string s = "";
                foreach (var item in datas)
                {
                    if (i == 5)
                    {
                        //s = s.Remove(s.Length - 1);
                        s += "\n";
                        s += item + ";";
                        i = 0;
                    }
                    else
                    {
                        s += item + ";";
                    }
                    i++;
                }
                entryArea.Text = s;

            return result;
        }
        catch (Exception ex)
        {

        }

        return null;
    }

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

    private void SaveProjectClicked(object sender, EventArgs e)
    {
        if (datas.Count() == 0)
        {
            DisplayAlert("Error", "Open a Project!", "OK");
        }
        else
        {
            if (entryArea.Text != "")
            {
                pathString = System.IO.Path.Combine(folderName, "Projects", subfile);

                using (var fs = System.IO.File.CreateText(pathString))
                {
                    fs.Write(entryArea.Text.ToString());
                }
                //DisplayAlert("Success", "Project created successfully!", "OK");
                
            }
            else
            {
                DisplayAlert("Error", "The Project is Empty!", "OK");
            }
        }   
       
    }

    private void SettingsClicked(object sender, EventArgs e)
    {
        if (datas.Count() == 0)
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

            int i = 0;
            string s = "";
            foreach (var item in datas)
            {
                if (i == 5)
                {
                    //s = s.Remove(s.Length - 1);
                    s += "\n";
                    s += item + ";";
                    i = 0;
                }
                else
                {
                    s += item + ";";
                }
                i++;
            }
            entryArea.Text = s;
        }    

    }
}


