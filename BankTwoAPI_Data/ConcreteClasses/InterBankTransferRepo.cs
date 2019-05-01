using BankTwoAPI_Data.Data;
using BankTwoAPI_Data.Interfaces;
using BankTwoAPI_Entities.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace BankTwoAPI_Data.ConcreteClasses
{
    public class InterBankTransferRepo : IInterBankTransferRepo<InterBankTransfer>
    {
        public BankTwoDatabase _bankTwoDatabase { get; }
        private ILogger _logger { get; }

        public InterBankTransferRepo(BankTwoDatabase bankTwoDatabase,ILogger<InterBankTransfer> logger)
        {
            _bankTwoDatabase = bankTwoDatabase;
            _logger = logger;
        }   

        public bool Delete(int id)
        {
            //No Soft Delete Implemented
            _logger.LogError("Delete method not implemented [InterBankTransferRepo]");
            throw new NotImplementedException();
        }

        public InterBankTransfer Get(int id)
        {
            var interBankTransfer = _bankTwoDatabase.InterBankTransfers.SingleOrDefault(p=> p.Id == id);
            return interBankTransfer;          
        }

        public List<InterBankTransfer> GetAll(int offset, int count)
        {
            var interBankTransfers = _bankTwoDatabase.InterBankTransfers.Skip(offset).Take(count).ToList();
            return interBankTransfers;
        }

        public List<InterBankTransfer> GetCustomerAccountTransferRecords(string CustomerAccountNumber, int offset, int count)
        {
            //RawSqlString SQLQUery = "";
            var interBankTransfers = _bankTwoDatabase.InterBankTransfers.Where(p => p.CustomerAccountNumber == CustomerAccountNumber)
                .Skip(offset).Take(count).Include(p => p.Banks).ToList();
            return interBankTransfers;
        }

        public bool Post(InterBankTransfer interBankTransfer)
        {
            _bankTwoDatabase.InterBankTransfers.Add(interBankTransfer);
            try
            {
                _logger.LogInformation("Saving Intertransfer record to database",interBankTransfer);
                _bankTwoDatabase.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.DatabaseError,e,e.Message,interBankTransfer);
                return false;
            }
            return true;
        }

        public bool Put(InterBankTransfer type)
        {
            //No Record Update Implemented
            _logger.LogError("Delete method not implemented [InterBankTransferRepo]");
            throw new Exception("Invalid Command. Record cannot be updated");
        }


    }
}
