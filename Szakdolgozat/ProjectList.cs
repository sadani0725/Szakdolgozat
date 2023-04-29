using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdolgozat
{
    //  Projekt területeket tartalmaz (projektnév, elérési útvonal, területek, PCA eredmények)
    public class ProjectList
    {
        string projectName;
        string pathString;
        List<ProjectType> list;
        List<PCAResultType> listPCA;
        List<SVMResultType> listSVM;
        public ProjectList(string _projectName, string _pathString) 
        {
            projectName = _projectName;
            pathString = _pathString;
            list = new List<ProjectType>();
            listPCA = new List<PCAResultType>();
            listSVM = new List<SVMResultType>();
        }
        public void AddNewProjectType(ProjectType pt)
        {
            list.Add(pt);
        }
        public void AddNewPCAResult(PCAResultType prt)
        {
            listPCA.Add(prt);
        }
        public void AddNewSVMResult(SVMResultType srt)
        {
            listSVM.Add(srt);
        }
        public string GetProjectName() 
        {
            return projectName;
        }
        public string GetPathString()
        {
            return pathString;
        }
        public List<ProjectType> GetProjectList() 
        {
            return list;
        }
        public List<PCAResultType> GetPCAList() 
        {
            return listPCA;
        }
        public List<SVMResultType> GetSVMList()
        {
            return listSVM;
        }
        public void ClearProjectList()
        {
            list.Clear();
        }
        public void RemoveFromProjectList(ProjectType pt)
        {
            list.Remove(pt);
        }

        public void RefreshPCADatas(string _Name, int _CellsBuildingLabel2, int _CellsVegetationLabel2, int _CellsRoadLabel1, double _AVGBuildingLabel, double _AVGVegetationLabel)
        {
            foreach (var item in listPCA)
            {
                if (item.Name == _Name)
                {
                    item.CellsBuildingLabel2 = _CellsBuildingLabel2;
                    item.CellsVegetationLabel2 = _CellsVegetationLabel2;
                    item.CellsRoadLabel1 = _CellsRoadLabel1;
                    item.AVGBuildingLabel= _AVGBuildingLabel;
                    item.AVGVegetationLabel = _AVGVegetationLabel;
                }
            }
        }

        public void RefreshPCAValues(string _Name, double _PCAValue)
        {
            foreach (var item in listPCA)
            {
                if (item.Name == _Name)
                {
                    item.PCAValue = _PCAValue;
                }
            }
        }
    }
}