using System.Diagnostics;
using System.IO.Compression;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;

namespace Szakdolgozat;

public partial class ImportExport : ContentPage
{
    private static string ThisPage = "ImportExport";
    string filename;    //  Projekt neve kiterjesztés nélkül
    string subfile;     //  Projekt neve kiterjesztéssel
    string pathString;  //  Elérési útvonal
    string importName;   //  Import neve
    string importPath;  //  Import elérési útvonala
    string projectPathString = System.IO.Path.Combine(@"c:\UrbanizationProjects", "Projects");      //  Alapértelmezett mentési hely Windows rendszereken (C: meghajtó)
    string exportedProjectsPathString = System.IO.Path.Combine(@"c:\UrbanizationProjects", "Exported Projects"); //  Alapértelmezett mentési hely Windows rendszereken (C: meghajtó)
    ProjectList datas;  //  Projekt adatait tárolja (Név, Elérési út, Adatok)
    List<KeyValuePair<CheckBox, int>> ExportList;     //  Hozzárendeli a CheckBoxokat a melletük lévõ hely számához
    List<KeyValuePair<CheckBox, string>> ImportList;     //  Hozzárendeli a CheckBoxokat a melletük lévõ hely számához

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
            ImportLayout.IsVisible = false;
            SelectButtons2Layout.IsVisible = false;
            ExportLayout.WidthRequest = Application.Current.MainPage.Width - Application.Current.MainPage.Height + 90;
            ExportListScroll.HeightRequest = Application.Current.MainPage.Height * 0.65;
            ImportListScroll.HeightRequest = Application.Current.MainPage.Height * 0.65;
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
                    Margin = 5
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
        int count = 0;
        foreach (var item in ExportList)
        {
            if (item.Key.IsChecked)
            {
                count++;
            }
        }
        if (count == 0)
        {
            DisplayAlert("Error", "Pick a Location first!", "OK");
        }
        else
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

                                string from = System.IO.Path.Combine(datas.GetPathString(), datas.GetProjectName().Remove(datas.GetProjectName().Length - 4), item2.Num.ToString());
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
                ZipFile.CreateFromDirectory(System.IO.Path.Combine(exportedProjectsPathString, GetSaveName.Text.ToString()), System.IO.Path.Combine(exportedProjectsPathString, GetSaveName.Text.ToString() + ".zip"));

                foreach (var item in System.IO.Directory.GetDirectories(System.IO.Path.Combine(exportedProjectsPathString, GetSaveName.Text.ToString())))
                {
                    foreach (var item2 in System.IO.Directory.GetDirectories(item))
                    {
                        foreach (var item3 in System.IO.Directory.GetDirectories(item2))
                        {
                            System.IO.Directory.Delete(item3);
                        }
                        foreach (var item3 in System.IO.Directory.GetFiles(item2))
                        {
                            System.IO.File.Delete(item3);
                        }
                        System.IO.Directory.Delete(item2);
                    }
                    foreach (var item2 in System.IO.Directory.GetFiles(item))
                    {
                        System.IO.File.Delete(item2);
                    }
                    System.IO.Directory.Delete(item);
                }
                foreach (var item in System.IO.Directory.GetFiles(System.IO.Path.Combine(exportedProjectsPathString, GetSaveName.Text.ToString())))
                {
                    System.IO.File.Delete(item);
                }
                System.IO.Directory.Delete(System.IO.Path.Combine(exportedProjectsPathString, GetSaveName.Text.ToString()));

                ExportLayout.WidthRequest = Application.Current.MainPage.Width - Application.Current.MainPage.Height + 90;
                ExportSelectedLocationButton.Margin = Application.Current.MainPage.Height - 350;
                ExportSelectedLocationButton.IsVisible = true;
                ExportFinalLayout.IsVisible = false;

