using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Repository.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        void StartTransaction();
        void CommitTransaction();
    }

    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private TransactionScope transaction;

        public void StartTransaction()
        {
            this.transaction = new TransactionScope();
        }

        public void CommitTransaction()
        {
            this.transaction.Complete();
        }

        public void Dispose()
        {
            this.transaction.Dispose();
        }
    }
}
