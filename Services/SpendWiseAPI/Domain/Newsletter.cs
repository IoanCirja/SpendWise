using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Newsletter
    {
        public Guid subscription_id {  get; set; }
        public string email { get; set; }
    }
}
