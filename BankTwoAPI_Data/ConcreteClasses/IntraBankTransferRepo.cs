using BankTwoAPI_Data.Data;
using BankTwoAPI_Data.Interfaces;
using BankTwoAPI_Entities.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.Extensions.Logging;

namespace BankTwoAPI_Data.ConcreteClasses
{
    public class IntraBankTransferRepo : IIntraBankTransferRepo<IntraBankTransfer>
    {
        public BankTwoDatabase _bankTwoDatabase { get; }
        private ILogger _logger { get; }

        public IntraBankTransferRepo(BankTwoDatabase bankTwoDatabase , ILogger<IntraBankTransferRepo> logger)
        {      
            _bankTwoDatabase = bankTwoDatabase;
            _logger = logger;
        }

        public bool Delete(int id)
        {
            //No Soft delete implemented
            _logger.LogError("Delete method not implemented [IntraBankTransferRepository]");
            throw new NotImplementedException();
        }

        public IntraBankTransfer Get(int id)
        {
            var intraTransferRecord = _bankTwoDatabase.IntraBankTransfers.Include(p => p.Banks).SingleOrDefault(p => p.Id == id);
            return intraTransferRecord;
        }

        public List<IntraBankTransfer> GetAll(int offset, int count)
        {
            var intraTransferRecords = _bankTwoDatabase.IntraBankTransfers.Skip(offset).Take(count).Include(p => p.Banks).ToList();
            return intraTransferRecords;
        }

        public List<IntraBankTransfer> GetCustomerAccountTransferRecords(string CustomerAccountNumber, int offset, int count)
        {
            //RawSqlString RawSQLQuery = "";
            var intraBankTransfers = _bankTwoDatabase.IntraBankTransfers.Where(p => p.CustomerAccountNumber == CustomerAccountNumber)
                .Skip(offset).Take(count).Include(p => p.Banks).ToList();
            return intraBankTransfers;
        }

        public bool Post(IntraBankTransfer intraBankTransfer)
        {
            _bankTwoDatabase.IntraBankTransfers.Add(intraBankTransfer);
            try
            {
                _logger.LogInformation("Saving Intrabank transfer record to database", intraBankTransfer);
                _bankTwoDatabase.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.DatabaseError,e,e.Message,intraBankTransfer);
                return false;
            }
            return true;
        }

        public bool Put(IntraBankTransfer intraBankTransfer)
        {
            _logger.LogError("Update method not implemented [IntraBankTransferRepository]");
            throw new Exception("Invalid Command. Record cannot be updated");

        }


    }
}
