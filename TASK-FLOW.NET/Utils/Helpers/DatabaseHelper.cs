using Npgsql;

namespace TASK_FLOW.NET.Utils.Helpers
{
    public static class DatabaseHelper
    {
        public static async Task WaitForDatabaseAsync(string connectionString, int maxRetries = 10, TimeSpan? delay = null)
        {
            delay ??= TimeSpan.FromSeconds(5);

            for (int i = 1; i <= maxRetries; i++)
            {
                try
                {
                    using var connection = new NpgsqlConnection(connectionString);
                    await connection.OpenAsync();
                    Console.WriteLine("Database is ready to use");
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Attempt {i}/{maxRetries} fails: Retry in {delay.Value.TotalSeconds} secons.. {ex.Message}");
                    await Task.Delay(delay.Value);
                }
            }
            throw new Exception("Could not connect to database");
        }
    }
}
