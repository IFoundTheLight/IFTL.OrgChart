using IFTL.OrgChart.Lib;
using IFTL.OrgChart.Lib.Models;

namespace IFTL.OrgChart.Framework
{
    internal class Program
    {
        private const string URLADDRESS = "https://docs.google.com/spreadsheets/d/1oY7mOjyqEVhu3FlNhuaelEuwQ_Vxae6xnEcTYWFWKAE/export?format=csv";
        private static OrganizationChart oc = new OrganizationChart();
        static void Main(string[] args)
        {
            GenerateImageModel model = new GenerateImageModel();
            model.Url = URLADDRESS;
            oc.GenerateImage(model);
        }
    }
}
