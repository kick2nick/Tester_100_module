using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Models
{
    public class Led : INotifyPropertyChanged
    {
        private int _ledNum;
        [ColumnName("Led №")]
        [ReadOnly(true)]
        public int LedNum
        {
            get
            {
                return _ledNum;
            }
            set
            {
                if (value != _ledNum)
                {
                    _ledNum = value;
                    OnPropertyChanged("RectNum");
                }
            }
        }

        private int _orderNum;
        [ColumnName("Порядок")]
        public int OrderNum
        {
            get
            {
                return _orderNum;
            }
            set
            {
                if (value != _orderNum)
                {
                    _orderNum = value;
                    OnPropertyChanged("OrderNum");
                }
            }
        }

        private bool _found;
        [ReadOnly(true)]
        [ColumnName("Обнаружен")]
        public bool Found
        {
            get
            {
                return _found;
            }

            set
            {
                if (value != _found)
                {
                    _found = value;
                    OnPropertyChanged("Found");
                }
            }
        }

        private long _timeFromPrevious;
        [ReadOnly(true)]
        [ColumnName("Интервал, мс")]
        public long TimeFromPrevious
        {
            get
            {
                return _timeFromPrevious;
            }

            set
            {
                if (value != _timeFromPrevious)
                {
                    _timeFromPrevious = value;
                    OnPropertyChanged("Found");
                }
            }
        }


        private bool _coupled;
        [ColumnName("Одновременно")]
        public bool Coupled
        {
            get
            {
                return _coupled;
            }

            set
            {
                if (value != _coupled)
                {
                    _coupled = value;
                    OnPropertyChanged("Found");
                }
            }
        }


        [Display(AutoGenerateField = false)]
        public Rectangle LedRectangle { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public static void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            PropertyDescriptor propertyDescriptor = (PropertyDescriptor)e.PropertyDescriptor;
            e.Column.Header = propertyDescriptor.DisplayName;
            if (propertyDescriptor.DisplayName == "LedRectangle")
            {
                e.Cancel = true;
            }
        }
    }
}