using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CandySugar.Xam.Common.Entity
{
    public class BasicEntity
    {
        [PrimaryKey]
        public string PId { get; set; }
        public long Span { get; set; }
        public void InitProperty()
        {
            this.PId = Guid.NewGuid().ToString();
            this.Span = DateTime.Now.Ticks;
        }
    }
}
