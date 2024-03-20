namespace IFTL.OrgChart.Lib
{
    /// <summary>
    /// Employee
    /// </summary>
    public class Employee
    {
        #region Public Variables
        public int EmployeeId { get; set; }
        public int PositionNumber { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string JobCode { get; set; }
        public int ReportsTo { get; set; }
        public string HomeIncumbent { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Blank Constructor
        /// </summary>
        public Employee()
        {

        }

        /// <summary>
        /// Constructor with Parameters
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="positionNumber"></param>
        /// <param name="name"></param>
        /// <param name="title"></param>
        /// <param name="jobCode"></param>
        /// <param name="reportsTo"></param>
        /// <param name="homeIncumbent"></param>
        public Employee(int employeeId, int positionNumber, string name, string title, string jobCode, int reportsTo, string homeIncumbent)
        {
            EmployeeId = employeeId;
            PositionNumber = positionNumber;
            Name = name;
            Title = title;
            JobCode = jobCode;
            ReportsTo = reportsTo;
            HomeIncumbent = homeIncumbent;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Get If Its Their Home
        /// </summary>
        /// <returns></returns>
        public string GetIfHomeIncumbent()
        {
            if (HomeIncumbent != null)
            {
                return "S";
            }
            else
            {
                return "H";
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Get Displayable Employee
        /// </summary>
        /// <returns></returns>
        public string GetEmployeeDisplay()
        {
            return $"{Name}({GetIfHomeIncumbent()})";
        }
        #endregion
    }
}
