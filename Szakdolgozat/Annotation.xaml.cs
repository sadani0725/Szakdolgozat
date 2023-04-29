using Accord.Statistics.Analysis;
using Accord.Statistics.Models.Regression.Linear;

namespace Szakdolgozat;

public partial class Annotation : ContentPage
{
    private static string ThisPage = "Annotation";
    ProjectList datas;
    List<KeyValuePair<int, double>> PCAResultList;
    public Annotation()
	{
		InitializeComponent();
        ReadSVM();
	}

    public Annotation(ProjectList _datas)
    {
        InitializeComponent();
        datas = _datas;
        ReadSVM();
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
        var labelsWindow = new Window
        {
            Page = new Labels(datas)
            {
                
            }          
        };

        labelsWindow.Destroying += (e, s) =>
        {
            labelsWindow = null;
        };

        labelsWindow.Stopped += (e, s) =>
        {
            labelsWindow = null;
        };

        labelsWindow.Resumed += (e, s) =>
        {
            labelsWindow = null;
        };

        labelsWindow.Deactivated += (e, s) =>
        {
            labelsWindow = null;
        };

        Application.Current.OpenWindow(labelsWindow);
    }

    //  Recalculate Indices gombra kattintva ez fut le
    private void RecalculateIndicesClicked(object sender, EventArgs e)
    {
        double avgB = 0;
        double avgV = 0;
        int sumB2 = 0;
        int sumV2 = 0;
        int sumP1 = 0;
        int counter = 0;

        double[][] data = new double[datas.GetProjectList().Count][];

        PCAResultList = new List<KeyValuePair<int, double>>();

        foreach (var item in datas.GetSVMList())
        {
            avgB += item.BuildingLabel;
            avgV += item.VegetationLabel;
            if (item.BuildingLabel == 2)
            {
                sumB2++;
            }
            if (item.VegetationLabel == 2)
            {
                sumV2++;
            }
            if (item.RoadLabel == 1)
            {
                sumP1++;
            }

            counter++;

            if (counter % 100 == 0)
            {
                avgB = avgB / 100;
                avgV = avgV / 100;

                double[] D = new double[5] { avgB, avgV, sumB2, sumV2, sumP1 };
                data[(counter / 100) - 1] = D;
              
                datas.RefreshPCADatas(datas.GetPCAList()[(counter / 100) - 1].Name, sumB2, sumV2, sumP1, avgB, avgV);              

                avgB = 0.0;
                avgV = 0.0;
                sumB2 = 0;
                sumV2 = 0;
                sumP1 = 0;
            }
        }

        double[][] output = PC1(data);

        for (int i = 0; i < datas.GetPCAList().Count; i++)
        {
            datas.RefreshPCAValues(datas.GetPCAList()[i].Name, -1 * output[i][0]);
        }

        WritePCA();

    }

    public double[][] PC1(double[][] data)
    {
        var pca = new PrincipalComponentAnalysis()
        {
            Method = PrincipalComponentMethod.Center,
            Whiten = true
        };

        MultivariateLinearRegression transform = pca.Learn(data);

        pca.NumberOfOutputs = 1;

        return pca.Transform(data);
    }

    public void ReadSVM()
    {
        datas.GetSVMList().Clear();

        string path = @"" + Path.Combine(datas.GetPathString(), datas.GetProjectName().Remove(datas.GetProjectName().Length - 4), "svmResult.csv");

        string[] lines = System.IO.File.ReadAllLines(path);
        foreach (string line in lines)
        {
            string[] subs = line.Split(';');
            if (subs[0] == "Picture_name")
            {

            }
            else
            {
                datas.AddNewSVMResult(new SVMResultType(subs[0], System.Convert.ToInt32(subs[0][subs[0].Remove(subs[0].Length - 7).Length - 1].ToString()), System.Convert.ToInt32(subs[1]), System.Convert.ToInt32(subs[2]), System.Convert.ToInt32(subs[3]), System.Convert.ToInt32(subs[4])));
            }
        }
    }

    public void WritePCA()
    {
        string path = @"" + Path.Combine(datas.GetPathString(), datas.GetProjectName().Remove(datas.GetProjectName().Length - 4), "pcaResult.csv");

        string s = "Picture_name;Cells_where_building_label=2;Cells_where_vegetation_label=2;Cells_where_road_label=1;Avg_building_labels;Avg_vegetation_labels;PCA_value;Location_name" + "\n";
        foreach (var item in datas.GetPCAList())
        {
            s += item.Path + ";" + item.CellsBuildingLabel2 + ";" + item.CellsVegetationLabel2 + ";" + item.CellsRoadLabel1 + ";" + item.AVGBuildingLabel + ";" + item.AVGVegetationLabel + ";" + item.PCAValue + ";" + item.Name + "\n";
        }

        using (var fs = System.IO.File.CreateText(path))
        {
            fs.Write(s);
        }
    }
}