using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specification.OrderSpec
{
    public class OrderSpecification : BaseSpecification<Models.Order.Order>
    {
        public OrderSpecification(string email):
            base(o=>o.BuyerEmail== email){
           Includes.Add(o => o.DeliveryMethod);
           Includes.Add(o => o.Items);
           AddByDSC(o => o.OrderDate);
        }
        public OrderSpecification(string email , int orderId) :
            base(o => o.BuyerEmail == email && o.Id == orderId)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
        }
    }
}
