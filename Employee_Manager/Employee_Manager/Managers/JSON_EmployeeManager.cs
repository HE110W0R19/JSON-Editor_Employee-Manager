using Employee_Manager.Models;
using Newtonsoft.Json;

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

        public void ReadEmployeesFromFile()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _employees = JsonConvert.DeserializeObject<List<Employee>>(json) ?? new List<Employee>();

                if (_employees.Count < 1) 
                {
                    Console.WriteLine("ERROR: Incorrect data format in the file (see README.md)");    
                }
            }
            else
            {
                Console.WriteLine("ERROR: Invalid path or file!");
            }
        }
    }
}
