using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VirtualKeyboard.Wpf.Controls
{
    class AdvancedTextBox : TextBox
    {
        public int CaretPosition
        {
            get { return (int)GetValue(CaretPositionProperty); }
            set { SetValue(CaretPositionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CaretPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CaretPositionProperty =
            DependencyProperty.Register("CaretPosition", typeof(int), typeof(AdvancedTextBox), 
                new PropertyMetadata(0, OnCaretPositionChange));

        public string SelectedValue
        {
            get { return (string)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register("SelectedValue", typeof(string), typeof(AdvancedTextBox), 
                new PropertyMetadata("", OnSelectedTextChange));

        public string TextValue
        {
            get { return (string)GetValue(TextValueProperty); }
            set { SetValue(TextValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextValueProperty =
            DependencyProperty.Register("TextValue", typeof(string), typeof(AdvancedTextBox), 
                new PropertyMetadata("", OnTextValueChange));

        public AdvancedTextBox()
        {
            SelectionChanged += AdvancedTextBox_SelectionChanged;
            TextChanged += (s, e) => SetValue(TextValueProperty, Text);
        }

        private void AdvancedTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            SetValue(CaretPositionProperty, CaretIndex);
            SetValue(SelectedValueProperty, SelectedText);
        }

        private static void OnCaretPositionChange(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            int? value = e.NewValue as int?;
            ((TextBox)sender).CaretIndex = value ?? 0;
        }

        private static void OnSelectedTextChange(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            string value = e.NewValue as string;
            ((TextBox)sender).SelectedText = value ?? "";
        }

        private static void OnTextValueChange(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var s = (AdvancedTextBox)sender;
            int caretPosition = s.CaretPosition;
            string value = e.NewValue as string;
            s.Text = value;
            s.CaretIndex = caretPosition <= value.Length ? caretPosition : value.Length;
        }
    }
}
