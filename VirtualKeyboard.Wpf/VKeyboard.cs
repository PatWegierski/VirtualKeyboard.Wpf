using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VirtualKeyboard.Wpf.Controls;
using VirtualKeyboard.Wpf.ViewModels;
using VirtualKeyboard.Wpf.Views;

namespace VirtualKeyboard.Wpf
{
    public static class VKeyboard
    {
        private const string _keyboardValueName = "KeyboardValueContent";
        private const string _keyboardName = "KeyboardContent";

        private static Type _hostType = typeof(DefaultKeyboardHost);

        private static TaskCompletionSource<string> _tcs;
        private static Window _windowHost;

        public static bool ShowDiscardButton { get; set; }

        public static void Config(Type hostType)
        {
            if (hostType.IsSubclassOf(typeof(Window))) _hostType = hostType;
            else throw new ArgumentException();
        }

        public static void Listen<T>(Expression<Func<T, string>> property) where T: UIElement
        {
            EventManager.RegisterClassHandler(typeof(T), UIElement.PreviewMouseLeftButtonDownEvent, (RoutedEventHandler)(async (s, e) =>
            {
                if (s is AdvancedTextBox) return;
                var memberSelectorExpression = property.Body as MemberExpression;
                if (memberSelectorExpression == null) return;
                var prop = memberSelectorExpression.Member as PropertyInfo;
                if (prop == null) return;
                var initValue = (string)prop.GetValue(s);
                var value = await OpenAsync(initValue);
                prop.SetValue(s, value, null);
            }));
        }

        public static Task<string> OpenAsync(string initialValue = "")
        {
            if (_windowHost != null) throw new InvalidOperationException();

            _tcs = new TaskCompletionSource<string>();
            _windowHost = (Window)Activator.CreateInstance(_hostType);
            var viewModel = new VirtualKeyboardViewModel(initialValue)
            {
                ShowDiscardButton = ShowDiscardButton
            };
            _windowHost.DataContext = viewModel;
            ((ContentControl)_windowHost.FindName(_keyboardValueName)).Content = new KeyboardValueView();
            ((ContentControl)_windowHost.FindName(_keyboardName)).Content = new VirtualKeyboardView();
            void handler(object s, CancelEventArgs a)
            {
                ((Window)s).Closing -= handler;

                if (IsAccepted())
                {
                    var result = GetResult();
                    _tcs?.SetResult(result);
                }
                
                _windowHost = null;
                _tcs = null;
            }

            _windowHost.Closing += handler;

            _windowHost.Owner = Application.Current.MainWindow;
            _windowHost.Show();
            return _tcs.Task;
        }

        public static void Close()
        {
            if (_windowHost == null) throw new InvalidOperationException();
            
            _windowHost.Close();
        }

        private static bool IsAccepted()
        {
            var viewModel = (VirtualKeyboardViewModel)_windowHost.DataContext;
            return viewModel.Accepted;
        }

        private static string GetResult()
        {
            var viewModel = (VirtualKeyboardViewModel)_windowHost.DataContext;
            return viewModel.KeyboardText;
        }
    }
}
