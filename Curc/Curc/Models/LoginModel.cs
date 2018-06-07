using System;
using System.Collections.Generic;
using System.Text;

namespace Curc.Models
{
    public class LoginModel : BaseModel
    {
        private bool _isAdmin;
        public bool isAdmin {
            get {
                return _isAdmin;
            }
            set {
                if (_isAdmin != value) {
                    _isAdmin = value;
                    this.onPropertyChanged(nameof(isAdmin));
                }
            }
        }

        private string _userImage;
        public string userImage {
            get {
                return _userImage;
            }
            set {
                if (_userImage != value) {
                    _userImage = value;
                    this.onPropertyChanged(nameof(userImage));
                }
            }
        }

        private string _userName;
        public string userName {
            get {
                return _userName;
            }
            set {
                if (_userName != value) {
                    _userName = value;
                    this.onPropertyChanged(nameof(userName));
                }
            }
        }
    }
}
