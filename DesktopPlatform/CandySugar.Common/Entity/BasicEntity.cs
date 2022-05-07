using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CandySugar.Common.Entity
{
    public class BasicEntity
    {
        [SugarColumn(IsPrimaryKey = true)]
        public Guid PId { get; set; }
        [SugarColumn(ColumnDataType = "bigint", IsNullable = true)]
        public long Span { get; set; }
        public void Create()
        {
            this.PId = Guid.NewGuid();
            this.Span = DateTime.Now.Ticks;
        }
    }
}
