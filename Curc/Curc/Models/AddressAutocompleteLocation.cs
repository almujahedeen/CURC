using System;
using System.Collections.Generic;
using System.Text;

namespace Curc.Models
{
    public class AddressAutocompletePlaceID : BaseModel
    {
        public AddressAutocompletePlaceIDResult result { get; set; }
    }

    public class AddressAutocompletePlaceIDResult
    {
        public AddressAutocompletePlaceIDGeometry geometry { get; set; }
    }
    public class AddressAutocompletePlaceIDGeometry
    {
        public AddressAutocompletePlaceIDLocation location { get; set; }
    }
    public class AddressAutocompletePlaceIDLocation
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

}
