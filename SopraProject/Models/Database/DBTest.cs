using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
namespace SopraProject
{
    public class Truc
    {
        [Key]
        public int TrucID {get;set;}
        public string UnFieldEnPlusMDR {get; set;}
        public int ID {get;set;}

    }
    public class Machin
    {
        [Key]
        public int MachinID { get; set; }
        public virtual List<Truc> DesTrucs { get; set; }
        public string UnMachin { get; set; }
        public Machin()
        {
            DesTrucs = new List<Truc>();
        }
    }

    public class DBTestContext2 : DbContext
    {
        public DbSet<Machin> Machins { get; set; }
        public DBTestContext2() : base() 
        {
        }
        public DBTestContext2(System.Data.Common.DbConnection con) : base(con, true) {
        }
        public DBTestContext2(string str) : base(str) {
        }
        /*public DBTestContext2(SQLiteConnection con) : base(con, true) {
        }*/
    }
}

