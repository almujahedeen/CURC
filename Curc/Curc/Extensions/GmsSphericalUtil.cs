using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace Curc.Extensions
{
    /// <summary>
    /// This is a java to c# port from 
    /// https://github.com/googlemaps/android-maps-utils/blob/dba3b0d8a9657ebab8c67a4f50bd731437a229bc/library/src/com/google/maps/android/SphericalUtil.java
    /// </summary>
    public static class GmsSphericalUtil
    {
        /// <summary>
        ///  Returns the LatLng resulting from moving a distance from an origin
        ///  in the specified heading (expressed in degrees clockwise from north).
        /// </summary>
        /// <param name="from">The LatLng from which to start.</param>
        /// <param name="distance">The distance to travel</param>
        /// <param name="heading">The heading in degrees clockwise from north.</param>
        /// <returns>Position with offset</returns>
        public static Position ComputeOffset(Position from, double distance, double heading)
        {
            distance /= GmsMathUtils.EarthRadius;
            heading = heading.ToRadian();
            // http://williams.best.vwh.net/avform.htm#LL
            double fromLat = from.Latitude.ToRadian();
            double fromLng = from.Longitude.ToRadian();
            double cosDistance = Math.Cos(distance);
            double sinDistance = Math.Sin(distance);
            double sinFromLat = Math.Sin(fromLat);
            double cosFromLat = Math.Cos(fromLat);
            double sinLat = cosDistance * sinFromLat + sinDistance * cosFromLat * Math.Cos(heading);
            double dLng = Math.Atan2(
                    sinDistance * cosFromLat * Math.Sin(heading),
                    cosDistance - sinFromLat * sinLat);
            return new Position(Math.Asin(sinLat).ToDegrees(), (fromLng + dLng).ToDegrees());
        }
    }
}
