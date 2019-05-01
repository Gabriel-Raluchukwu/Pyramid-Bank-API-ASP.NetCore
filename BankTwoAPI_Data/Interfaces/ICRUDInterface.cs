using System;
using System.Collections.Generic;
using System.Text;

namespace BankTwoAPI_Data.Interfaces
{
    public interface ICRUDInterface<T>
    {
        //put UPDATE    post  CREATE
        //get READ   delete DELETE

        bool Post(T type);
        T Get(int id);
        List<T> GetAll(int offset,int count);
        bool Put(T type);
        bool Delete(int id);
    }
}
