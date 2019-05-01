using System;
using System.Collections.Generic;
using System.Text;

namespace BankTwoAPI_Data.Interfaces
{
    public interface IAirtimeTopUpRepo<T>:ICRUDInterface<T>
    {
        List<T> GetCustomerAirtimeTopUp(string accountNumber,int offset,int count);
    }
}
