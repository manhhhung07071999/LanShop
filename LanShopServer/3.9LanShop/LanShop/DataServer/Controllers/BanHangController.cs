using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Mvc;
using Models;
using Models.Data;
using System.Xml;

namespace DataServer.Controllers
{
    class BanHangController : Controller
    {
        BsonData.Collection MatHangDb
        {
            get
            {
                return RemovableDB.GetCollection<MatHang>();
            }
        }
        BsonData.Collection HoaDonDb
        {
            get
            {
                return RemovableDB.GetCollection<Models.HoaDon>();
            }
        }
        BsonData.Collection KhachHangDb
        {
            get
            {
                return RemovableDB.GetCollection<Models.KhachHang>();
            }
        }

        class FindInfo
        {
            public string Value { get; set; }
        }

        T GetValue<T>()
        {
            return Vst.Json.GetObject<T>(MessageContext.Message);
        }

        public object Save()
        {
            var v = GetValue<HoaDon>();
            HoaDonDb.Insert(v);

            return new { Html = GenerateHtml("template/hoa_don_temp.html", v) };
        }

        public object TimKhachHang()
        {
            var v = GetValue<KhachHang>();
            return KhachHangDb.FindById<KhachHang>(v.SoDT);
        }
        
        public object TimMatHang()
        {
            var i = this.GetValue<FindInfo>();
            var data = MatHangDb.FindById<MatHang>(i.Value);
            if (data != null)
            {
                return data;
            }

            var arr = MatHangDb.Select<MatHang>(x => x.MaVach == i.Value).ToArray();
            if (arr.Length > 0)
                return arr[0];

            return "NOT-FOUND";
        }

        public string GenerateHtml(string path, HoaDon hoaDon)
        {
            var doc = new XmlDocument
            {
                Schemas = new System.Xml.Schema.XmlSchemaSet()
            };
            doc.Load(path);

            var map = new Dictionary<string, XmlElement>();
            var Q = new Queue<XmlElement>();
            Q.Enqueue(doc.DocumentElement);

            while (Q.Count > 0)
            {
                var e = Q.Dequeue();
                var id = e.GetAttribute("id");

                if (!string.IsNullOrEmpty(id))
                {
                    map.Add(id, e);
                }

                var child = e.FirstChild;
                while (child != null)
                {
                    if (child.NodeType == XmlNodeType.Element)
                    {
                        Q.Enqueue((XmlElement)child);

                    }
                    child = child.NextSibling;
                }
            }

            foreach (var p in hoaDon.GetType().GetProperties())
            {
                XmlElement node;
                if (map.TryGetValue(p.Name, out node))
                {
                    var v = p.GetValue(hoaDon);
                    SetText(node, v);
                }
            }

            var tab = map["lines-content"];
            var lineTemp = (XmlElement)tab.LastChild;

            tab.RemoveChild(lineTemp);

            var type = typeof(Models.ChiTiet);
            int index = 0;
            foreach (var line in hoaDon.ChiTiet)
            {
                var tr = tab.AppendChild(lineTemp.Clone());
                foreach (XmlElement td in tr)
                {
                    var name = td.GetAttribute("class");
                    if (string.IsNullOrEmpty(name)) { continue; }
                    if (name == "num")
                    {
                        SetText(td, ++index);
                        continue;
                    }

                    var p = type.GetProperty(name);
                    if (p != null)
                    {
                        SetText(td, p.GetValue(line));
                    }
                }
            }

            return doc.OuterXml;
        }
        protected virtual void SetText(XmlElement e, object v)
        {
            if (v == null || string.Empty.Equals(v)) { return; }
            var frm = e.GetAttribute("data-format");
            if (string.IsNullOrEmpty(frm))
            {
                frm = "{0}";
            }
            else
            {
                frm = "{0:" + frm + "}";
            }
            e.InnerText = string.Format(frm, v);
        }

    }

}
