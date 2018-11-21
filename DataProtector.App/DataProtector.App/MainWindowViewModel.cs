using SimpleImpersonation;
using System;
using System.ComponentModel;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;

namespace DataProtector.App
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private string userName;
        private string userData;
        private string protectedUserData;

        private ICommand protectCommand;

        public string UserName
        {
            get => userName;
            set
            {
                if (userName != value)
                {
                    userName = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }

        public SecureString Password { get; set; }

        public string UserData
        {
            get => userData;
            set
            {
                if (userData != value)
                {
                    userData = value;
                    OnPropertyChanged(nameof(UserData));
                }
            }
        }

        public string ProtectedUserData
        {
            get => protectedUserData;
            set
            {
                if (protectedUserData != value)
                {
                    protectedUserData = value;
                    OnPropertyChanged(nameof(ProtectedUserData));
                }
            }
        }

        public ICommand ProtectCommand
        {
            get
            {
                return protectCommand ?? (protectCommand = new CommandHandler(Protect, true));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Protect()
        {
            var credentials = new UserCredentials(userName, Password);
            Impersonation.RunAsUser(credentials, LogonType.NewCredentials, () =>
            {
                var userDataBytes = Encoding.UTF8.GetBytes(userData);
                var protectedUserDataBytes = ProtectedData.Protect(userDataBytes, null, DataProtectionScope.CurrentUser);
                var protectedUserDataBytesAsBase64Str = Convert.ToBase64String(protectedUserDataBytes);
                ProtectedUserData = protectedUserDataBytesAsBase64Str;
                var unprotected = ProtectedData.Unprotect(protectedUserDataBytes, null, DataProtectionScope.CurrentUser);
                UserData = Encoding.UTF8.GetString(unprotected);
            });
        }
    }
}