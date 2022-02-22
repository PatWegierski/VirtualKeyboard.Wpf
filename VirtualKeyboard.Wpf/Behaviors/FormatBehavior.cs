using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VirtualKeyboard.Wpf.Behaviors
{
    public static class FormatBehavior
    {
        public static string GetRegex(TextBox obj)
        {
            return (string)obj.GetValue(FormatProperty);
        }

        public static void SetRegex(TextBox obj, string value)
        {
            obj.SetValue(RegexProperty, value);
        }

        public static Format? GetFormat(TextBox obj)
        {
            return (Format)obj.GetValue(FormatProperty);
        }

        public static void SetFormat(TextBox obj, Format? value)
        {
            obj.SetValue(FormatProperty, value);
        }

        public static readonly DependencyProperty RegexProperty =
            DependencyProperty.RegisterAttached("Regex", typeof(string), typeof(FormatBehavior),
                new PropertyMetadata(null, OnRegexChanged));

        public static readonly DependencyProperty FormatProperty =
            DependencyProperty.RegisterAttached("Format", typeof(Format?), typeof(FormatBehavior),
                new PropertyMetadata(null, OnFormatChanged));

        private static void OnRegexChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                var regex = (string)e.NewValue;
                if (!string.IsNullOrWhiteSpace(regex))
                {
                    textBox.PreviewTextInput += BlockCharactersRegex;
                }
                else
                {
                    textBox.PreviewTextInput -= BlockCharactersRegex;
                }
            }
        }

        private static void OnFormatChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (e.NewValue == null)
                {
                    textBox.PreviewTextInput -= BlockCharacters;
                }
                else
                {
                    var format = (Format)e.NewValue;

                    switch (format)
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
        }

        private static void BlockCharactersRegex(object sender, TextCompositionEventArgs e)
        {
            var textBox = ((TextBox)e.Source);
            var format = (string) textBox.GetValue(RegexProperty);
            var tmpText = textBox.Text;
            if (!string.IsNullOrEmpty(textBox.SelectedText))
            {
                var position = tmpText.IndexOf(textBox.SelectedText, StringComparison.Ordinal);
                tmpText = tmpText.Remove(position, textBox.SelectedText.Length);
            }

            e.Handled = !Regex.IsMatch(tmpText.Insert(textBox.CaretIndex, e.Text), format);
        }

        private static void BlockCharacters(object sender, TextCompositionEventArgs e)
        {
            var textBox = ((TextBox)e.Source);
            if (textBox.GetValue(RegexProperty) != null)
            {
                e.Handled = false;
                return;
            }

            var tmpText = textBox.Text;
            if (!string.IsNullOrEmpty(textBox.SelectedText))
            {
                var position = tmpText.IndexOf(textBox.SelectedText, StringComparison.Ordinal);
                tmpText = tmpText.Remove(position, textBox.SelectedText.Length);
            }

            var format = (Format)textBox.GetValue(FormatProperty);
            e.Handled = !format.Match(tmpText.Insert(textBox.CaretIndex, e.Text));
        }
    }
}