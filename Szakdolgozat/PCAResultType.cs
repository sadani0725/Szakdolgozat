using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Szakdolgozat
{
    public class PCAResultType
    {
        public int Num { get; set; }
        public string Path { get; set; }
        public int CellsBuildingLabel2 { set; get; }
        public int CellsVegetationLabel2 { set; get; }
        public int CellsRoadLabel1 { set; get; }
        public double AVGBuildingLabel { set; get; }
        public double AVGVegetationLabel { set; get; }
        public double PCAValue { set; get; }
        public string Name { get; set; }

        public PCAResultType(int _Num, string _Path, int _CellsBuildingLabel2, int _CellsVegetationLabel2, int _CellsRoadLabel1, double _AVGBuildingLabel, double _AVGVegetationLabel, double _PCAValue, string _Name)
        {
            Num = _Num;
            Path = _Path;
            CellsBuildingLabel2= _CellsBuildingLabel2;
            CellsVegetationLabel2= _CellsVegetationLabel2;
            CellsRoadLabel1= _CellsRoadLabel1;
            AVGBuildingLabel= _AVGBuildingLabel;
            AVGVegetationLabel= _AVGVegetationLabel;
            PCAValue= _PCAValue;
            Name = _Name;
        }
        
    }
}
