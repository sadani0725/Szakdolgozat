using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdolgozat
{
    //  Terület típus, egy terület ezekből az adatokból épül fel
    public class ProjectType
    {
        public int Num { get; set; }
        public double Latitude { set; get; }
        public double Longitude { set; get; }
        public string Name { set; get; }
        public double Size { set; get; }
        public double Zoom { set; get; }

        public ProjectType(int _Num, double _Latitude, double _Longitude, string _Name, double _Size,double _Zoom)
        {
            Num = _Num;
            Latitude = _Latitude;
            Longitude = _Longitude;
            Name = _Name;
            Size = _Size;
            Zoom = _Zoom;
        }
    }
}
