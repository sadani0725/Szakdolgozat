using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using System.Collections.Specialized;
using System.Diagnostics.Metrics;

namespace Szakdolgozat;

public partial class Labels : ContentPage
{
    string filename;    //  Projekt neve kiterjesztés nélkül
    string subfile;     //  Projekt neve kiterjesztéssel
    string pathString;  //  Elérési útvonal
    static string projectPathString = System.IO.Path.Combine(@"c:\UrbanizationProjects", "Projects");      //  Alapértelmezett mentési hely Windows rendszereken (C: meghajtó)
    ProjectList datas;
    List<Button> buildingsButtons;
    List<Button> vegetationsButtons;
    List<Button> roadsButtons;
    Color changeState;
    public Labels()
    {
        InitializeComponent();
        LoadPage();
    }

    public Labels(ProjectList _datas)
    {
        InitializeComponent();
        if (_datas != null)
        {
            datas = _datas;
            filename = _datas.GetProjectName().Remove(_datas.GetProjectName().Length - 4);
            subfile = _datas.GetProjectName();
            pathString = _datas.GetPathString();
        }

        LoadPage();
    }

    public void LoadPage()
    {
        BuildingLayout.Clear();
        VegetationLayout.Clear();
        RoadLayout.Clear();
        if (buildingsButtons != null) 
            buildingsButtons.Clear();
        if (vegetationsButtons != null)
            vegetationsButtons.Clear();
        if (roadsButtons != null)
            roadsButtons.Clear();

        var dropdownList = new List<string>();

        foreach (var item in datas.GetProjectList())
        {
            foreach (var item2 in datas.GetPCAList())
            {
                if (item.Name == item2.Name)
                {
                    dropdownList.Add(item.Num + ": " + item.Name + ": " + item2.PCAValue.ToString() + "(Lat: " + item.Latitude + ", Lng: " + item.Longitude + ")");
                }
            }
        }

        picker.ItemsSource = dropdownList;
        picker.SelectedIndex = 0;
        picker.SelectedItem = picker.Items[0];

        changeState = Color.Parse("White");
        SetWhiteBTN.BorderColor = Color.Parse("Red");
        SetWhiteBTN.BorderWidth = 3;
        SetBlackBTN.BorderColor = Color.Parse("Black");
        SetBlackBTN.BorderWidth = 1;
        SetGrayBTN.BorderColor = Color.Parse("Gray");
        SetGrayBTN.BorderWidth = 1;

        ShowLabels();
    }

    

    private void PickerChanged(object sender, EventArgs e)
    {
        ShowLabels();
    }

    public void ShowLabels()
    {
        BuildingLayout.Clear();
        VegetationLayout.Clear();
        RoadLayout.Clear();
        if (buildingsButtons != null)
            buildingsButtons.Clear();
        if (vegetationsButtons != null)
            vegetationsButtons.Clear();
        if (roadsButtons != null)
            roadsButtons.Clear();

        StackLayout b = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
        };

