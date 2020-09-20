using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.Data;

using System.Mvc;

namespace LanShop.Controllers.Data
{
    class AccountController : Vst.Network.SocketController
    {
        static BsonData.Collection _accounts;
        public static BsonData.Collection Accounts
        {
            get
            {
                if (_accounts == null)
                {
                    new DataController();

                    _accounts = RemovableDB.GetCollection<Account>();
                    if (_accounts.Count() == 0)
                    {
                        var id = "SupperAdmin";
                        var acc = new Account
                        {
                            Name = "Supper Admin",
                            AuthorName = id,
                            Password = VST.Security.Generate(id.ToLower() + "1"),
                        };
                        _accounts.Insert(id, acc);
                    }
                }
                return _accounts;
            }
        }

        protected virtual object Login(LoginInfo info)
        {
            if (info != null && !string.IsNullOrEmpty(info.UserName))
            {
                var acc = Accounts.FindById<Account>(info.UserName);
                if (acc == null) { return new Vst.Network.SocketMessage { Code = -1, Message = "ACCOUNT" }; }

                var pass = VST.Security.Generate(acc.Id + info.Password);
                if (acc.Password == pass)
                {
                    var token = acc.Token;
                    if (token == null)
                    {
                        token = VST.Security.Generate(acc.Id + DateTime.Now.Ticks);
                        acc.Token = token;

                        Accounts.Update(acc.Id, acc);
                    }

                    acc.Password = null;
                    var u = new UserInfo
                    {
                        Account = acc,
                        ObjectId = acc.Id,
                    };
                    UserController.Users.Update(token, u);
                    return new Vst.Network.SocketMessage { Message = Vst.Json.GetString(u) };
                }

                return new Vst.Network.SocketMessage { Code = -2, Message = "PASS" };
            }
            return null;
        }
        public virtual object Login()
        {
            var info = Vst.Json.GetObject<LoginInfo>(MessageContext.Message);
            return Login(info);
        }
        public virtual object Logout()
        {
            UserController.Users.FindAndDelete<UserInfo>(MessageContext.Message, null);
            return null;
        }

        public object Create(string id, Account acc)
        {
            if (Accounts.Contains(id)) { return "EXIST"; }

            if (acc.Password == null)
            {
                acc.Password = VST.Security.Generate(id + id);
            }
            Accounts.Insert(id, acc);
            return null;
        }

        public object ChangePass()
        {
            return null;
        }

        public ActionResult Join()
        {
            return View(new Vst.UpdateRequest { Value = new LoginInfo {
                UserName = "supperadmin", Password = "1",
            } });
        }
        public ActionResult Join(Vst.UpdateRequest request)
        {
            var info = (LoginInfo)request.Value;
            var res = Login(info);

            if ((int)res.GetType().GetProperty("Code").GetValue(res) < 0)
            {
                System.Windows.MessageBox.Show((string)res.GetType().GetProperty("Message").GetValue(res));
                return View(request);
            }

            return Redirect("HoaDon");
        }
    }
}
