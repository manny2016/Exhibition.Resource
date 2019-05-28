

namespace Exhibition.Core
{
    using Dapper;
    using Models = Exhibition.Core.Models;
    using Entities = Exhibition.Core.Entities;
    public static class ModelExtension
    {
        public static DynamicParameters GenernateParameters(this Models::Terminal terminal)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ip", terminal.Ip);
            parameters.Add("@name", terminal.Name);
            parameters.Add("@schematic", terminal.Schematic);
            parameters.Add("@windows", terminal.Windows == null
                ? "[]"
                : terminal.Windows.SerializeToJson());
            parameters.Add("@description", terminal.Description);
            parameters.Add("@endpoint", terminal.Endpoint);
            return parameters;
        }

        public static string GenernateInsertScript(this Models::Terminal terminal)
        {
            return @"
INSERT INTO terminal(Ip,Name,Description,Schematic,Endpoint,Windows) 
VALUES(@ip,@name,@description,@schematic,@endpoint,@windows);";
        }

        public static string GenernateUpdateScript(this Models::Terminal terminal)
        {
            return @"UPDATE terminal SET Name=@name,Description=@description,
Schematic=@schematic,Endpoint=@endpoint,Windows=@windows WHERE Ip=@ip;";
        }

        public static string GenernateDeleteScript(this Models::Terminal terminal)
        {
            return @"DELETE FROM terminal WHERE Ip=@ip";
        }

        public static Models::Terminal Convert(this Entities::Terminal terminal)
        {
            if (terminal == null) return null;
            return new Models::Terminal()
            {
                Ip = terminal.Ip,
                Description = terminal.Description,
                Endpoint = terminal.Endpoint,
                Name = terminal.Name,
                Schematic = terminal.Schematic,
                Windows = terminal.Windows.DeserializeToObject<Models::Window[]>()
            };
        }

        public static DynamicParameters GenernateParameters(this Models::Directive directive)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@type", (int)directive.Type);
            parameters.Add("@name", directive.Name);
            parameters.Add("@target", directive.Terminal.Ip);
            parameters.Add("@window", directive.Window);
            parameters.Add("@description", directive.Description);
            parameters.Add("@context", directive.Resources.SerializeToJson());
            return parameters;
        }

        public static string GenernateInsertScript(this Models::Directive directive)
        {
            return @"
INSERT INTO Directive(Name,Target,Window,Type,Description,Context)
VALUES(@name,@target,@window,@type,@description,@context);
";
        }

        public static string GenernateUpdateScript(this Models::Directive directive)
        {
            return @"
UPDATE Directive SET Target=@target,Window=@window,Type=@type,Description=@description,Context=@context
WHERE Name = @name;
";
        }

        public static Models::Directive Convert(this Entities::Directive directive)
        {
            return new Core.Models.Directive()
            {
                Description = directive.Description,
                Name = directive.Name,
                Resources = directive.Context.DeserializeToObject<Models::Resource[]>(),
                Terminal = new Core.Models.Terminal() { Ip = directive.Target },
                Type = directive.Type,
                Window = directive.Window
            };
        }

        public static string GenernateDeleteScript(this Models::Directive directive)
        {
            return "DELETE FROM Directive WHERE Name = @name";
        }
        public static string GenernateWhereCase(this Models::SQLiteQueryFilter filter)
        {
            if (filter == null) return " WHERE 1=1 ";
            if (string.IsNullOrEmpty(filter.Value)) return " WHERE 1=1 ";
            return $" WHERE {filter.Name} LIKE '%{filter.Value.Replace("'","''")}%'";
        }
    }
}
