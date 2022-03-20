using CandySugar.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Core.Service
{
    public interface IAx
    {
        void SaveCover(CandyGalCoverDto input);
        byte[] GetCover(string input);
    }
}
