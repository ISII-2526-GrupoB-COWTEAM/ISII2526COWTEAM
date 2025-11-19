namespace AppForSEII2526.UT {
    public class AppForSEII25264SqliteUT
    {
        protected readonly DbConnection _connection;
        protected readonly ApplicationDbContext _context;
        protected readonly DbContextOptions<ApplicationDbContext> _contextOptions;

        protected ApplicationDbContext CreateContext() => new(_contextOptions);


        void Dispose() => _connection.Dispose();
        public AppForSEII25264SqliteUT() {
            
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            
            _contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_connection).Options;

            
            _context = new ApplicationDbContext(_contextOptions);
            if (_context.Database.EnsureCreated()) {
                using var viewCommand = _context.Database.GetDbConnection().CreateCommand();
                viewCommand.CommandText = @"
                CREATE VIEW AllResources AS
                SELECT Name
                FROM Devices;";
                viewCommand.ExecuteNonQuery();
            }
        }
    }
}