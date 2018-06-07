using Curc.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Curc.Helpers
{
    public class Cache : BaseModel
    {
        #region Singleton instance
        private static Cache _instance;
        public static Cache instance {
            get {
                _instance = _instance ?? new Cache();
                return _instance;
            }
        }
        private Cache()
        {
            dic = App.Current.Properties;
        }
        #endregion

        private IDictionary<string, object> dic;

        public LoginModel loginModel {
            get {
                if (dic.ContainsKey("loginModel"))
                    return JsonConvert.DeserializeObject<LoginModel>(dic["loginModel"] as string);
                return null;
            }
            set {
                dic["loginModel"] = JsonConvert.SerializeObject(value);
                this.onPropertyChanged(nameof(loginModel));
            }
        }
    }
}
