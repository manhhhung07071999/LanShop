using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace LanShop.Controllers.Data
{
    class UserController : DataController
    {
        static BsonData.Collection _users;
        public static BsonData.Collection Users
        {
            get
            {
                if (_users == null)
                {
                    _users = MainDb.GetCollection("Users");
                }
                return _users;
            }
        }

        #region Token Checking
        public UserInfo GetUserByToken(string token, params string[] authors)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }
            var u = Users.FindById<UserInfo>(token);
            if (u != null && (authors.Length == 0 
                || u.Account.CheckAuthor(authors))) {
                return u;
            }
            return null;
        }
        #endregion
    }
}
