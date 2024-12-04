/* Name: Sarah Mirza
 * ID: W10178836
 * Description: Developing the backend functionality for a basic password manager application.
 */

using System;
using System.ComponentModel;
using CSC317PassManagerP2Starter.Modules.Models;

namespace CSC317PassManagerP2Starter.Modules.Views
{
    public class PasswordRow : BindableObject, INotifyPropertyChanged
    {
        private PasswordModel _pass;
        private bool _isVisible = false;
        private bool _editing = false;

        public PasswordRow(PasswordModel source)
        {
            _pass = source ?? throw new ArgumentNullException(nameof(source));
        }

        // Platform binding property
        public string Platform
        {
            get
            {
                // Return the platform name from the PasswordModel
                return _pass?.Platform ?? string.Empty;
            }
            set
            {
                // Update the platform in the PasswordModel and refresh the row
                if (_pass.Platform != value)
                {
                    _pass.Platform = value;
                    RefreshRow();
                }
            }
        }

        // Username binding property
        public string PlatformUserName
        {
            get
            {
                // Return the username from the PasswordModel
                return _pass?.UserName ?? string.Empty;
            }
            set
            {
                // Update the username in the PasswordModel and refresh the row
                if (_pass.UserName != value)
                {
                    _pass.UserName = value;
                    RefreshRow();
                }
            }
        }

        // Password binding property
        public string PlatformPassword
        {
            get
            {
                // Show the password if IsShown is true; otherwise, return "<hidden>"
                return IsShown ? _pass?.Password ?? string.Empty : "<hidden>";
            }
            set
            {
                // Update the password temporarily in the row
                if (_pass.Password != value)
                {
                    _pass.Password = value;
                    RefreshRow();
                }
            }
        }


        // Password ID binding property
        public int PasswordID
        {
            get
            {
                // Return the password ID from the PasswordModel
                return _pass?.ID ?? -1;
            }
        }

        // Show Password toggle binding property
        public bool IsShown
        {
            get => _isVisible;
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    RefreshRow();
                }
            }
        }


        // Editing mode toggle binding property
        public bool Editing
        {
            get
            {
                // Return the editing mode status
                return _editing;
            }
            set
            {
                // Set the editing mode status and refresh the row
                if (_editing != value)
                {
                    _editing = value;
                    RefreshRow();
                }
            }
        }

        // Called when a bound property changes, notifying the view to update
        private void RefreshRow()
        {
            OnPropertyChanged(nameof(Platform));
            OnPropertyChanged(nameof(PlatformUserName));
            OnPropertyChanged(nameof(PlatformPassword));
            OnPropertyChanged(nameof(IsShown));
            OnPropertyChanged(nameof(Editing));
        }

        // Decrypts the password (implement actual decryption logic here)
        private string DecryptPassword(string encryptedPassword)
        {
            // For simplicity, we'll just return the password as-is.
            // Replace this with actual decryption logic if needed.
            return encryptedPassword;
        }

        // Saves the changes to the password model data
        public void SavePassword()
        {
            if (Editing)
            {
                // Commit the changes to the model
                _editing = false;
                RefreshRow();
            }
        }

        // Event handler for property changes (for INotifyPropertyChanged)
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
