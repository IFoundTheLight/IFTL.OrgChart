using GraphVizNet;
using IFTL.OrgChart.Lib.Models;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;

namespace IFTL.OrgChart.Lib
{
    public partial class OrganizationChart
    {
        #region Private Constants
        private const string POSITIONNUMBERFIELD = "POSITION_NBR";
        private const string EMPLOYEEIDFIELD = "Emplid";
        private const string NAMEFIELD = "Name";
        private const string JOBCODEFIELD = "Job_CODE";
        private const string REPORTSTOFIELD = "Reports_TO";
        private const string TITLEFIELD = "DESCR";
        private const string HOMEINCUMBENTFIELD = "Home_Incumbent_Name";
        #endregion

        #region Private Variables
        public string _positionNumberField = POSITIONNUMBERFIELD;
        public string _employeeIdField = EMPLOYEEIDFIELD;
        public string _nameField = NAMEFIELD;
        public string _jobCodeField = JOBCODEFIELD;
        public string _reportsToField = REPORTSTOFIELD;
        public string _titleField = TITLEFIELD;
        public string _homeIncumbentField = HOMEINCUMBENTFIELD;
        #endregion

        #region Public Variables
        /// <summary>
        /// Position Number Field
        /// </summary>
        public string PositionNumberField
        {
            get
            {
                return _positionNumberField;
            }
            set
            {
                _positionNumberField = value;
            }
        }

        /// <summary>
        /// Employee Id Field
        /// </summary>
        public string EmployeeIdField
        {
            get { return _employeeIdField; }
            set { _employeeIdField = value; }
        }

        /// <summary>
        /// Name Field
        /// </summary>
        public string NameField
        {
            get { return _nameField; }
            set { _nameField = value; }
        }

        /// <summary>
        /// JobCode Field
        /// </summary>
        public string JobCodeField
        {
            get { return _jobCodeField; }
            set { _jobCodeField = value; }
        }

        /// <summary>
        /// ReportsTo Field
        /// </summary>
        public string ReportsToField
        {
            get { return _reportsToField; }
            set { _reportsToField = value; }
        }

        /// <summary>
        /// Title Field
        /// </summary>
        public string TitleField
        {
            get { return _titleField; }
            set { _titleField = value; }
        }

        /// <summary>
        /// HomeIncumbent Field
        /// </summary>
        public string HomeIncumbentField
        {
            get { return _homeIncumbentField; }
            set { _homeIncumbentField = value; }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Set the current fields to the ones in the model
        /// </summary>
        /// <param name="model"></param>
        private void UpdateFields(FieldParmaetersModel model)
        {
            if (!string.IsNullOrEmpty(model.NameField))
                this.NameField = model.NameField;

            if (!string.IsNullOrEmpty(model.PositionNumberField))
                this.PositionNumberField = model.PositionNumberField;

            if (!string.IsNullOrEmpty(model.ReportsToField))
                this.ReportsToField = model.ReportsToField;

            if (!string.IsNullOrEmpty(model.TitleField))
                this.TitleField = model.TitleField;

            if (!string.IsNullOrEmpty(model.HomeIncumbentField))
                this.HomeIncumbentField = model.HomeIncumbentField;

            if (!string.IsNullOrEmpty(model.JobCodeField))
                this.JobCodeField = model.JobCodeField;
        }

        /// <summary>
        /// Get Data as Datatable
        /// </summary>
        /// <returns></returns>
        private DataTable GetDataAsDataTable(string url)
        {
            // Get Data from url
            WebClient webc = new WebClient();
            string CSVdata = webc.DownloadString(url);

            DataTable dataTable = new DataTable();
            StringReader sr = new StringReader(CSVdata);
            using (TextFieldParser parser = new TextFieldParser(sr))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.HasFieldsEnclosedInQuotes = true;

                // Get the Fields
                string[] fields = parser.ReadFields();
                foreach (string field in fields)
                {
                    dataTable.Columns.Add(field);
                }

                while (!parser.EndOfData)
                {
                    // Get the Fields
                    fields = parser.ReadFields();

                    DataRow dRow = dataTable.NewRow();
                    dRow.ItemArray = fields;
                    dataTable.Rows.Add(dRow);
                }
            }
            return dataTable;
        }

