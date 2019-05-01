using System;
using System.Collections.Generic;
using System.Text;

namespace BankTwoAPI_Data.Interfaces
{
    public interface ICustomerRepo<T>:ICRUDInterface<T>
    {
         T GetByUsername(string username);
    }
}
