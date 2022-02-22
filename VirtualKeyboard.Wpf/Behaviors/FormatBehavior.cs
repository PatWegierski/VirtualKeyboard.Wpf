using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VirtualKeyboard.Wpf.Behaviors
{
    public static class FormatBehavior
    {
        public static Format GetFormat(TextBox obj)
        {
            return (Format)obj.GetValue(FormatProperty);
        }

        public static void SetFormat(TextBox obj, Format value)
        {
            obj.SetValue(FormatProperty, value);
        }

        public static readonly DependencyProperty FormatProperty =
            DependencyProperty.RegisterAttached("Format", typeof(Format), typeof(FormatBehavior),
                new PropertyMetadata(Format.Alphabet, OnFormatChanged));

        private static void OnFormatChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                var isDigitOnly = (Format)(e.NewValue);

                switch (isDigitOnly)
                {
                    case Format.Decimal:
                    case Format.Integer:
                        textBox.PreviewTextInput += BlockCharacters;
                        break;
                    default:
                        textBox.PreviewTextInput -= BlockCharacters;
                        break;
                }
            }

        }

        private static void BlockCharacters(object sender, TextCompositionEventArgs e)
        {
            var textBox = ((TextBox)e.Source);
            var format = (Format) textBox.GetValue(FormatProperty);
            var tmpText = textBox.Text;
            if (!string.IsNullOrEmpty(textBox.SelectedText))
            {
                var position = tmpText.IndexOf(textBox.SelectedText);
                tmpText = tmpText.Remove(position, textBox.SelectedText.Length);
            }

            e.Handled = !format.Match(tmpText.Insert(textBox.CaretIndex, e.Text));
        }
    }
}