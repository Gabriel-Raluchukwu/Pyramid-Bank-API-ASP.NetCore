using System;
using System.Collections.Generic;
using System.Text;

namespace BankTwoAPI_Data.Interfaces
{
    public interface IInterBankTransferRepo<T>:ICRUDInterface<T>
    {
        List<T> GetCustomerAccountTransferRecords(string CustomerAccountNumber, int offset, int count);
    }
}
