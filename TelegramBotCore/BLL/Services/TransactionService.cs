using DAL.Models;
using System.Linq;
using AutoMapper;
using BLL.Models;
using System.Collections.Generic;
using System;

namespace BLL.Services
{
    public class TransactionService
    {
        private ModelContext _dbContext { get; set; }

        public TransactionService()
        {
            _dbContext = new ModelContext();

        }

        public User GetUser(string chatId)
        {
            var user = _dbContext.Users.ToList().FirstOrDefault(u => u.ChatId == chatId);
            return new User() { ChatId = user.ChatId};
        }

        public List<Transaction> GetTransactions(string chatId, string transactionType)
        {
            var transactionsDb =_dbContext.Transactions.ToList().Where(t => t.ChatID == chatId && t.TransactionType == transactionType);
            List<Transaction> transactions = new List<Transaction>();
            foreach(var t in transactionsDb)
            {
                if(t != null)
                {
                    transactions.Add(new Transaction()
                    {
                        TransactionType = t.TransactionType,
                        Amount = t.Amount,
                        Appointment = t.Appointment,
                        ChatID = t.ChatID,
                        StoredDate = t.StoredDate
                    });
                }
            }
            return transactions;
        }

        public void AddUser(string chatId)
        {
            if (_dbContext.Users.Contains(new UserDb() { ChatId = chatId }))
                return;
            var user = new UserDb() { ChatId = chatId };
            _dbContext.Add(user);
            SaveChanges();
        }

        public void AddTransaction(string chatId, Transaction transaction)
        {
            var transactionDb = new TransactionDb()
            {
                TransactionType = transaction.TransactionType,
                Amount = transaction.Amount,
                Appointment = transaction.Appointment,
                ChatID = chatId,
                StoredDate = DateTime.UtcNow
            };
            transactionDb.ChatID = chatId;
            _dbContext.Transactions.Add(transactionDb);
            SaveChanges();
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
