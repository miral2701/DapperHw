using Dapper;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace DapperHw
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TaskRepository taskRepository = new TaskRepository(@"Server=(localdb)\MSSQLLocalDB;Database=testdb;Trusted_Connection=True;");
            //taskRepository.AddTask(new TaskModel {Title="Clean",Description="On sunday",DueDate=DateTime.Now.AddDays(3),IsCompleted = false,
            //});
            // taskRepository.DeleteTask(1);
            //taskRepository.AddTask(new TaskModel
            //{
            //    Title = "Do homework",
            //    Description = "Till monday",
            //    DueDate = DateTime.Now.AddDays(4),
            //    IsCompleted = false,
            //});

            //taskRepository.AddTask(new TaskModel
            //{
            //    Title = "Buy haoween decorations",
            //    Description = "-2 lamps -1skeleton",
            //    DueDate = DateTime.Now.AddDays(7),
            //    IsCompleted = false,
            //});

            var task = taskRepository.GetTask(2);

            var task2 = new TaskModel()
            {
                Id = 3,
                Title = "Do homework",
                Description = "Till monday",
                DueDate = DateTime.Now.AddDays(4),
                IsCompleted = true,
            };

            taskRepository.UpdateTask(task2);
        }
    }



    public class TaskModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
    }

    class TaskRepository
    {
        private readonly string connectionString;
        public TaskRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void AddTask(TaskModel task)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Execute("INSERT INTO Tasks (Title,Description,DueDate,IsCompleted) VALUES (@Title,@Description,@DueDate,@IsCompleted)", task);
            }
        }

        public TaskModel GetTask(int taskid)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<TaskModel>("SELECT * FROM Tasks Where Id=@Id", new { Id = taskid });
            }
        }

        public void UpdateTask(TaskModel task)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Execute("UPDATE Tasks SET Title=@Title,Description=@Description,DueDate=@DueDate,IsCompleted=@IsCompleted WHERE Id=@Id", task);
            }
        }

        public void DeleteTask(int taskid)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Execute("DELETE FROM Tasks WHERE Id=@Id", new { Id = taskid });
            }
        }
    }
}
