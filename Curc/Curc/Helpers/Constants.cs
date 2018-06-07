using System;
using System.Collections.Generic;
using System.Text;

namespace Curc.Helpers
{
    public class Constants
    {
        public static double screenWidth { get; set; }
        public static double screenHeight { get; set; }
        public static double scale { get { return (screenWidth + screenHeight) / (320 + 568); } }
        public static float nativeScale { get; set; }

        public const string placesAutocompleteAPIKey = "AIzaSyCEXvIYhVVugb9jx6Aiqoe9G63wECJn92g";
    }
}
