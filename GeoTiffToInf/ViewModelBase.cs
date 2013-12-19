using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GeoTiffToInf {
    class ViewModelBase : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null) {
            if (EqualityComparer<T>.Default.Equals(field, value)) {
                return false;
            }
            else {
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }
        }
    }
}
