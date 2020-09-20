using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace LanShop.Views.Account
{
    class Join : Editor<LoginInfo>
    {
        protected override bool UpdateModel(string actionName)
        {
            return base.UpdateModel("join");
        }
    }
}
