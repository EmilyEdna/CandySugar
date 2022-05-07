using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Common.DTO
{
    public class CandyGalCoverDto
    {
        public string CoverBase64 { get; set; }
        public string Cover { get; set; }
        public byte[] Source { get; set; }
        public void ConvertToBase64()
        {
            this.CoverBase64 = Convert.ToBase64String(Source);
        }

        public void ConvertToBytes()
        {
            this.Source = Convert.FromBase64String(CoverBase64);
        }
    }
}
