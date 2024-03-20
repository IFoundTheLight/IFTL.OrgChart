using System.Collections.Generic;

namespace IFTL.OrgChart.Lib
{
    /// <summary>
    /// Position
    /// </summary>
    public class Position
    {
        #region Public Variables
        public int PositionNumber { get; set; }
        public string Title { get; set; }
        public string JobCode { get; set; }
        public int ReportsTo { get; set; }
        public List<Employee> Employees { get; set; } = new List<Employee>();
        #endregion

        #region Constructors
        /// <summary>
        /// Position
        /// </summary>
        public Position()
        {

        }

        /// <summary>
        /// Position
        /// </summary>
        /// <param name="positionNumber"></param>
        /// <param name="title"></param>
        /// <param name="jobCode"></param>
        /// <param name="reportsTo"></param>
        public Position(int positionNumber, string title, string jobCode, int reportsTo)
        {
            PositionNumber = positionNumber;
            Title = title;
            JobCode = jobCode;
            ReportsTo = reportsTo;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Get Displayable Employee List
        /// </summary>
        /// <returns></returns>
        private string GetEmployeeDisplay()
        {
            List<string> EmployeesDisplay = new List<string>();
            Employees.ForEach(x => EmployeesDisplay.Add(x.GetEmployeeDisplay()));
            return string.Join("\\n", EmployeesDisplay);
        }

        /// <summary>
        /// Get Position Display
        /// </summary>
        /// <returns></returns>
        private string GetPositionDisplay()
        {
            return $"{PositionNumber}\\n{Title}\\n{JobCode}\\n{GetEmployeeDisplay()}";
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Return the string for the position in Graphviz format
        /// </summary>
        /// <returns></returns>
        public string GetGraphvizPosition()
        {
            return $"{PositionNumber} [label=\"{GetPositionDisplay()}\" shape=rect]";
        }

        /// <summary>
        /// Return the string for the report to in Graphviz format
        /// </summary>
        /// <returns></returns>
        public string GetGraphvizPositionReportTo()
        {
            if (ReportsTo > 0)
            {
                return $"{PositionNumber} -> {ReportsTo} [arrowhead=none]";
            }
            return string.Empty;
        }
        #endregion
    }
}
