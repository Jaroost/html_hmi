using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFServer
{
    public class Machine
    {
        public string Type { get; set; }
        public List<StationContent> Stations { get; set; }
    }

    public class StationContent
    {
        public StationContent(int num, string type)
        {
            this.Num = num;
            this.Type = type;
        }
        public int Num { get; set; }
        public string Type { get; set; }
    }

    public class UnitStation: StationContent
    {
        public UnitStation(int num):base(num, "Unit")
        {
            //rien
        }

        public List<Axe> Axis { get; set; }
    }

    public class Axe
    {
        public string Name { get; set; }
        public double Minimum { get; set; }
        public double Maximum { get; set; }
        public double Position { get; set; }
    }

    public class LoaderStation : StationContent
    {
        public LoaderStation(int num):base(num, "Loader")
        {
            //rien
        }

        public List<Robot> Robots { get; set; }
    }

    public class UnloaderStation: StationContent
    {
        public UnloaderStation(int num): base(num, "Unloader")
        {
            //rien
        }
        public List<Robot> Robots { get; set; }
    }

    public class Robot
    {
        static int GLOBAL_ID = 0;
        private string key = string.Empty;
        public string Key
        {
            get
            {
                if (key.Equals(string.Empty))
                {
                    key = GLOBAL_ID + "";
                    GLOBAL_ID++;
                }
                return key;
            }
            set
            {
                key = value;
            }
        }
        public string SerialNumber { get; set; }
        public string Name { get; set; }
    }
}
