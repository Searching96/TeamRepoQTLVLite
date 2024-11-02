using System;
using System.Windows;
using System.Windows.Controls;

namespace Library_GUI.UserControls
{
    public partial class MyDatePicker : UserControl
    {
        public MyDatePicker()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(MyDatePicker));

        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }
        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(MyDatePicker));
    }
} 