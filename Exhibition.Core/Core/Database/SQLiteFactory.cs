

namespace Exhibition.Core
{
    using System;
    using System.Data.SQLite;
    using System.IO;
    using System.Data;
    using Dapper;
    
    using System.Linq;
    public class SQLiteFactory
    {
        const string ConnectionString = "Data Source={0};Version=3;Pooling=True;Max Pool Size=100;";
        const string datasouece = "exhibition.sqlite";
        static SQLiteFactory()
        {
            Initialize();
        }
        static void Initialize()
        {
            var fileInfo = new FileInfo(GetDataSource());
            if (!fileInfo.Exists)
            {
                SQLiteConnection.CreateFile(GetDataSource());
                using (var database = Genernate())
                {
                    database.Execute(GenernateTableScraptforTerminal());
                    database.Execute(GenernateTableScriptforDirective());
                }
            }
        }
        static string GenernateSQLiteConnectionString(string name = datasouece)
        {
            return string.Format(ConnectionString, GetDataSource(name));
        }
        static string GetDataSource(string name = datasouece)
        {
            var directory = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "data"))
                .CreateIfNotExists();
            return Path.Combine(directory.FullName, name);
        }
        public static IDbConnection Genernate(string name = datasouece)
        {
            return new SQLiteConnection(GenernateSQLiteConnectionString(name));
        }
        static string GenernateTableScraptforTerminal()
        {
            return @"
CREATE TABLE  IF NOT EXISTS Terminal (
    Ip    varchar(20),
	Name  varchar(50),
	Description   varchar(200),
	Schematic varchar(200),
	Endpoint  varchar(200),
	Windows   TEXT,
	PRIMARY KEY(Ip)
);";
        }
        static string GenernateTableScriptforDirective()
        {
            return @"
CREATE TABLE IF NOT EXISTS Directive (
    Name  varchar(100),
    TargetIp VARCHAR(20),
    Window INTEGER,
	Type  INTEGER,
	Description   varchar(100),
	Context   TEXT,
	PRIMARY KEY(Name),
        FOREIGN KEY (TargetIp) REFERENCES Terminal(Ip)
);            ";
        }
    }
}
