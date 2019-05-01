using System;
using System.Collections.Generic;
using System.Text;

namespace BankTwoAPI_Data.Interfaces
{
    public interface ICardRequestRepo<T>:ICRUDInterface<T>
    {
        List<T> GetCustomerCardRequests(string accountNumber, int offset, int count);
    }
}
