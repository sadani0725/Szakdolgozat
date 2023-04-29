using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdolgozat
{
    public class SVMResultType
    {
        public string Name { get; set; }
        public int Num { set; get; }
        public int Cell { set; get; }
        public int BuildingLabel { set; get; }
        public int VegetationLabel { set; get; }
        public int RoadLabel { set; get; }

        public SVMResultType(string _Name, int _Num, int _Cell, int _BuildingLabel, int _VegetationLabel, int _RoadLabel) 
        { 
            Name = _Name;
            Num = _Num;
            Cell = _Cell;
            BuildingLabel = _BuildingLabel;
            VegetationLabel= _VegetationLabel;
            RoadLabel = _RoadLabel;
        }

    }
}
