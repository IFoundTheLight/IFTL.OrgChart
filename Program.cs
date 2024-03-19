using IFTL.OrgChart.Lib;
using Microsoft.VisualBasic.FileIO;
using System.Data;
using System.Net;
using System.Text;

namespace IFTL.OrgChart
{
    internal class Program
    {
        private static OrganizationChart oc = new OrganizationChart();
        static void Main(string[] args)
        {
            oc.GenerateImage();
        }
    }
}
