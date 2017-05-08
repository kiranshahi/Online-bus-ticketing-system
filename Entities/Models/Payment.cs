using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class Payment
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public decimal PaidAmount { get; set; }
        public string PaymentId { get; set; }
        public int BookSeatId { get; set; }
    }
}
