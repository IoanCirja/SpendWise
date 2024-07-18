using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContracts
{
    public class ContactUsContract
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string message { get; set; }
    }
}
