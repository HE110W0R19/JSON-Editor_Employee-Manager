using Employee_Manager.Abstracts;
using Employee_Manager.Models;
using Newtonsoft.Json;

namespace Employee_Manager.Managers.JSON_Manager
{
    public class JSON_EmployeeManager : IEmployeeManager
    {
        private readonly string _filePath;
        private List<Employee> _employees;

        // Конструктор, инициализирующий путь к файлу и загружающий данные
        public JSON_EmployeeManager(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            _employees = new List<Employee>();
            ReadEmployeesFromFile();
        }

        /// <summary>
        /// Метод для загрузки данных сотрудников из JSON-файла
        /// Если файл существует, данные считываются и десериализуются в список сотрудников
        /// Если файл не существует, инициализируется пустой список сотрудников
        /// </summary>
        public void ReadEmployeesFromFile()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _employees.AddRange(JsonConvert.DeserializeObject<List<Employee>>(json) ?? _employees);

                if (_employees.Count < 1)
                {
                    Console.WriteLine($"{DateTime.Now.ToLongTimeString()} ERROR: Incorrect data format in the file (see README.md)");
                }
            }
            else
            {
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()} ERROR: Invalid path or file!");
            }

            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} SUCCESS: Employees was readed from {Path.GetFileName(_filePath)}");
        }

        /// <summary>
        /// Метод для сохранения данных сотрудников в JSON-файл
        /// Список сотрудников сериализуется в JSON-формат и записывается в файл
        /// </summary>
        private void SaveEmployees()
        {
            try
            {
                var json = JsonConvert.SerializeObject(_employees, Formatting.Indented);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION: Something is wrong when saving\nMessgae: {ex.Message}");
                return;
            }
        }

        /// <summary>
        /// Метод для добавления нового сотрудника
        /// Создается новый сотрудник с автоматически сгенерированным Id (максимальный Id из существующих + 1)
        /// Добавляется новый сотрудник в список и данные сохраняются в файл
        /// </summary>
        /// <param name="firstName">Имя</param>
        /// <param name="lastName">Фамилия</param>
        /// <param name="salary">ЗП ($/час)</param>
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

            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} SUCCESS: Employee {firstName} {lastName} was added!");
        }

        /// <summary>
        /// Метод для получения информации о сотруднике по Id
        /// Находит сотрудника по Id и выводит его данные
        /// Если сотрудник не найден, выводится сообщение об ошибке
        /// </summary>
        /// <param name="id">Id сотрудника</param>
        public void GetEmployee(int id)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()} ERROR: Employee with Id {id} not found.");
                return;
            }

            Console.WriteLine($"\nId = {employee.Id}, FirstName = {employee.FirstName}, LastName = {employee.LastName}, SalaryPerHour = {employee.SalaryPerHour}");
        }

        /// <summary>
        /// Метод для удаления сотрудника по Id
        /// Находит сотрудника по Id и удаляет его из списка
        /// Если сотрудник не найден, выводится сообщение об ошибке
        /// </summary>
        /// <param name="id">Id сотрудника</param>
        public void DeleteEmployee(int id)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()} ERROR: Employee with Id {id} not found.");
                return;
            }

            _employees.Remove(employee);
            SaveEmployees();

            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} SUCCESS: Employee with Id:{id} was deleted!");
        }

        /// <summary>
        /// Метод для обновления данных сотрудника
        /// Находит сотрудника по Id и обновляет его данные, если сотрудник существует
        /// Если указаны новые значения для полей, они обновляются у соответствующего сотрудника
        /// Если сотрудник не найден, выводится сообщение об ошибке
        /// </summary>
        /// <param name="id"> Id сотрудника</param>
        /// <param name="firstName">Имя</param>
        /// <param name="lastName">Фамилия</param>
        /// <param name="salary">ЗП ($/час)</param>
        public void UpdateEmployee(int id, string? firstName = null, string? lastName = null, decimal? salary = null)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()} ERROR: Employee with Id {id} not found.");
                return;
            }

            if (!string.IsNullOrEmpty(firstName))
                employee.FirstName = firstName;

            if (!string.IsNullOrEmpty(lastName))
                employee.LastName = lastName;

            if (salary.HasValue)
                employee.SalaryPerHour = salary.Value;

            SaveEmployees();

            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} SUCCESS: Employee exchanged to [{firstName}, {lastName}, {salary}]");
        }

        /// <summary>
        /// Метод для получения списка всех сотрудников
        /// Перебирает список сотрудников и выводит информацию о каждом из них
        /// </summary>
        public void GetAllEmployees()
        {
            foreach (var employee in _employees)
            {
                Console.WriteLine($"Id = {employee.Id}, FirstName = {employee.FirstName}, LastName = {employee.LastName}, SalaryPerHour = {employee.SalaryPerHour}");
            }
        }
    }
}
