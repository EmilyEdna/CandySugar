using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CandySugar.Xam.Common.Entity
{
    public class BasicEntity
    {
        [PrimaryKey]
        public Guid PId { get; set; }
        public long Span { get; set; }
        public void InitProperty()
        {
            this.PId = Guid.NewGuid();
            this.Span = DateTime.Now.Ticks;
        }
    }
}
