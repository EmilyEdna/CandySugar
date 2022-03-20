using CandySugar.Common;
using CandySugar.Common.DTO;
using CandySugar.Common.Entity.Model;
using CandySugar.Core.Service;
using XExten.Advance.LinqFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Core.ServiceImpl
{
    public class Ax : SqlSugarDbContext, IAx
    {
        public byte[] GetCover(string input)
        {
            var db = Db;
            var ck = db.Queryable<CandyGalCover>().First(t => t.Cover == input);
            if (ck == null) return new byte[0];
            return Convert.FromBase64String(db.Queryable<CandyGalCover>()
                .Where(t => t.Cover == input)
                .Select(t => t.CoverBase64).First());
        }

        public void SaveCover(CandyGalCoverDto input)
        {
            var db = Db;
            var ck = db.Queryable<CandyGalCover>().First(t => t.Cover == input.Cover);
            if (ck == null)
            {
                input.ConvertToBase64();
                var entity = input.ToMapest<CandyGalCover>();
                db.Insertable(entity).CallEntityMethod(t => t.Create()).ExecuteCommand();
            }
        }
    }
}