        StackLayout v = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
        };

        StackLayout r = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
        };

        StackLayout i = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
        };

        int counter = 0;
        Color buttonBG = Color.Parse("AliceBlue");

        buildingsButtons = new List<Button>();
        vegetationsButtons = new List<Button>();
        roadsButtons = new List<Button>();

        foreach (var item in datas.GetSVMList())
        {

            if (item.Num == System.Convert.ToInt32(picker.Items[picker.SelectedIndex].ToString()[0].ToString()))
            {

                if (item.BuildingLabel == 0)
                {
                    buttonBG = Color.Parse("White");
                }
                else if (item.BuildingLabel == 1)
                {
                    buttonBG = Color.Parse("Gray");
                }
                else if (item.BuildingLabel == 2)
                {
                    buttonBG = Color.Parse("Black");
                }

                Button bbtn = new Button
                {
                    Margin = -4.5,
                    WidthRequest = 35,
                    HeightRequest = 35,
                    BackgroundColor = buttonBG,
                    BorderColor = buttonBG == Color.Parse("Gray") ? Color.Parse("Gray") : Color.Parse("Black"),
                    Scale = 0.65,
                    BorderWidth = 1,
                };
                bbtn.Clicked += new EventHandler(this.ImageShowClicked);
                b.Add(bbtn);
                buildingsButtons.Add(bbtn);

                if (item.VegetationLabel == 0)
                {
                    buttonBG = Color.Parse("White");
                }
                else if (item.VegetationLabel == 1)
                {
                    buttonBG = Color.Parse("Gray");
                }
                else if (item.VegetationLabel == 2)
                {
                    buttonBG = Color.Parse("Black");
                }

                Button vbtn = new Button
                {
                    Margin = -4.5,
                    WidthRequest = 35,
                    HeightRequest = 35,
                    BackgroundColor = buttonBG,
                    BorderColor = buttonBG == Color.Parse("Gray") ? Color.Parse("Gray") : Color.Parse("Black"),
                    Scale = 0.65,
                    BorderWidth = 1,
                };
                vbtn.Clicked += new EventHandler(this.ImageShowClicked);
                v.Add(vbtn);
                vegetationsButtons.Add(vbtn);

                if (item.RoadLabel == 0)
                {
                    buttonBG = Color.Parse("White");
                }
                else if (item.RoadLabel == 1)
                {
                    buttonBG = Color.Parse("Black");
                }

                Button rbtn = new Button
                {
                    Margin = -4.5,
                    WidthRequest = 35,
                    HeightRequest = 35,
                    BackgroundColor = buttonBG,
                    BorderColor = buttonBG == Color.Parse("Gray") ? Color.Parse("Gray") : Color.Parse("Black"),
                    Scale = 0.65,
                    BorderWidth = 1,
                };
                rbtn.Clicked += new EventHandler(this.ImageShowClicked);
                r.Add(rbtn);
                roadsButtons.Add(rbtn);

                counter++;

                if (counter % 10 == 0)
                {
                    BuildingLayout.Add(b);
                    b = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                    };

                    VegetationLayout.Add(v);
                    v = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                    };

                    RoadLayout.Add(r);
                    r = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                    };
                }

                ImageSource.Source = System.IO.Path.Combine(datas.GetPathString(), filename, item.Num.ToString(), "01_grid.bmp");

                if (counter == 100)
                {
                    break;
                }
            }
        }
    }

    protected void ImageShowClicked(object sender, EventArgs e)
    {
        Button button = sender as Button;

        for (int i = 0; i < buildingsButtons.Count; i++)
        {
            buildingsButtons[i].BorderColor = buildingsButtons[i].BackgroundColor == Color.Parse("Grey") ? Color.Parse("Gray") : Color.Parse("Black");
            vegetationsButtons[i].BorderColor = vegetationsButtons[i].BackgroundColor == Color.Parse("Grey") ? Color.Parse("Gray") : Color.Parse("Black");
            roadsButtons[i].BorderColor = roadsButtons[i].BackgroundColor == Color.Parse("Grey") ? Color.Parse("Gray") : Color.Parse("Black");
            buildingsButtons[i].BorderWidth = 1;
            vegetationsButtons[i].BorderWidth = 1;
            roadsButtons[i].BorderWidth = 1;

            if (buildingsButtons[i] == button || vegetationsButtons[i] == button || roadsButtons[i] == button)
            {
                buildingsButtons[i].BorderColor = Color.Parse("Red");
                vegetationsButtons[i].BorderColor = Color.Parse("Red");
                roadsButtons[i].BorderColor = Color.Parse("Red");
                buildingsButtons[i].BorderWidth = 3;
                vegetationsButtons[i].BorderWidth = 3;
                roadsButtons[i].BorderWidth = 3;

                if (isEditable.IsChecked)
                {
                    if (isBuilding.IsChecked)
                    {
                        buildingsButtons[i].BackgroundColor = changeState;
                    }
                    else if (isVegetion.IsChecked)
                    {
                        vegetationsButtons[i].BackgroundColor = changeState;
                    }
                    else if (isRoad.IsChecked)
                    {
                        roadsButtons[i].BackgroundColor = changeState;
                    }
                }
            }
        }
    }

    protected void SetWhite(object sender, EventArgs e)
    {
        changeState = Color.Parse("White");
        SetWhiteBTN.BorderColor = Color.Parse("Red");
        SetWhiteBTN.BorderWidth = 3;
        SetBlackBTN.BorderColor = Color.Parse("Black");
        SetBlackBTN.BorderWidth = 1;
        SetGrayBTN.BorderColor = Color.Parse("Gray");
        SetGrayBTN.BorderWidth = 1;
    }

    protected void SetGray(object sender, EventArgs e)
    {
        if (isRoad.IsChecked)
        {

        }
        else
        {
            changeState = Color.Parse("Gray");
            SetGrayBTN.BorderColor = Color.Parse("Red");
            SetGrayBTN.BorderWidth = 3;
            SetWhiteBTN.BorderColor = Color.Parse("Black");
            SetWhiteBTN.BorderWidth = 1;
            SetBlackBTN.BorderColor = Color.Parse("Black");
            SetBlackBTN.BorderWidth = 1;
        }            
    }

    protected void SetBlack(object sender, EventArgs e)
    {
        changeState = Color.Parse("Black");
        SetBlackBTN.BorderColor = Color.Parse("Red");
        SetBlackBTN.BorderWidth = 3;
        SetGrayBTN.BorderColor = Color.Parse("Gray");
        SetGrayBTN.BorderWidth = 1;
        SetWhiteBTN.BorderColor = Color.Parse("Black");
        SetWhiteBTN.BorderWidth = 1;
    }

    protected void IsRoadClicked(object sender, EventArgs e)
    {
        if (isRoad.IsChecked && changeState == Color.Parse("Gray"))
        {
            changeState = Color.Parse("Black");
            SetBlackBTN.BorderColor = Color.Parse("Red");
            SetBlackBTN.BorderWidth = 3;
            SetWhiteBTN.BorderColor = Color.Parse("Black");
            SetWhiteBTN.BorderWidth = 1;
            SetGrayBTN.BorderColor = Color.Parse("Gray");
            SetGrayBTN.BorderWidth = 1;
        }        
    }

    protected void SaveChangesClicked(object sender, EventArgs e)
    {
        int counter = 0;
        foreach (var item in datas.GetSVMList())
        {
            if (item.Num == System.Convert.ToInt32(picker.Items[picker.SelectedIndex].ToString()[0].ToString()))
            {
                item.BuildingLabel = buildingsButtons[counter].BackgroundColor == Color.Parse("Black") ? 2 : buildingsButtons[counter].BackgroundColor == Color.Parse("Gray") ? 1 : 0;
                item.VegetationLabel = vegetationsButtons[counter].BackgroundColor == Color.Parse("Black") ? 2 : vegetationsButtons[counter].BackgroundColor == Color.Parse("Gray") ? 1 : 0;
                item.RoadLabel = roadsButtons[counter].BackgroundColor == Color.Parse("Black") ? 1 : 0;

                counter++;
            }
        }

        string path = @"" + Path.Combine(datas.GetPathString(), datas.GetProjectName().Remove(datas.GetProjectName().Length - 4), "svmResult.csv");

        string s = "Picture_name;Cell_name;Building_label;Vegetation_label;Road_label" + "\n";
        foreach (var item in datas.GetSVMList())
        {
            s += item.Name + ";" + item.Cell + ";" + item.BuildingLabel + ";" + item.VegetationLabel + ";" + item.RoadLabel + "\n";
        }

        using (var fs = System.IO.File.CreateText(path))
        {
            fs.Write(s);
        }
    }
}