using System;

namespace DAL.Models
{
    public class TransactionDb
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string ChatID { get; set; }
        public DateTime StoredDate { get; set; }
        public string Appointment { get; set; }
        public string TransactionType { get; set; }
    }
}
