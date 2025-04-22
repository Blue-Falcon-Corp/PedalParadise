namespace PedalParadise.Models
{
    public class Employee : User
    {
        public string EmployeeID { get; set; }
        public double Salary { get; set; }
        public int ManagerID { get; set; }  
        //consultar por managerID and userID references
    }
    
}
