using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.IO;
using System.Xml;

namespace System.Windows
{
    public class BindingInfo
    {
        public string BindingName { get; set; }
        public string Caption { get; set; }
        public string Input { get; set; }
        public bool AllowNull { get; set; }
        public object Value { get; set; }
        public string Type { get; set; }
        public string FormatString { get; set; }
        public int Width { get; set; }

        public string ToString(object v)
        {
            if (v == null || v.Equals(string.Empty))
                return null;

            if (FormatString == null)
                return v.ToString();

            return string.Format("{0:" + FormatString + "}", v);
        }
    }
    public class BindingInfoCollection : Dictionary<string, BindingInfo>
    {
        static Dictionary<string, BindingInfoCollection> _temps;
        public static void Include(string filename)
        {
            try
            {
                var map = Vst.Json.Read<Dictionary<string, BindingInfoCollection>>(filename);
                if (_temps == null)
                {
                    _temps = map;
                }
                else
                {
                    foreach (var p in map)
                    {
                        if (_temps.ContainsKey(p.Key))
                        {
                            _temps[p.Key] = p.Value;
                        }
                        else
                        {
                            _temps.Add(p.Key, p.Value);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public BindingInfoCollection Clone()
        {
            return (BindingInfoCollection)this.MemberwiseClone();
        }

        public static implicit operator BindingInfoCollection(string name)
        {
            return _temps[name];
        }
    }
}