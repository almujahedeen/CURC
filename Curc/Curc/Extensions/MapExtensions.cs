using Curc.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace Curc.Extensions
{
    public static class MapExtensions
    {
        public static PentaPoints toPentaPoints(this MapSpan mapSpan)
        {
            var center = mapSpan.Center;
            var halfheightDegrees = mapSpan.LatitudeDegrees / 2;
            var halfwidthDegrees = mapSpan.LongitudeDegrees / 2;

            var left = center.Longitude - halfwidthDegrees;
            var right = center.Longitude + halfwidthDegrees;
            var top = center.Latitude + halfheightDegrees;
            var bottom = center.Latitude - halfheightDegrees;

            if (left < -180)
                left = 180 + (180 + left);
            if (right > 180)
                right = (right - 180) - 180;

            var topLeft = new Position(top, left);
            var topRight = new Position(top, right);
            var bottomRight = new Position(bottom, right);
            var bottomLeft = new Position(bottom, left);

            return new PentaPoints {
                center = center,
                upperLeft = topLeft,
                upperRight = topRight,
                lowerRight = bottomRight,
                lowerLeft = bottomLeft
            };
        }

        public static Position toGoogleMapsPosition(this Plugin.Geolocator.Abstractions.Position position)
        {
            return new Position(position.Latitude, position.Longitude);
        }

        public static Plugin.Geolocator.Abstractions.Position toGeolocatorPosition(this Position position)
        {
            return new Plugin.Geolocator.Abstractions.Position(position.Latitude, position.Longitude);
        }
    }
}
