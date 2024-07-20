namespace Employee_Manager.Managers.JSON_Manager
{
    public class JSON_EmployeeManager_Builder
    {
        private readonly string _filePath;
        private readonly JSON_EmployeeManager _manager;

        /// <summary>
        /// Конструктор принимает путь к файлу и создает экземпляр EmployeeManager
        /// </summary>
        /// <param name="filePath">путь к файлу</param>
        public JSON_EmployeeManager_Builder(string filePath)
        {
            _filePath = filePath;
            _manager = new JSON_EmployeeManager(filePath);
        }

        /// <summary>
        /// Метод для выполнения операций на основе аргументов командной строки
        /// </summary>
        /// <param name="args">аргументы командной строки</param>
        public void Execute(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()} ERROR: No arguments provided.");
                return;
            }

            switch (args[0])
            {
                case "-add":
                    AddEmployee(args);
                    break;

                case "-update":
                    UpdateEmployee(args);
                    break;

                case "-get":
                    GetEmployee(args);
                    break;

                case "-delete":
                    DeleteEmployee(args);
                    break;

                case "-getall":
                    GetAllEmployees();
                    break;

                default:
                    Console.WriteLine($"{DateTime.Now.ToLongTimeString()} ERROR: Unknown command.");
                    break;
            }
        }

        /// <summary>
        /// Метод для добавления нового сотрудника
        /// </summary>
        /// <param name="args">аргументы командной строки</param>
        private void AddEmployee(string[] args)
        {
            var firstName = args[1].Split(' ')[0].Split(':')[1];
            var lastName = args[1].Split(' ')[1].Split(':')[1];
            var salaryStr = args[1].Split(' ')[2].Split(':')[1];

            if (firstName != null && lastName != null && decimal.TryParse(salaryStr, out var salary))
            {
                _manager.AddEmployee(firstName, lastName, salary);
            }
            else
            {
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()} ERROR: Invalid arguments for adding an employee.");
            }
        }

        /// <summary>
        /// Метод для обновления информации о сотруднике
        /// </summary>
        /// <param name="args">аргументы командной строки</param>
        private void UpdateEmployee(string[] args)
        {
            if (int.TryParse(args.FirstOrDefault(arg => arg.StartsWith("Id:"))?.Split(':')[1], out var idToUpdate))
            {
                var firstName = args.FirstOrDefault(arg => arg.StartsWith("FirstName:"))?.Split(':')[1];
                var lastName = args.FirstOrDefault(arg => arg.StartsWith("LastName:"))?.Split(':')[1];
                var salaryStr = args.FirstOrDefault(arg => arg.StartsWith("Salary:"))?.Split(':')[1];

                decimal? salaryToUpdate = null;
                if (salaryStr != null && decimal.TryParse(salaryStr, out var parsedSalary))
                {
                    salaryToUpdate = parsedSalary;
                }

                _manager.UpdateEmployee(idToUpdate, firstName, lastName, salaryToUpdate);
            }
            else
            {
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()} ERROR: Invalid arguments for updating an employee.");
            }
        }

        /// <summary>
        /// Метод для получения информации о сотруднике по Id
        /// </summary>
        /// <param name="args">аргументы командной строки</param>
        private void GetEmployee(string[] args)
        {
            if (int.TryParse(args.FirstOrDefault(arg => arg.StartsWith("Id:"))?.Split(':')[1], out var idToGet))
            {
                _manager.GetEmployee(idToGet);
            }
            else
            {
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()} ERROR: Invalid arguments for getting an employee.");
            }
        }

        /// <summary>
        /// Метод для удаления сотрудника по Id
        /// </summary>
        /// <param name="args">аргументы командной строки</param>
        private void DeleteEmployee(string[] args)
        {
            if (int.TryParse(args.FirstOrDefault(arg => arg.StartsWith("Id:"))?.Split(':')[1], out var idToDelete))
            {
                _manager.DeleteEmployee(idToDelete);
            }
            else
            {
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()} ERROR: Invalid arguments for deleting an employee.");
            }
        }

        /// <summary>
        /// Метод для получения списка всех сотрудников
        /// </summary>
        private void GetAllEmployees()
        {
            _manager.GetAllEmployees();
        }
    }
}
