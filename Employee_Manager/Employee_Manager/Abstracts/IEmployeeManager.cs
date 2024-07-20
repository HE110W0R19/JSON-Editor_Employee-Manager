namespace Employee_Manager.Abstracts
{
    public interface IEmployeeManager
    {
        void AddEmployee(string firstName, string lastName, decimal salary);
        void DeleteEmployee(int id);
        void GetAllEmployees();
        void GetEmployee(int id);
        void ReadEmployeesFromFile();
        void UpdateEmployee(int id, string? firstName = null, string? lastName = null, decimal? salary = null);
    }
}