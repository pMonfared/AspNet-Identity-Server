﻿namespace ASPNETCoreIdentitySample.ViewModels.Identity.Settings
{
    public class Connectionstrings
    {
        public SqlServer SqlServer { get; set; }
        public Localdb LocalDb { get; set; }
        public Sqlite Sqlite { get; set; }
    }
}