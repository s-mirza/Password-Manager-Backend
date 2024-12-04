using CSC317PassManagerP2Starter.Modules;
using CSC317PassManagerP2Starter.Modules.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC317PassManagerP2Starter.Modules.Controllers
{
    public enum AuthenticationError { NONE, INVALIDUSERNAME, INVALIDPASSWORD }

    public class LoginController
    {
        private readonly User _user;
        private bool _loggedIn = false;

        public LoginController()
        {
            // Initialize the test user
            string password = "ab1234"; // Raw password
            var keyTuple = PasswordCrypto.GenKey(); // Generate encryption key and IV

            _user = new User
            {
                ID = 1,
                FirstName = "John",
                LastName = "Doe",
                UserName = "test",
                PasswordHash = PasswordCrypto.GetHash(password),
                Key = keyTuple.Item1,
                IV = keyTuple.Item2
            };
        }

        public User? CurrentUser
        {
            get
            {
                if (_loggedIn)
                {
                    // Return a copy of the user without exposing sensitive data
                    return new User
                    {
                        ID = _user.ID,
                        FirstName = _user.FirstName,
                        LastName = _user.LastName,
                        UserName = _user.UserName,
                        Key = _user.Key,
                        IV = _user.IV
                    };
                }
                return null; // User not logged in
            }
        }

        public AuthenticationError Authenticate(string username, string password)
        {
            // Validate the username
            if (username != _user.UserName)
            {
                return AuthenticationError.INVALIDUSERNAME;
            }

            // Validate the password by comparing the hashes
            byte[]? inputPasswordHash = PasswordCrypto.GetHash(password);
            if (!PasswordCrypto.CompareBytes(_user.PasswordHash, inputPasswordHash))
            {
                return AuthenticationError.INVALIDPASSWORD;
            }

            // Successful login
            _loggedIn = true;
            return AuthenticationError.NONE;
        }
    }
}
