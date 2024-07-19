using Employee_Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Manager.Managers
{
    public class JSON_EmployeeManager
    {
        private readonly string? _filePath;
        private List<Employee>? _employees;

        public JSON_EmployeeManager(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }
    }
}
