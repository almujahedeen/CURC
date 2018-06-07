using System;
using System.Collections.Generic;
using System.Text;

namespace Curc.Models
{
    public class AddressAutocompleteItem : BaseModel
    {
        public List<Prediction> predictions { get; set; }
    }

    public class Prediction
    {
        public string description { get; set; }
        public string place_id { get; set; }
    }
}
