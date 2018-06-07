using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Curc.Abstractions
{
    public interface ILocation
    {
        void startLocationUpdate(ICommand locationUpdateCommand);
        void stopLocationUpdate();
    }
}
