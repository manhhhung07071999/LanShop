using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Mvc;

namespace LanShop.Controllers
{
    using System.IO;
    using Vst;
    using Models;

    class SetupController : Controller
    {
        const string ConfigFileName = "server.json";

        static Config _conf;
        public static Config Config
        {
            get
            {
                if (_conf == null)
                {
                    try
                    {
                        _conf = Vst.Json.Read<Config>(MyApp.MapPath(ConfigFileName));
                    }
                    catch
                    {

                    }
                    if (_conf == null)
                    {
                        _conf = new Config();
                    }
                }
                return _conf;
            }
        }

        public ActionResult Default()
        {
            return View(new Vst.UpdateRequest { Value = Config, Action = Vst.UpdateActions.Insert });
        }

        public ActionResult Edit()
        {
            return View(new Vst.UpdateRequest {
                Value = new Config { ServerIp = _conf.ServerIp },
                Action = Vst.UpdateActions.Update
            });
        }

        public ActionResult Update(UpdateRequest request)
        {
            var conf = Config;

            var fi = new System.IO.FileInfo(MyApp.MapPath(ConfigFileName));
            if (fi.Exists == false)
            {
                fi.Create();
            }

            Vst.Json.Save(fi.FullName, request.Value);

            if (request.Action == UpdateActions.Update)
            {
                if (((Config)request.Value).ServerIp != conf.ServerIp)
                {
                    return View(new Views.Setup.Restart(), null);
                }
                else
                {
                    return Done();
                }
            }
            return Redirect("banHang");
        }

        public ActionResult Detect()
        {
            if (Config.ServerIp == null)
            {
                return GoFirst();
            }
            return Redirect("banHang");
        }
    }
}
