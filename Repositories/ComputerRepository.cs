using LabManager.Database;
using LabManager.Models;
using Microsoft.Data.Sqlite;
using Dapper;


namespace LabManager.Repositories;

class ComputerRepository
{
    private readonly DatabaseConfig _databaseConfig;

    public ComputerRepository(DatabaseConfig databaseConfig) {
        _databaseConfig = databaseConfig;
    }

    public IEnumerable<Computer> GetAll()
    {  
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();

        var computers = connection.Query<Computer>("SELECT * FROM Computers");

        return computers;
    }


    public Computer Save(Computer computer)
    {
        var connection = new SqliteConnection(_databaseConfig.ConnectionString);

        connection.Open();

        connection.Execute("INSERT INTO Computers VALUES(@Id, @Ram, @Processor)", computer);

        connection.Close();

        return computer;
    }


    public Computer GetById(int id)
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
    
        var computer = connection.QuerySingle<Computer>("SELECT * FROM Computers WHERE ID = (@Id)", new { Id = id });

        return computer;
    }

    public Computer Update(Computer computer)
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        
        connection.Execute("Update Computers SET ram = (@Ram), processor = (@Processor) WHERE id = (@Id)", computer); 
        
        return computer;
    }


    public void Delete(int id)
    {
        var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();

        connection.Execute("DELETE FROM Computers WHERE ID = (@Id", new { Id = id});

        connection.Close();
    }

    public bool ExistsById(int id)
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();

        var result = connection.ExecuteScalar<bool>("SELECT count(id) FROM Computers WHERE ID = @Id;" , new { Id = id });

        return result;
    }

    //Converter um Reader para um computador
    private Computer readerToComputer(SqliteDataReader reader) 
    {
       var computer = new Computer(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
    
       return computer;  
    }
}