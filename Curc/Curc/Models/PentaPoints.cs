using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace Curc.Models
{
    public class PentaPoints
    {
        public Position center { get; set; }
        public Position upperLeft { get; set; }
        public Position upperRight { get; set; }
        public Position lowerRight { get; set; }
        public Position lowerLeft { get; set; }
    }
}
