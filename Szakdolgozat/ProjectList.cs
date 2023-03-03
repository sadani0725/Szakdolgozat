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
        public ProjectList(string _projectName, string _pathString) 
        {
            projectName = _projectName;
            pathString = _pathString;
            list = new List<ProjectType>();
            listPCA = new List<PCAResultType>();
        }
        public void AddNewProjectType(ProjectType pt)
        {
            list.Add(pt);
        }
        public void AddNewPCAResult(PCAResultType prt)
        {
            listPCA.Add(prt);
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
        public void ClearProjectList()
        {
            list.Clear();
        }
        public void RemoveFromProjectList(ProjectType pt)
        {
            list.Remove(pt);
        }
    }
}