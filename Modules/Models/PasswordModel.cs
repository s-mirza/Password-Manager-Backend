/* Name: Sarah Mirza
 * ID: W10178836
 * Description: Developing the backend functionality for a basic password manager application.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC317PassManagerP2Starter.Modules.Models
{
    public class PasswordModel
    {
        // Unique identifier for each password entry.
        public int ID { get; set; }

        // Platform where the password is used (e.g., Facebook, Google, etc.).
        public string Platform { get; set; }

        // Username associated with the password.
        public string UserName { get; set; }

        // Encrypted password value.
        public string Password { get; set; }

        // Constructor to initialize the PasswordModel.
        public PasswordModel(int id, string platform, string userName, string password)
        {
            ID = id;
            Platform = platform;
            UserName = userName;
            Password = password; // Assuming you encrypt the password later.
        }

        // Default constructor.
        public PasswordModel() { }

        // Method to validate if the necessary fields are populated.
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Platform) &&
                   !string.IsNullOrWhiteSpace(UserName) &&
                   !string.IsNullOrWhiteSpace(Password);
        }

        // Encrypt the password before storing (simple example, use a real encryption algorithm).
        public string EncryptPassword(string password)
        {
            // Example of a simple encryption (this is for illustration, not secure for real use).
            byte[] data = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(data); // Encoding the password to Base64 (replace with real encryption).
        }

        // Decrypt the password when displaying it.
        public string DecryptPassword(string encryptedPassword)
        {
            // Example of a simple decryption (replace with real decryption logic).
            byte[] data = Convert.FromBase64String(encryptedPassword);
            return Encoding.UTF8.GetString(data);
        }

        // Method to update the password securely (use encryption).
        public void UpdatePassword(string newPassword)
        {
            Password = EncryptPassword(newPassword); // Encrypt the new password before storing.
        }

        // Sample method to display the password (using decryption).
        public string GetDecryptedPassword()
        {
            return DecryptPassword(Password);
        }

        // Override ToString() for easier display of the password model.
        public override string ToString()
        {
            return $"Platform: {Platform}, UserName: {UserName}, Password: {GetDecryptedPassword()}";
        }
    }
}
