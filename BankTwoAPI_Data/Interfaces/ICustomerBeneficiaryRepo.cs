using System;
using System.Collections.Generic;
using System.Text;

namespace BankTwoAPI_Data.Interfaces
{
    public interface ICustomerBeneficiaryRepo<T>:ICRUDInterface<T>
    {
        List<T> GetCustomerBeneficiaries(string id, int offset, int count);
    }
}
