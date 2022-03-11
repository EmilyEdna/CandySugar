using CandySugar.Xam.Common.Entity;
using CandySugar.Xam.Common.Entity.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Xam.Common
{
    public class SqliteDbContext
    {
        public static SqliteDbContext Instance => new Lazy<SqliteDbContext>().Value;

        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "sqllite.db3");
        public SQLiteAsyncConnection SqlDb => GetDataBase();
        private SQLiteAsyncConnection GetDataBase() => new SQLiteAsyncConnection(dbPath);
        public async void InitTabel()
        {
            var Table = this.GetType().Assembly.GetTypes().Where(t => t.IsClass == true).Where(t => t.BaseType == typeof(BasicEntity)).ToArray();
            await SqlDb.CreateTablesAsync(CreateFlags.None, Table);
            await SqlDb.CloseAsync();
        }
    }
}
