using CandySugar.Xam.Common.Entity.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Xam.Core.Service
{
    public interface IXSLiShi
    {
        Task Insert(XS_LiShi input);

        Task Update(XS_LiShi input);

        Task InsertOrUpdate(XS_LiShi input);

        Task<bool> CheckFirst(XS_LiShi input);
    }
}
