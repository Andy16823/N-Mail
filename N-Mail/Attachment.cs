using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nMail
{
    public class Attachment
    {
        public String Name { get; set; }
        public String ContentTyp { get; set; }
        public Byte[] bytes { get; set; }
        public String Encodebase { get; set; }

    }
}
