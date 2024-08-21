using System.Collections.Generic;

namespace DeptEmailSender.Models
{
    public class Department
    {
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public string DeptInfo { get; set; }
        public int? ParentDepartmentId { get; set; }
        public  ICollection<Department> SubDepartments { get; set; } = new List<Department>();
    }
}
