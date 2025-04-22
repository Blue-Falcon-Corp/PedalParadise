using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedalParadise.Models
{
    public class Employee : User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string EmployeeID { get; set; }
        public double Salary { get; set; }
        public int ManagerID { get; set; }  
        //consultar por managerID and userID references
    }
    
}
