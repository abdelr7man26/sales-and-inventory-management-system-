using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Models
{
    public enum PersonType { Supplier, Customer }

    public class Person
    {
        public int ID { get; set; }
        public string PersonName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public decimal Balance { get; set; }
        public PersonType Type { get; set; }
    }
}

