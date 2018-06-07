using Curc.Enumerations;
using Curc.Extensions;
using Curc.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace Curc.Models
{
    public class UserPinModel : BaseModel
    {
        private string _image;
        public string image {
			get { return _image; }
			set {
				if (_image != value) {
					_image = value;
					onPropertyChanged(nameof(image));
					markerType = MarkerType.UserPin;
				}
			}
		}

        private string _name;
        public string name {
            get {
                return _name;
            }
            set {
                if (_name != value) {
                    _name = value;
                    this.onPropertyChanged(nameof(name));
                }
            }
        }

        private float _heading;
        public float heading {
            get {
                return _heading;
            }
            set {
                if (_heading != value) {
                    _heading = value;
                    this.onPropertyChanged(nameof(heading));
                }
            }
        }

        private MarkerType _markerType;
        public MarkerType markerType {
            get {
                return _markerType;
            }
            set {
                if (_markerType != value) {
                    _markerType = value;
                    this.onPropertyChanged(nameof(markerType));
                    setAnchor();
                }
            }
        }

        #region Position operations
        private Position _position;
        public Position position {
            get {
                return _position;
            }
            set {
                if (_position != value) {
                    _position = value;
                    this.onPropertyChanged(nameof(position));
                }
            }
        }
        private double? _latitude;
        public double? latitude {
            set {
                if (_latitude != value) {
                    _latitude = value;
                    setPosition();
                }
            }
        }
        private double? _longitude;
        public double? longitude {
            set {
                if (_longitude != value) {
                    _longitude = value;
                    setPosition();
                }
            }
        }
        private void setPosition()
        {
            if (_latitude == null || _longitude == null)
                return;
            position = new Position(_latitude.Value, _longitude.Value);
            _latitude = _longitude = null;
        } 
        #endregion

        private Pin _pin;
        public Pin pin {
            get {
                if (_pin == null) {
                    _pin = new Pin {
						Label = "",
                        IsDraggable = false,
                        Anchor = new Point(0.5, 0.5)
                    };
                    _pin.BindingContext = this;
                    _pin.SetBinding(Pin.LabelProperty, "name");
                    _pin.SetBinding(Pin.PositionProperty, "position");
                    _pin.SetBinding(Pin.RotationProperty, "heading");
                }
                return _pin;
            }
        }

        #region Image Processors
		private static Stream _startStream;
		private static Stream startStream {
			get {
				if (_startStream == null) {
					_startStream = getRawStream("start.png");
				}
				return _startStream;
			}
		}
		private static Stream _legStream;
		private static Stream legStream {
			get {
				if (_legStream == null) {
					_legStream = getRawStream("leg.png");
				}
				return _legStream;
			}
		}
		private static Stream _stopoverStream;
		private static Stream stopoverStream {
			get {
				if (_stopoverStream == null) {
					_stopoverStream = getRawStream("stopover.png");
				}
				return _stopoverStream;
			}
		}
		private static Stream _endStream;
		private static Stream endStream {
			get {
				if (_endStream == null) {
					_endStream = getRawStream("end.png");
				}
				return _endStream;
			}
		}
		private static Stream getRawStream(string fileName)
		{
			byte[] buffer = null;
			var assembly = typeof(UserPinModel).GetTypeInfo().Assembly;
			using (var stream = assembly.GetManifestResourceStream($"Curc.Resources.{fileName}")) {
				buffer = new byte[stream.Length];
				stream.Read(buffer, 0, (int)stream.Length);
				using (var editableImage = Plugin.ImageEdit.CrossImageEdit.Current.CreateImage(buffer)) {
					var modified = editableImage.Resize(((int)(30 * Constants.nativeScale)), ((int)(30 * Constants.nativeScale))).ToPng();
					return new MemoryStream(modified);
				}
			}
		}
		public async Task downloadImageLink()
		{
			if (string.IsNullOrWhiteSpace(image))
				throw new Exception("Assign an image link first.");

			var imageStream = await image.toStreamAsync();
			if (imageStream != null) {
				using (var editableImage = await Plugin.ImageEdit.CrossImageEdit.Current.CreateImageAsync(imageStream)) {
					var modified = editableImage.Resize(((int)(30 * Constants.nativeScale)), ((int)(30 * Constants.nativeScale))).ToPng();
					pin.Icon = BitmapDescriptorFactory.FromStream(new MemoryStream(modified));
				}
			}
		}
		private void setAnchor()
		{
			switch (markerType) {
				case MarkerType.UserPin:
					pin.Anchor = new Point(0.5, 0.5);
					return;
				case MarkerType.Start:
					pin.Icon = BitmapDescriptorFactory.FromStream(UserPinModel.startStream);
					break;
				case MarkerType.Leg:
					pin.Icon = BitmapDescriptorFactory.FromStream(UserPinModel.legStream);
					break;
				case MarkerType.StopOver:
					pin.Icon = BitmapDescriptorFactory.FromStream(UserPinModel.stopoverStream);
					break;
				case MarkerType.End:
					pin.Icon = BitmapDescriptorFactory.FromStream(UserPinModel.endStream);
					break;
			}
			pin.Anchor = new Point(0.5, 1);
		}
        #endregion
    }
}
