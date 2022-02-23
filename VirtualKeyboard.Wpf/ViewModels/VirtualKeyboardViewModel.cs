using System.ComponentModel;
using System.Text.RegularExpressions;
using VirtualKeyboard.Wpf.Types;

namespace VirtualKeyboard.Wpf.ViewModels
{
    class VirtualKeyboardViewModel : INotifyPropertyChanged
    {
        public Regex Regex { get; }

        public bool Accepted { get; private set; }

        private bool _showDiscardButton;
        public bool ShowDiscardButton
        {
            get => _showDiscardButton;
            set
            {
                _showDiscardButton = value;
                NotifyPropertyChanged(nameof(ShowDiscardButton));
            }
        }

        private string _keyboardText;
        public string KeyboardText {
            get => _keyboardText;
            set
            {
                _keyboardText = value;
                NotifyPropertyChanged(nameof(KeyboardText));
            }
        }
        private KeyboardType _keyboardType;
        public KeyboardType KeyboardType {
            get => _keyboardType; 
            private set
            {
                _keyboardType = value;
                NotifyPropertyChanged(nameof(KeyboardType));
            }
        }
        private bool _uppercase;
        public bool Uppercase
        {
            get => _uppercase;
            private set
            {
                _uppercase = value;
                NotifyPropertyChanged(nameof(Uppercase));
            }
        }
        private int _caretPosition;
        public int CaretPosition
        {
            get => _caretPosition;
            set
            {
                if (value < 0) _caretPosition = 0;
                else if (value > KeyboardText.Length) _caretPosition = KeyboardText.Length;
                else _caretPosition = value;
                NotifyPropertyChanged(nameof(CaretPosition));
            }
        }
        private string _selectedValue;
        public string SelectedValue
        {
            get => _selectedValue;
            set
            {
                _selectedValue = value;
                NotifyPropertyChanged(nameof(SelectedValue));
            }
        }
        public Command AddCharacter { get; }
        public Command ChangeCasing { get; }
        public Command RemoveCharacter { get; }
        public Command ChangeKeyboardType { get; }
        public Command Accept { get; }
        public Command Discard { get; }

        public VirtualKeyboardViewModel(string initialValue, KeyboardType type, string regex = null, Format? format = null)
        {
            _keyboardText = initialValue;
            _keyboardType = type;
            Regex = regex == null ? null : new Regex(regex);
            if (Regex == null && format != null)
            {
                Regex = format.Value.GetRegex();
            }

            if (Regex == null)
            {
                switch (_keyboardType)
                {
                    case KeyboardType.NumericOnly:
                        Regex = Format.Decimal.GetRegex();
                        break;
                    case KeyboardType.Alphabet:
                        Regex = Format.Alphanumeric.GetRegex();
                        break;
                }
            }
            _uppercase = false;
            CaretPosition = _keyboardText.Length;
            AddCharacter = new Command(a =>
            {
                if (!(a is string character) || character.Length != 1) return;

                if (Uppercase) character = character.ToUpper();

                var tmpText = KeyboardText;
                if (!string.IsNullOrEmpty(SelectedValue))
                {
                    tmpText = RemoveSubstring(SelectedValue);
                }

                tmpText = tmpText.Insert(CaretPosition, character);

                if (Regex != null)
                {
                    if(!Regex.IsMatch(tmpText)) return;
                }
                KeyboardText = tmpText;
                CaretPosition++;

                if (!string.IsNullOrEmpty(SelectedValue))
                {
                    SelectedValue = string.Empty;
                }
            });
            ChangeCasing = new Command(a => Uppercase = !Uppercase);
            RemoveCharacter = new Command(a =>
            {
                if(!string.IsNullOrEmpty(SelectedValue))
                {
                    KeyboardText = RemoveSubstring(SelectedValue);
                }
                else
                {
                    var position = CaretPosition - 1;
                    if (position >= 0)
                    {
                        KeyboardText = KeyboardText.Remove(position, 1);
                        if (position < KeyboardText.Length) CaretPosition--;
                        else CaretPosition = KeyboardText.Length;
                    }
                }
            });
            ChangeKeyboardType = new Command(a =>
            {
                if (KeyboardType == KeyboardType.Alphabet) KeyboardType = KeyboardType.Special;
                else KeyboardType = KeyboardType.Alphabet;
            });
            Accept = new Command(a =>
            {
                Accepted = true;
                VKeyboard.Close();
            });
            Discard = new Command(a =>
            {
                Accepted = false;
                VKeyboard.Close();
            });
        }

        private string RemoveSubstring(string substring)
        {
            var position = KeyboardText.IndexOf(substring);
            return KeyboardText.Remove(position, substring.Length);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
