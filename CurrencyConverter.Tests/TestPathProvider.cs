using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyConverter.Interfaces;

namespace CurrencyConverter.Tests
{
    public class TestPathProvider:IPathProvider
    {
        public string MapPath(string path)
        {
            var solutionDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            var testProj = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Name;
            var mainProj = solutionDir.Remove(solutionDir.LastIndexOf(testProj),testProj.Length);
            var mainProjPath = Path.Combine(mainProj, path);
            return mainProjPath;
        }
    }
}
