using Employee_Manager.Abstracts;
using Employee_Manager.Models;
using Newtonsoft.Json;

namespace Employee_Manager.Managers
{
    public class JSON_EmployeeManager : IEmployeeManager
    {
        private readonly string _filePath;
        private List<Employee> _employees;

        public JSON_EmployeeManager(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            _employees = new List<Employee>();
            ReadEmployeesFromFile();
        }

        public void ReadEmployeesFromFile()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _employees.AddRange(JsonConvert.DeserializeObject<List<Employee>>(json) ?? _employees);

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

        private void SaveEmployees()
        {
            var json = JsonConvert.SerializeObject(_employees, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        public void AddEmployee(string firstName, string lastName, decimal salary)
        {
            int newId = 1;

            if (_employees.Any() == true)
            {
                newId = _employees.Max(e => e.Id) + 1;
            }

            var newEmployee = new Employee
            {
                Id = newId,
                FirstName = firstName,
                LastName = lastName,
                SalaryPerHour = salary
            };

            _employees.Add(newEmployee);
            SaveEmployees();

            Console.WriteLine($"SUCCESS: Employee {firstName} {lastName} was added!");
        }

        public void GetEmployee(int id)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                Console.WriteLine($"ERROR: Employee with Id {id} not found.");
                return;
            }

            Console.WriteLine($"\nId = {employee.Id}, FirstName = {employee.FirstName}, LastName = {employee.LastName}, SalaryPerHour = {employee.SalaryPerHour}");
        }

        public void DeleteEmployee(int id)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                Console.WriteLine($"ERROR: Employee with Id {id} not found.");
                return;
            }

            _employees.Remove(employee);
            SaveEmployees();

            Console.WriteLine($"SUCCESS: Employee with Id:{id} was deleted!");
        }

        public void UpdateEmployee(int id, string? firstName = null, string? lastName = null, decimal? salary = null)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                Console.WriteLine($"ERROR: Employee with Id {id} not found.");
                return;
            }

            if (!string.IsNullOrEmpty(firstName))
                employee.FirstName = firstName;

            if (!string.IsNullOrEmpty(lastName))
                employee.LastName = lastName;

            if (salary.HasValue)
                employee.SalaryPerHour = salary.Value;

            SaveEmployees();

            Console.WriteLine($"SUCCESS: Employee exchanged to [{firstName}, {lastName}, {salary}]");
        }

        public void GetAllEmployees()
        {
            foreach (var employee in _employees)
            {
                Console.WriteLine($"Id = {employee.Id}, FirstName = {employee.FirstName}, LastName = {employee.LastName}, SalaryPerHour = {employee.SalaryPerHour}");
            }
        }
    }
}
