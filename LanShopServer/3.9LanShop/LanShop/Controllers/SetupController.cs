using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Mvc;
using Models;
using Models.Data;

namespace LanShop.Controllers
{
    using System.IO;
    using Vst;

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
            return View(new Vst.UpdateRequest
            {
                Value = new Config { ServerIp = Config.ServerIp },
                Action = Vst.UpdateActions.Update
            });
        }

        public ActionResult Update(UpdateRequest request)
        {
            var conf = SetupController.Config;

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
            return Redirect("home");
        }

        //public ActionResult Detect()
        //{
        //    if (Config.ServerIp == null)
        //    {
        //        return GoFirst();
        //    }

        //    return DetectDb();
        //}

        public ActionResult DetectDb()
        {
            var usb = RemovableDB.GetDrivers();
            if (usb.Count == 0)
            {
                return View(new Views.Setup.UsbNotFound(), null);
            }

            var dbPath = RemovableDB.GetDbDrive(usb);
            if (dbPath == null)
            {
                return View(new Views.Setup.CreateDb(), usb);
            }

            RemovableDB.Create(dbPath);

            //var serverPath = MyApp.MapPath("dataserver.exe");
            //var info = new System.Diagnostics.ProcessStartInfo(serverPath);
            //info.Arguments = Config.ServerIp + " " + dbPath;

            //System.Diagnostics.Process.Start(info);

            return Redirect("server");
        }
        public ActionResult CreateDb(bool ok)
        {
            MyApp.Browser.Close();
            return Done();
        }
        public ActionResult CreateDb(string dir)
        {
            RemovableDB.Create(dir);
            return Redirect("hoaDon");
        }

        public ActionResult CloneDb()
        {
            var usb = new List<string>();
            string src = null;
            foreach (var u in RemovableDB.GetDrivers())
            {
                if (RemovableDB.HasDb(u))
                {
                    if (src != null)
                        return Message("2db");

                    src = u;
                }
                else
                {
                    usb.Add(u);
                }
            }
            if (usb.Count == 0)
                return Message("0db");

            return View(new Views.Setup.CreateDb(), usb);
        }

        public ActionResult CloneDb(string dir)
        {
            if (dir == "False")
            {
                return Done();
            }

            var db = new BsonData.DataBase(dir, RemovableDB.DataBase.Name);
            db.GetCollection<HoaDon>();
            db.GetCollection<MatHang>();

            var model = new CloneModel {
                Database = db,
                Destination = dir,
                Source = RemovableDB.GetAllFile(),
            };


            return View(new Views.Setup.RunClone(), model);
        }

        public ActionResult CloneCompleted()
        {
            RemovableDB.GetCollection<HoaDon>().Clear();
            return Redirect("hoaDon");
        }
    }
}
