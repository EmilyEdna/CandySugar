using CandySugar.Common.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.StaticFramework;

namespace CandySugar.Common
{
    public class SqlSugarDbContext
    {
        public SqlSugarScope Db => new(new ConnectionConfig
        {
            DbType = DbType.Sqlite,
            InitKeyType = InitKeyType.Attribute,
            ConnectionString = $"DataSource={DbRoute}",
            IsAutoCloseConnection = true,
        });
        private static string DbRoute => SyncStatic.CreateFile(Path.Combine(SyncStatic.CreateDir(Path.Combine(Environment.CurrentDirectory, "Candy")), "Candy.db"));

        public void InitCandy()
        {
            var Table = this.GetType().Assembly.GetTypes().Where(t => t.IsClass == true).Where(t => t.BaseType == typeof(BasicEntity)).ToArray();
            Db.CodeFirst.InitTables(Table);
        }
    }
}
