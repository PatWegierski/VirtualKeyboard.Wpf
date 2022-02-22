using System.Windows;
using System.Windows.Controls;

namespace VirtualKeyboard.Wpf.Behaviors
{
    public static class KeyboardType
    {
        public static Types.KeyboardType GetType(TextBox obj)
        {
            return (Types.KeyboardType)obj.GetValue(KeyboardTypeProperty);
        }

        public static void SetType(TextBox obj, Types.KeyboardType value)
        {
            obj.SetValue(KeyboardTypeProperty, value);
        }

        public static readonly DependencyProperty KeyboardTypeProperty =
            DependencyProperty.RegisterAttached("Type", typeof(Types.KeyboardType), typeof(KeyboardType),
                new PropertyMetadata(Types.KeyboardType.Alphabet));
    }
}