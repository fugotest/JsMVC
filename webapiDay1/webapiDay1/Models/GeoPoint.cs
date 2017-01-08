using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace webapiDay1.Models
{
    public class GeoPoint
    {
        [Range (1, 5)]
        public int lat { get; set; }
        public int lon { get; set; }
    }
}