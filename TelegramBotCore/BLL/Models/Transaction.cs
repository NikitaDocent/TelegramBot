using System;

namespace BLL.Models
{
    public class Transaction
    {
        public double Amount { get; set; }
        public DateTime StoredDate { get; set; }
        public string Appointment { get; set; }
        public string ChatID { get; set; }
        public string TransactionType { get; set; }
        public Transaction()
        {

        }
        public Transaction(double ammount, string appointment, string type)
        {
            Amount = ammount;
            Appointment = appointment;
            TransactionType = type;
        }
    }
}
