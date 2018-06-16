using System;
using System.Collections.Generic;
using Xamarin.Forms.GoogleMaps;
//using Curc.Helpers;

namespace Curc.Models
{
	public class GoogleRoute : BaseModel
	{
		public List<Routes> routes { get; set; }

		public class Routes
		{
			public OverviewPolyline overview_polyline { get; set; }
		}
		public class OverviewPolyline
		{
			public string points { get; set; }

			public List<Position> polyLinePositions {
				get {
					if (string.IsNullOrWhiteSpace(points)) return null;
					var poly = new List<Position>();
					char[] polylinechars = points.ToCharArray();
					int index = 0;

					int currentLat = 0;
					int currentLng = 0;
					int next5bits;
					int sum;
					int shifter;

					try {
						while (index < polylinechars.Length) {
							sum = 0;
							shifter = 0;
							do {
								next5bits = polylinechars[index++] - 63;
								sum |= (next5bits & 31) << shifter;
								shifter += 5;
							} while (next5bits >= 32 && index < polylinechars.Length);

							if (index >= polylinechars.Length)
								break;

							currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);


							sum = 0;
							shifter = 0;
							do {
								next5bits = polylinechars[index++] - 63;
								sum |= (next5bits & 31) << shifter;
								shifter += 5;
							} while (next5bits >= 32 && index < polylinechars.Length);

							if (index >= polylinechars.Length && next5bits >= 32)
								break;

							currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
							var lat = Convert.ToDouble(currentLat) / 100000.0;
							var lng = Convert.ToDouble(currentLng) / 100000.0;
							var p = new Position(lat, lng);
							poly.Add(p);
						}
					} catch (Exception ex) {
						System.Diagnostics.Debug.WriteLine(ex.StackTrace);
					}
					return poly;
				}
			}
		}
	}
}
