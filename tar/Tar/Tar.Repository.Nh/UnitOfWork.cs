using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using Tar.Core;

namespace Tar.Repository.Nh
{
    public class UnitOfWork : Repository.UnitOfWork
    {
         private readonly List<ISessionHelper> _sessionHelpers;
        private readonly List<ITransaction> _transactions;

        public UnitOfWork(IServiceLocator locator, IUnitOfWorkCounter counter)
            : base(counter)
        {
            _sessionHelpers = locator.GetAll<ISessionHelper>().ToList();
            _transactions = _sessionHelpers.Select(
                sessionHelper => sessionHelper.GetSession().BeginTransaction()).ToList();
        }

        protected override void DoCommit()
        {
            _transactions.ForEach(transaction => transaction.Commit());
            _sessionHelpers.ForEach(sessionHelper => sessionHelper.CloseSession());
        }

        protected override void DoRollback()
        {
            _transactions.ForEach(transaction => transaction.Rollback());
            _sessionHelpers.ForEach(sessionHelper => sessionHelper.CloseSession());
        }
        //private readonly ISessionHelper _sessionHelper;
        //private readonly ITransaction _transaction;

        //public UnitOfWork(ISessionHelper sessionHelper, IUnitOfWorkCounter counter)
        //    : base(counter)
        //{
        //    if (sessionHelper == null) throw new ArgumentNullException("sessionHelper");
        //    _sessionHelper = sessionHelper;
        //    _transaction = _sessionHelper.GetSession().BeginTransaction();
        //}

        //protected override void DoCommit()
        //{
        //    _transaction.Commit();
        //    _sessionHelper.CloseSession();
        //}

        //protected override void DoRollback()
        //{
        //    _transaction.Rollback();
        //    _sessionHelper.CloseSession();
        //}
    }
}