using BankTwoAPI_Data.Data;
using BankTwoAPI_Data.Interfaces;
using BankTwoAPI_Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankTwoAPI_Data.ConcreteClasses
{
    public class CardRequestRepo : ICardRequestRepo<CardRequest>
    {
        public BankTwoDatabase _bankTwoDatabase { get; }
        private ILogger _logger { get; }

        public CardRequestRepo(BankTwoDatabase bankTwoDatabase,ILogger<CardRequestRepo> logger)
        {
            _bankTwoDatabase = bankTwoDatabase;
            _logger = logger;
        }
        public bool Delete(int id)
        {
            //Card Requests cannot be deleted
            //No Delete function implemented
            _logger.LogError("Delete Method Not Implemented [CardRequestRepository]");
            throw new NotImplementedException();
        }

        public CardRequest Get(int id)
        {
            var cardRequest = _bankTwoDatabase.CardRequests//.Where(p => p.IsActive == true)
                .SingleOrDefault(p => p.Id == id);
            return cardRequest;
           // throw new NotImplementedException();
        }

        public List<CardRequest> GetAll(int offset, int count)
        {
            var AllCardRequests = _bankTwoDatabase.CardRequests//.Where(p => p.IsActive == true)
                .Skip(offset).Take(count).ToList();
            return AllCardRequests;
        }

        public List<CardRequest> GetCustomerCardRequests(string accountNumber,int offset, int count)
        {
            var customerCardRequests = _bankTwoDatabase.CardRequests//.Where(p => p.IsActive == true)
                .Where(p => p.CustomerAccountNumber == accountNumber).Skip(offset).Take(count).ToList();
            return customerCardRequests;
        }

        public bool Post(CardRequest cardRequest)
        {
            _bankTwoDatabase.CardRequests.Add(cardRequest);
            try
            {
                _bankTwoDatabase.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.DatabaseError,e,e.Message,cardRequest);
                return false;
            }
            return true;
        }

        public bool Put(CardRequest type)
        {
            //No Update function Implemented
            //No CardRequest can be Updated
            _logger.LogError("Update Method Not Implemented [CardRequestRepository]");
            throw new NotImplementedException();
        }

      
        //NOTE: Make Respective charges after succsessful card request in Bank logic project
    }
}
