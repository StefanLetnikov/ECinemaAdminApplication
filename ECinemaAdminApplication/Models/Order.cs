using System;
using System.Collections.Generic;

namespace ECinemaAdminApplication.Models
{
    public class Order
    {
        public  Guid Id { get; set; }
        public string UserId { get; set; }
        public ECinemaApplicationUser User { get; set; }


        public ICollection<TicketInOrder> TicketInOrders { get; set; }
    }
}
