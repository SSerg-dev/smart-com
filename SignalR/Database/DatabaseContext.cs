﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.Database
{
    class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("DatabaseContext") { }
    }
}
