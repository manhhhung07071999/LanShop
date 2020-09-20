using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class Account : BsonData.Document
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string AuthorName { get; set; }
        public string DbName { get; set; }
        public string Token { get; set; }
        public string ProfileId { get; set; }
        public int CheckProfile { get; set; }
        public string LanguageId { get; set; }
        public bool CheckAuthor(string[] author)
        {
            foreach (var a in author)
            {
                if (AuthorName == a)
                    return true;
            }
            return false;
        }
        public bool CheckAuthor(string author)
        {
            return this.CheckAuthor(new string[] { author });
        }
    }

    public class AccountBindingModel : Account
    {
        public string UserName { get; set; }
    }

    public class ChangePasswordBindingModel : Account
    {
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class CheckTokenResult
    {
        public AccountBindingModel Account { get; set; }
        public BsonData.DataBase Database { get; set; }
        public bool IsAuthorMatch(string author)
        {
            return Account != null && Account.AuthorName == author;
        }
        public bool IsAccountMatch
        {
            get { return Account != null; }
        }
    }
}