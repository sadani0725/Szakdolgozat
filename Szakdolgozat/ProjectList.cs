using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdolgozat
{
    //  Projekt területeket tartalmaz (projektnév, elérési útvonal, területek)
    public class ProjectList
    {
        string projectName;
        string pathString;
        List<ProjectType> list;
        public ProjectList(string _projectName, string _pathString) 
        {
            projectName = _projectName;
            pathString = _pathString;
            list = new List<ProjectType>();
        }
        public void AddNewProjectType(ProjectType pt)
        {
            list.Add(pt);
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
    }
}