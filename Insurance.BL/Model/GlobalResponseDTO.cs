using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.BL.Model
{
    public class GlobalResponseDTO
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int ID { get; set; }
    }
}
