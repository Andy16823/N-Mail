using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nMail
{
    /// <summary>
    /// Stellt eine Klasse für Anhänge bereit
    /// </summary>
    public class Attachment
    {

        public String Name { get; set; }
        public String ContentType { get; set; }
        public Byte[] Bytes { get; set; }
        public String EncodeBase { get; set; }

    }
}
