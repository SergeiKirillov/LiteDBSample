using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteDbMySample1
{
    class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Phone> Phones { get; set; }
    }

    class Phone
    {
        public int Code { get; set; }
        public string Number { get; set; }
        public PhoneType Type { get; set; }

    }

    public enum PhoneType { Module, LandLine}

}
