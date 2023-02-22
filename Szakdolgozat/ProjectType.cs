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
        public int Latitude { set; get; }
        public int Longitude { set; get; }
        public string Name { set; get; }
        public int Size { set; get; }
        public int Zoom { set; get; }

        public ProjectType(int _Num, int _Latitude, int _Longitude, string _Name, int _Size, int _Zoom)
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
