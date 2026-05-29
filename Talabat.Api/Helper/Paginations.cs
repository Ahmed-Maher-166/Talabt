using Talabat.APIS.DTOS;

namespace Talabat.APIS.Helper
{
    public class Paginations<T>
    {

        public Paginations(int take, int skip,int _Count, IReadOnlyList<T> mappedProduct)
        {
            PageSize = take;
            Index = skip;
            Data = mappedProduct;
            Count = _Count;
        }

        public int Count { get; set; }
        public int Index {  get; set; }
        public int PageSize { get; set; }
        public IReadOnlyList<T> Data { get; set; }
    }
}
