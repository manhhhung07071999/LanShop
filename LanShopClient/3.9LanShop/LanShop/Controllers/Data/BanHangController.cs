using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Mvc;

namespace LanShop.Controllers.Data
{
    using Models;
    class BanHangController : ClientController
     {
        static MapChiTiet _details;

        public ActionResult Default()
        {
            if (_details == null || _details.Count == 0)
            {
                _details = new MapChiTiet();
            }
            return View(_details);
        }

        public ActionResult TimMatHang(string value)
        {
            var msg = new Vst.Network.SocketMessage("banHang/timMatHang", new { Value = value });
            Send(msg, res => {
                return Vst.Json.GetObject<ChiTiet>(res.Message);
            });
            
            return Done();
        }

        public ActionResult NhapChiTiet()
        {
            return View();
        }

        public ActionResult ShowMatHang(Vst.UpdateActions action, ChiTiet matHang)
        {
            matHang.SoLuong = 1;
            return View(new Vst.UpdateRequest {
                Action = action,
                Value = matHang
            });
        }

        public ActionResult Update(Vst.UpdateRequest request)
        {
            _details.Update(request.Action, (ChiTiet)request.Value);
            return Done();
        }

        public ActionResult ShowHoaDon()
        {
            var h = new HoaDon();

            h.Ngay = DateTime.Now;
            h.Ma = h.Ngay.Ticks.ToString();

            double s = 0;
            foreach (var p in _details)
            {
                s += p.Value.TongSauVAT;
            }
            h.Tong = s;
            return View(h);
        }

        public ActionResult ChotHoaDon(HoaDon model)
        {
            model.ChiTiet.AddRange(_details.Values);
            var msg = new Vst.Network.SocketMessage("banHang/save", model);
            Send(msg, res => {
                Engine.Execute("banHang/InHoaDon", res.Message);
                return null;
            });

            return Done();
        }

        public ActionResult InHoaDon(string msg)
        {
            var doc = Vst.Json.GetObject<HoaDonDocument>(msg);
            return View(doc);
        }
     }
    
}
