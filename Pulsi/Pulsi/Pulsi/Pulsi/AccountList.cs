using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulsi
{
    class AccountList
    {
        //Fields
        private List<Account> accountList = new List<Account>();

        //Properties
        public List<Account> AccountListProperty
        {
            get { return accountList; }
            set { accountList = value; }
        }

        //methods
        public void AddAccount(string username, string password)
        {
            Account account = new Account(username, password);
            accountList.Add(account);
        }
    }
}
