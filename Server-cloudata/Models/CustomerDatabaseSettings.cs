using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server_cloudata.Models
{
    public class CustomerDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string CustomersCollectionName { get; set; } = null!;
    }
}
