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
    public class MobileNetworksRepo : IMobileNetworksRepo<MobileNetworks>
    {
        public BankTwoDatabase _bankTwoDatabase { get; }
        private ILogger _logger { get; }

        public MobileNetworksRepo(BankTwoDatabase bankTwoDatabase,ILogger<MobileNetworksRepo> logger)
        {
            _bankTwoDatabase = bankTwoDatabase;
            _logger = logger;
        }
        public bool Delete(int id)
        {
            var MobileNetwork = _bankTwoDatabase.MobileNetworks.Where(p => p.IsActive == true).SingleOrDefault(p => p.Id == id);
            try
            {
                MobileNetwork.IsActive = false;
                _logger.LogInformation("Delete: Saving network update to database", new { MobileNetwork.MobileNetworkName });
                _bankTwoDatabase.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError(LogEvents.DatabaseUpdateConcurrencyError,e,e.Message,new { MobileNetwork.MobileNetworkName});
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.DatabaseError,e,e.Message,new { MobileNetwork.MobileNetworkName});
                return false;
            }
            return true;
        }

        public MobileNetworks Get(int id)
        {
            var mobileNetwork = _bankTwoDatabase.MobileNetworks.Where(p => p.IsActive == true).SingleOrDefault(p => p.Id == id);
            return mobileNetwork;
        }

        public List<MobileNetworks> GetAll(int offset, int count)
        {
            List<MobileNetworks> mobileNetworks = _bankTwoDatabase.MobileNetworks.Where(p => p.IsActive == true)
                .Skip(offset).Take(count).ToList();
            return mobileNetworks;
        }

        public bool Post(MobileNetworks mobileNetwork)
        {
            _bankTwoDatabase.MobileNetworks.Add(mobileNetwork);
            try
            {
                _logger.LogInformation("Saving Mobile network to database",mobileNetwork);
                _bankTwoDatabase.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.DatabaseError,e,e.Message,mobileNetwork);
                return false;
            }
            return true;
        }

        public bool Put(MobileNetworks mobileNetwork)
        {
            _bankTwoDatabase.Entry(mobileNetwork).State = EntityState.Modified;
            try
            {
                _logger.LogInformation("Saving Mobile network update to database", mobileNetwork);
                _bankTwoDatabase.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError(LogEvents.DatabaseUpdateConcurrencyError, e, e.Message, mobileNetwork);
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.DatabaseError, e, e.Message, mobileNetwork);
                return false;
            }
            return true;
        }
    }
}