                foreach (var item in ExportList)
                {
                    item.Key.IsChecked = false;
                }
                DisplayAlert("Success", "Files are exported!", "OK");
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
        if (datas == null)
        {
            DisplayAlert("Error", "Open a Project!", "OK");
        }
        else
        {
            var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".zip" } }
                });

            PickOptions options = new()
            {
                PickerTitle = "Please select a comic file",
                FileTypes = customFileType,
            };

            var result = PickAndShow(options);
        }       
    }

    // Az Open Project kisegítõ függvénye
    public async Task<FileResult> PickAndShow(PickOptions options)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(options);

            if (result != null)
            {
                ExportLayout.IsVisible = false;
                ExportLayout.Clear();
                SelectButtonsLayout.IsVisible = false;
                ImportLayout.WidthRequest = Application.Current.MainPage.Width - Application.Current.MainPage.Height + 90;
                ImportSelectedLocationButton.Margin = Application.Current.MainPage.Height - 350;
                ImageBG.IsVisible = false;
                ImportLayout.IsVisible = true;
                ImportLayout.Clear();
                SelectButtons2Layout.IsVisible = true;

                ImportList = new List<KeyValuePair<CheckBox, string>>();

                importPath = result.FullPath.ToString();
                filename = result.FileName.Remove(result.FileName.Length - 4);
                importName = filename;
                subfile = result.FileName;

                string path = @"" + Path.Combine(result.FullPath.ToString());

                using (FileStream file = File.OpenRead(path))
                {
                    using (ZipArchive zip = new ZipArchive(file, ZipArchiveMode.Read))
                    {
                        foreach (ZipArchiveEntry entry in zip.Entries)
                        {
                            if (entry.Name == filename + ".csv")
                            {
                                using (StreamReader sr = new StreamReader(entry.Open()))
                                {
                                    List<string> lines = new List<string>();

                                    while (!sr.EndOfStream)
                                    {
                                        lines.Add(sr.ReadLine());
                                    }

                                    foreach (string line in lines)
                                    {
                                        string[] subs = line.Split(';');

                                        StackLayout s = new StackLayout
                                        {
                                            Orientation = StackOrientation.Horizontal,
                                            Margin = 5,
                                        };

                                        Label l = new Label
                                        {
                                            Text = subs[0] + ";" + subs[1] + ";" + subs[2] + ";" + subs[3] + ";" + subs[4] + ";" + subs[5],
                                            Margin = 5
                                        };

                                        CheckBox cb = new CheckBox
                                        {
                                            BackgroundColor = Color.Parse("AliceBlue"),
                                            Color = Color.Parse("Skyblue"),
                                        };
                                        s.Add(cb);
                                        s.Add(l);
                                        ImportLayout.Add(s);
                                        ImportList.Add(new KeyValuePair<CheckBox, string>(cb, l.Text));

                                    }
                                }
                            }                            
                        }
                    }
                }                        
            }

            return result;
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", "The formation of the Imorted file is not accteptable!", "OK");
        }
        return null;
    }

    //  Select All gombra kattintva ez fut le
    private void SelectAll2Clicked(object sender, EventArgs e)
    {
        foreach (var item in ImportList)
        {
            item.Key.IsChecked = true;
        }
    }

    //  Select None gombra kattintva ez fut le
    private void SelectNone2Clicked(object sender, EventArgs e)
    {
        foreach (var item in ImportList)
        {
            item.Key.IsChecked = false;
        }
    }

    //  Import Selected Location gombra kattintva ez fut le
    private void ImportSelectedLocationsClicked(object sender, EventArgs e)
    {
        int max = 0;
        foreach (var item in datas.GetProjectList())
        {
            if (item.Num > max)
            {
                max = item.Num;
            }
        }

        string extract = Path.Combine(exportedProjectsPathString, importName);
        System.IO.Directory.CreateDirectory(Path.Combine(exportedProjectsPathString, importName));
        System.IO.Compression.ZipFile.ExtractToDirectory(importPath, extract);

        string s = "";
        foreach (var item in datas.GetProjectList())
        {
            s += item.Num + ";" + item.Latitude + ";" + item.Longitude + ";" + item.Name + ";" + item.Size + ";" + item.Zoom + "\n";
        }

        foreach (var item in ImportList)
        {
            if (item.Key.IsChecked)
            {
                max++;
                s += max + item.Value.Remove(0, item.Value.Split(';')[0].Length) + "\n";
                string[] subs = item.Value.Split(';');

                datas.AddNewProjectType(new ProjectType(max, Convert.ToDouble(subs[1].Replace('.',',')), Convert.ToDouble(subs[2].Replace('.', ',')), subs[3], Convert.ToDouble(subs[4].Replace('.', ',')), Convert.ToDouble(subs[5].Replace('.', ','))));

                string from = System.IO.Path.Combine(extract, importName, item.Value.Split(";")[0].ToString());
                string to = System.IO.Path.Combine(datas.GetPathString(), datas.GetProjectName().Remove(datas.GetProjectName().Length - 4), max.ToString());

                Copy(from, to);
            }
        }

        using (var fs = System.IO.File.CreateText(Path.Combine(datas.GetPathString(), datas.GetProjectName())))
        {
            fs.Write(s);           
        }

        foreach (var item in System.IO.Directory.GetDirectories(extract))
        {
            foreach (var item2 in System.IO.Directory.GetDirectories(item))
            {
                foreach (var item3 in System.IO.Directory.GetDirectories(item2))
                {
                    System.IO.Directory.Delete(item3);
                }
                foreach (var item3 in System.IO.Directory.GetFiles(item2))
                {
                    System.IO.File.Delete(item3);
                }
                System.IO.Directory.Delete(item2);
            }
            foreach (var item2 in System.IO.Directory.GetFiles(item))
            {
                System.IO.File.Delete(item2);
            }
            System.IO.Directory.Delete(item);
        }
        foreach (var item in System.IO.Directory.GetFiles(extract))
        {
            System.IO.File.Delete(item);
        }
        System.IO.Directory.Delete(System.IO.Path.Combine(extract));

        foreach (var item in ImportList)
        {
            item.Key.IsChecked = false;
        }
        DisplayAlert("Success", "Files are imported!", "OK");
    }
}