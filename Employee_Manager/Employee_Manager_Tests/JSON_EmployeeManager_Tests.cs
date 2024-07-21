using Employee_Manager.Managers.JSON_Manager;
using Employee_Manager.Models;
using Newtonsoft.Json;

namespace Employee_Manager_Tests
{
    public class JSON_EmployeeManager_Tests : IDisposable
    {
        private const string TestFilePath = "test_employees.json";
        private readonly JSON_EmployeeManager _manager;
        private readonly JSON_EmployeeManager_Builder _builder;
        public JSON_EmployeeManager_Tests()
        {
            // Создаем тестовый файл перед каждым тестом
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }

            _manager = new JSON_EmployeeManager(TestFilePath);
            _builder = new JSON_EmployeeManager_Builder(TestFilePath);
        }

        [Fact]
        public void AddEmployee_ShouldAddNewEmployee()
        {
            // Arrange
            string firstName = "John";
            string lastName = "Doe";
            decimal salary = 100.50m;

            // Act
            _manager.AddEmployee(firstName, lastName, salary);

            // Assert
            var employees = GetEmployeesFromFile();
            Assert.Single(employees);
            Assert.Equal(firstName, employees[0].FirstName);
            Assert.Equal(lastName, employees[0].LastName);
            Assert.Equal(salary, employees[0].SalaryPerHour);
        }

        [Fact]
        public void AddEmployee_ShouldAssignUniqueId()
        {
            // Arrange
            string firstName1 = "John";
            string lastName1 = "Doe";
            decimal salary1 = 100.50m;

            string firstName2 = "Jane";
            string lastName2 = "Smith";
            decimal salary2 = 120.75m;

            // Act
            _manager.AddEmployee(firstName1, lastName1, salary1);
            _manager.AddEmployee(firstName2, lastName2, salary2);

            // Assert
            var employees = GetEmployeesFromFile();
            Assert.Equal(2, employees.Count);
            Assert.Equal(1, employees[0].Id);
            Assert.Equal(2, employees[1].Id);
        }

        [Fact]
        public void Execute_GetEmployeeCommand_ShouldOutputEmployee()
        {
            // Arrange
            _builder.Execute(new string[] { "-add", "FirstName:John", "LastName:Doe", "Salary:100.50" });
            string[] args = { "-get", "Id:1" };

            // Act
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                _builder.Execute(args);
                sw.Flush();

                // Assert
                var expectedOutput = "Id = 1, FirstName = John, LastName = Doe, SalaryPerHour = 100.50" + Environment.NewLine;
                Assert.Equal(expectedOutput, sw.ToString());
            }
        }

        [Fact]
        public void Execute_DeleteEmployeeCommand_ShouldDeleteEmployee()
        {
            // Arrange
            _builder.Execute(new string[] { "-add", "FirstName:John", "LastName:Doe", "Salary:100.50" });
            string[] args = { "-delete", "Id:1" };

            // Act
            _builder.Execute(args);

            // Assert
            var employees = GetEmployeesFromFile();
            Assert.Empty(employees);
        }

        [Fact]
        public void Execute_GetAllEmployeesCommand_ShouldOutputAllEmployees()
        {
            // Arrange
            _builder.Execute(new string[] { "-add", "FirstName:John", "LastName:Doe", "Salary:100.50" });
            _builder.Execute(new string[] { "-add", "FirstName:Jane", "LastName:Smith", "Salary:120.75" });
            string[] args = { "-getall" };

            // Act
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                _builder.Execute(args);
                sw.Flush();

                // Assert
                var expectedOutput = 
                    "Id = 1, FirstName = John, LastName = Doe, SalaryPerHour = 100.50" + Environment.NewLine +
                    "Id = 2, FirstName = Jane, LastName = Smith, SalaryPerHour = 120.75" + Environment.NewLine;
                Assert.Equal(expectedOutput, sw.ToString());
            }
        }

        // Вспомогательный метод для чтения сотрудников из файла
        private List<Employee> GetEmployeesFromFile()
        {
            var json = File.ReadAllText(TestFilePath);
            return JsonConvert.DeserializeObject<List<Employee>>(json);
        }

        // Удаление тестового файла после каждого теста
        public void Dispose()
        {
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }
    }
}