        /// <summary>
        /// Get Employees
        /// </summary>
        /// <param name="employeeinfo"></param>
        /// <returns></returns>
        private List<Employee> GetEmployees(DataTable employeeinfo)
        {
            List<Employee> employees = new List<Employee>();

            foreach (DataRow row in employeeinfo.Rows)
            {
                Employee newEmployee = new Employee();
                newEmployee.PositionNumber = int.Parse(row[POSITIONNUMBERFIELD].ToString() ?? string.Empty);
                newEmployee.EmployeeId = int.Parse(row[EMPLOYEEIDFIELD].ToString() ?? string.Empty);
                newEmployee.Name = row[NAMEFIELD].ToString() ?? string.Empty;
                newEmployee.JobCode = row[JOBCODEFIELD].ToString() ?? string.Empty; ;
                if (!string.IsNullOrEmpty(row[REPORTSTOFIELD].ToString()))
                {
                    newEmployee.ReportsTo = int.Parse(row[REPORTSTOFIELD].ToString());
                }
                newEmployee.Title = row[TITLEFIELD].ToString() ?? string.Empty;
                newEmployee.HomeIncumbent = row[HOMEINCUMBENTFIELD].ToString() ?? string.Empty;
                employees.Add(newEmployee);
            }

            return employees;
        }

        /// <summary>
        /// Get Positions
        /// </summary>
        /// <param name="employees"></param>
        /// <returns></returns>
        private List<Position> GetPositions(List<Employee> employees)
        {
            List<Position> positions = new List<Position>();

            // Loop through employees
            foreach (Employee employee in employees)
            {
                bool PositionFound = false;

                // Loop through Positions
                foreach (Position position in positions)
                {

                    //Check if its a matching position
                    if (position.PositionNumber == employee.PositionNumber)
                    {
                        // We Found the position for the employee
                        PositionFound = true;

                        // Add Employee to position
                        position.Employees.Add(employee);

                        // Stop looking through position loop for this employee
                        break;
                    }
                }

                // Existing Position not found
                if (!PositionFound)
                {
                    // Create new position
                    Position newPosition = new Position(employee.PositionNumber,
                        employee.Title,
                        employee.JobCode,
                        employee.ReportsTo);

                    // Add Employee to position
                    newPosition.Employees.Add(employee);

                    // Add new poisiton to list of positions
                    positions.Add(newPosition);
                }
            }

            return positions;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Generate Image
        /// </summary>
        /// <returns></returns>
        public Image GenerateImage(GenerateImageModel model)
        {
            // Set Fields
            UpdateFields(model);

            // Get Data
            DataTable employeeInfo = GetDataAsDataTable(model.Url);

            // Get Employees
            List<Employee> employees = GetEmployees(employeeInfo);

            // Get Positions
            List<Position> positions = GetPositions(employees);

            // Build String
            StringBuilder graphString = new StringBuilder();
            graphString.Append("digraph { \n");
            foreach (Position position in positions)
            {
                string displayPosition = position.GetGraphvizPosition();
                if (displayPosition != null)
                {
                    graphString.Append($"{displayPosition}\n");
                }

                string reportTo = position.GetGraphvizPositionReportTo();
                if (reportTo != null)
                {
                    graphString.Append($"{reportTo}\n");
                }
            }
            graphString.Append("}");

            GraphViz graphViz = new GraphViz();
            byte[] renderedGraph = graphViz.LayoutAndRenderDotGraph(graphString.ToString(), "png");
            return Image.FromStream(new MemoryStream(renderedGraph));
        }

        /// <summary>
        /// Generate Graph String
        /// </summary>
        /// <returns></returns>
        public string GenerateGraphString(GenerateImageModel model)
        {
            // Set Fields
            UpdateFields(model);

            // Get Data
            DataTable employeeInfo = GetDataAsDataTable(model.Url);

            // Get Employees
            List<Employee> employees = GetEmployees(employeeInfo);

            // Get Positions
            List<Position> positions = GetPositions(employees);

            // Build String
            StringBuilder graphString = new StringBuilder();
            graphString.Append("digraph { \n");
            foreach (Position position in positions)
            {
                string displayPosition = position.GetGraphvizPosition();
                if (displayPosition != null)
                {
                    graphString.Append($"{displayPosition}\n");
                }

                string reportTo = position.GetGraphvizPositionReportTo();
                if (reportTo != null)
                {
                    graphString.Append($"{reportTo}\n");
                }
            }
            graphString.Append("}");
            return graphString.ToString();
        }
        #endregion
    }
}
