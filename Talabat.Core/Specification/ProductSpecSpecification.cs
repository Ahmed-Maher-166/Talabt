using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specification
{
    public class ProductSpecSpecification
    {
        public string? Sort { get; set; }
        public int? TypeId { get; set; }
        public int? BrandId { get; set; }
        public string? Name { get; set; }
        public int Index { get; set; } = 1;
        private byte pageSize;
        public byte PageSize
        {
            get { return pageSize; }
            set { pageSize = (byte)(value > 10 ? 10 : value); }
        }
    }
}
