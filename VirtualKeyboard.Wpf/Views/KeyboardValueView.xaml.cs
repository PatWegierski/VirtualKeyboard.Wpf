using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using VirtualKeyboard.Wpf.ViewModels;

namespace VirtualKeyboard.Wpf.Views
{
    /// <summary>
    /// Logika interakcji dla klasy KeyboardValueView.xaml
    /// </summary>
    partial class KeyboardValueView : UserControl
    {
        public KeyboardValueView()
        {
            InitializeComponent();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if(e.Key == Key.Delete || e.Key == Key.Back) return;
            if (this.DataContext is VirtualKeyboardViewModel vm)
            {
                e.Handled = true;
                if (e.Key == Key.Escape && vm.ShowDiscardButton)
                {
                    vm.Discard.Execute(null);
                    return;
                }

                if (e.Key == Key.Enter)
                {
                    vm.Accept.Execute(null);
                    return;
                }
                var charString = GetCharFromKey(e.Key)?.ToString();
                if (charString != null)
                {
                    if (vm.Regex == null)
                    {
                        vm.AddCharacter.Execute(charString);
                    }
                    else
                    {
                        if(vm.Regex.IsMatch(TextBox.Text + charString))
                            vm.AddCharacter.Execute(charString);
                    }
                }
            }
        }

        public enum MapType : uint
        {
            MAPVK_VK_TO_VSC = 0x0,
            MAPVK_VSC_TO_VK = 0x1,
            MAPVK_VK_TO_CHAR = 0x2,
            MAPVK_VSC_TO_VK_EX = 0x3,
        }

        [DllImport("user32.dll")]
        public static extern int ToUnicode(
            uint wVirtKey,
            uint wScanCode,
            byte[] lpKeyState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)] 
            StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags);

        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, MapType uMapType);

        public static char? GetCharFromKey(Key key)
        {
            char? ch = null;

            int virtualKey = KeyInterop.VirtualKeyFromKey(key);
            byte[] keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            uint scanCode = MapVirtualKey((uint)virtualKey, MapType.MAPVK_VK_TO_VSC);
            StringBuilder stringBuilder = new StringBuilder(2);

            int result = ToUnicode((uint)virtualKey, scanCode, keyboardState, stringBuilder, stringBuilder.Capacity, 0);
            switch (result)
            {
                case -1: 
                    break;
                case 0: 
                    break;
                case 1:
                {
                    ch = stringBuilder[0];
                    break;
                }
                default:
                {
                    ch = stringBuilder[0];
                    break;
                }
            }
            return ch;
        }
    }
}
