using Employee_Manager.Managers.JSON_Manager;

class Program
{
    public static void Main(string[] args)
    {
        // Определение относительного пути к файлу employees.json в папке Files
        string relativePath = Path.Combine("Files", "Employees.json");
        // Получение полного пути к файлу
        string filePath = Path.GetFullPath(relativePath);
        // Создание экземпляра EmployeeManagerBuilder
        var builder = new JSON_EmployeeManager_Builder(filePath);
        // Выполнение команд на основе аргументов
        builder.Execute(args);
    }
}