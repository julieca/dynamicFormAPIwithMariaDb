using LinqToDB;
using LinqToDB.Configuration;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Medico.Service.DynamicForm.Infrastructure
{
    public class DbReadConfig : DbBaseConfiguration
    {
        public DbReadConfig(IConfiguration config) : base(config)
        { }
        public override string DefaultConfiguration => "ReadOnlyConnection"; 
    }

    public class DbDefaultConfig : DbBaseConfiguration
    {
        public DbDefaultConfig(IConfiguration config) : base(config)
        {

        }

        public override string DefaultConfiguration => "DefaultConnection"; 
    }

    public class DbBaseConfiguration : ILinqToDBSettings
    {
        List<IConnectionStringSettings> _connectionStrings;

        public DbBaseConfiguration(IConfiguration config)
        {

            _connectionStrings = new List<IConnectionStringSettings>() {
                new ConnectionStringSettings
                        {
                            Name = "ReadOnlyConnection", // This is configuration name, you pass it to DataConnection constructor
                            ProviderName = ProviderName.MySql, // here we are setting database we are working with
                            ConnectionString = config["Data:ReadOnlyConnection:ConnectionString"]
                        },
                new ConnectionStringSettings
                        {
                            Name = "DefaultConnection", // This is configuration name, you pass it to DataConnection constructor
                            ProviderName = ProviderName.MySql, // here we are setting database we are working with
                            ConnectionString = config["Data:DefaultConnection:ConnectionString"]
                        }
            };
        }


        public IEnumerable<IDataProviderSettings> DataProviders
        {
            get { yield break; }
        }

        public virtual string DefaultConfiguration => "DefaultConnection";

        public string DefaultDataProvider => ProviderName.MySql;

        public IEnumerable<IConnectionStringSettings> ConnectionStrings => _connectionStrings.AsEnumerable();
    }

    public class ConnectionStringSettings : IConnectionStringSettings
    {
        public string ConnectionString { get; set; }
        public string Name { get; set; }
        public string ProviderName { get; set; }
        public bool IsGlobal => false;
    }
}
