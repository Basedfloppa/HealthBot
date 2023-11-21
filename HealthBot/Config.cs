using LinqToDB.Configuration;
using LinqToDB;

namespace Configuration
{
    class Config
    {
        public static string token = "5668094294:AAF9E5nuBo5kmEBplHeQJTngeMJSgxkhw_0";
    }
    public class ConnectionStringSettings : IConnectionStringSettings
    {
        public string ConnectionString { get; set; }
        public string Name { get; set; }
        public string ProviderName { get; set; }
        public bool IsGlobal => false;
    }

    public class MySettings : ILinqToDBSettings
    {
        public IEnumerable<IDataProviderSettings> DataProviders => Enumerable.Empty<IDataProviderSettings>();
        public string DefaultConfiguration => "PostgreSQL";
        public string DefaultDataProvider => "PostgreSQL";

        public IEnumerable<IConnectionStringSettings> ConnectionStrings
        {
            get
            {
                // note that you can return multiple ConnectionStringSettings instances here
                yield return
                    new ConnectionStringSettings
                    {
                        Name = "HealthBot",
                        ProviderName = ProviderName.PostgreSQL,
                        ConnectionString = "Server=localhost;Port=5432;Database=HealthBot;User Id=postgres;Password=1940-2010;"
                    };
            }
        }
    }
}