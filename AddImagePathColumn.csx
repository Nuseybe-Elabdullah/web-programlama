using Npgsql;

var connectionString = "Host=localhost;Port=5432;Database=GymManagementDB;Username=postgres;Password=123456";

try
{
    using var connection = new NpgsqlConnection(connectionString);
    connection.Open();
    
    // Check if column already exists
    var checkCmd = new NpgsqlCommand(@"
        SELECT COUNT(*) 
        FROM information_schema.columns 
        WHERE table_name='Services' AND column_name='ImagePath'", connection);
    
    var exists = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0;
    
    if (!exists)
    {
        // Add the column
        var addCmd = new NpgsqlCommand(@"
            ALTER TABLE ""Services"" 
            ADD COLUMN ""ImagePath"" VARCHAR(500) NULL", connection);
        
        addCmd.ExecuteNonQuery();
        Console.WriteLine("ImagePath column added successfully!");
    }
    else
    {
        Console.WriteLine("ImagePath column already exists.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
