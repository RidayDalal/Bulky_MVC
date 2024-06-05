using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulky.Models;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        // This repository implements all methods from the IRepository, but with a 
        // specific data type of Order headers. It also adds in two additional methods, 
        // the update and save methods.

        // Updates the Order headers provided by the user.
        void Update(OrderHeader obj);

        // This is for when the order itself goes through different stages of processing 
        // and shipping. This method can be used to modify only the order status, keeping
        // payment status the same as we go along.
        void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);

        // 
        void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId);
    }
}
