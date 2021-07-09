using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.BL.Model
{
    public class EmailModel
    {
        public string Email { get; set; }
        public string From { get; set; }
        public string Recipients { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
