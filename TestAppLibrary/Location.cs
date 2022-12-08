using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppLibrary
{
    public class Location
    {
        public string Name { get; set; }
        public Region Region { get; set; }

        public Location(string name, Region region)
        {
            Name = name;
            Region = region;
        }
    }
}
