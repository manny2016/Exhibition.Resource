

namespace Exhibition.Core
{
    using Dapper;
    using Models = Exhibition.Core.Models;
    using Entities = Exhibition.Core.Entities;
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using Exhibition.Core.Models;
    using Newtonsoft.Json.Linq;

    public static class ModelExtension
    {
        public static DynamicParameters GenernateParameters(this IBaseTerminal terminal)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@name", terminal.Name);
            parameters.Add("@type", terminal.Type);
            parameters.Add("@description", terminal.Description);
            parameters.Add("@settings", terminal.GetSettings());
            return parameters;
        }

        public static string GenernateInsertScript(this IBaseTerminal terminal)
        {
            return @"INSERT INTO terminal(Name,Type,Description,Settings) 
VALUES(@name,@type,@description,@settings);";
        }

        public static string GenernateUpdateScript(this IBaseTerminal terminal)
        {
            return @"UPDATE terminal SET Type=@type, Description=@description,Settings=@settings WHERE Name=@name;";
        }

        public static string GenernateDeleteScript(this IBaseTerminal terminal)
        {
            return @"DELETE FROM Terminal WHERE Name=@name";
        }

        public static IBaseTerminal Convert(this Entities::Terminal terminal)
        {
            if (terminal == null) return null;

            switch (terminal.Type)
            {
                case TerminalTypes.MediaPlayer:
                    return new MediaPlayerTerminal()
                    {
                        Name = terminal.Name,
                        Description = terminal.Description,
                        Settings = terminal.Settings.DeserializeToObject<MedaiPlayerSettings>()
                    };
                case TerminalTypes.SerialPort:
                    return new SerialPortTerminal()
                    {
                        Name = terminal.Name,
                        Description = terminal.Description,
                        Settings = terminal.Settings.DeserializeToObject<SerialPortSettings>()
                    };
                default:
                    throw new NotSupportedException(terminal.Type.ToString());
            }
        }

        public static DynamicParameters GenernateParameters(this Models::Directive directive)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@name", directive.Name);
            parameters.Add("@description", directive.Description);
            parameters.Add("@target", directive.Terminal.SerializeToJson());
            parameters.Add("@defaultwindow", directive.DefaultWindow == null ? "{}" : directive.DefaultWindow.SerializeToJson());
            parameters.Add("@resources", directive.Resources == null ? "[]" : directive.Resources.SerializeToJson());
            return parameters;
        }

        public static string GenernateInsertScript(this Models::Directive directive)
        {
            return @"
INSERT INTO Directive(Name,Description,Target,DefaultWindow,Resources)
VALUES(@name,@description,@target,@defaultwindow,@resources);
";
        }

        public static string GenernateUpdateScript(this Models::Directive directive)
        {
            return @"
UPDATE Directive SET Target=@target,Defaultwindow=@defaultwindow,Description=@description,Resources=@resources
WHERE Name = @name;
";
        }

        public static Models::Directive Convert(this Entities::Directive directive)
        {
            //var terminal = terminals.First(o => o.Ip.Equals(directive.TargetIp)).Convert();
            return new Core.Models.Directive()
            {
                Description = directive.Description,
                Name = directive.Name,
                DefaultWindow = directive.DefaultWindow.DeserializeToObject<Window>(),
                Resources = directive.Resources.DeserializeToObject<Resource[]>(),
                Terminal = directive.Target.TrytoGetInstance(),

            };
        }

        public static string GenernateDeleteScript(this Models::Directive directive)
        {
            return "DELETE FROM Directive WHERE Name = @name";
        }
        public static string GenernateConditionExpression(this Models::SQLiteQueryFilter<string> filter)
        {
            if (filter == null) return " WHERE 1=1 ";
            var conditions = new List<string>() { " WHERE 1=1 " };
            if (filter.Keys != null && filter.Keys.Length > 0)
            {
                conditions.Add($"({filter.PrimaryKey} IN ({string.Join(",", filter.Keys.Select(o => string.Format("'{0}'", o)))}))");
            }
            if (filter.TerminalTypes != null && filter.TerminalTypes.Length > 0)
            {
                conditions.Add($"(Type IN ({string.Join(",", filter.TerminalTypes.Select(o => string.Format("{0}", (int)o)))}))");
            }
            if (filter.DirectiveTypes != null && filter.DirectiveTypes.Length > 0)
            {
                conditions.Add($"(Type IN ({string.Join(",", filter.DirectiveTypes.Select(o => string.Format("{0}", (int)o)))}))");
            }
            if (string.IsNullOrEmpty(filter.Search) == false)
            {
                conditions.Add($"(Name LIKE '%{filter.Search}%' OR Description LIKE '%{filter.Search}%')");
            }
            return string.Join(" AND ", conditions);
        }

        public static Models::OptionModel Convert(this IBaseTerminal terminal)
        {
            return new Models::OptionModel()
            {
                Key = terminal.Name,
                Text = $"{terminal.Name}"
            };
        }
        public static Models::OptionModel Convert(this Models::Resource resource)
        {
            return new Models::OptionModel()
            {
                Key = resource.FullName,
                Text = resource.FullName
            };
        }
        public static Models::OptionModel Convert(this Models::Window window, string ip)
        {
            return new Models::OptionModel()
            {
                Key = $"{ip}-{window.Id}",
                Text = $"{window.Id}|{window.Location.X} * {window.Location.Y}(x * y) | {window.Size.Width} * {window.Size.Height} (width * height)"
            };
        }

        public static IBaseTerminal TrytoGetInstance(this string text)
        {
            var directly = "$.type";
            var jObject = JObject.Parse(text.ToLower());
            var intType = jObject.SelectToken(directly).Value<int?>();
            var type = intType == null ? TerminalTypes.NotSupport : (TerminalTypes)intType; ;
            if (type == TerminalTypes.NotSupport) throw new ArgumentNullException("Can't get terminal type,please check json string");
            switch (type)
            {
                case TerminalTypes.MediaPlayer:
                    return text.DeserializeToObject<MediaPlayerTerminal>();
                case TerminalTypes.SerialPort:
                    return text.DeserializeToObject<SerialPortTerminal>();
            }
            return null;
        }
    }
}
