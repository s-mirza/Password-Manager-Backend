/* Name: Sarah Mirza
 * ID: W10178836
 * Description: Developing the backend functionality for a basic password manager application.
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSC317PassManagerP2Starter.Modules.Controllers;
using CSC317PassManagerP2Starter.Modules.Models;
using CSC317PassManagerP2Starter.Modules.Views;

namespace CSC317PassManagerP2Starter.Modules.Controllers
{
    public class PasswordController
    {
        // Stores a list of sample passwords for the test user.
        public List<PasswordModel> _passwords = new List<PasswordModel>();
        private int counter = 1;

        // Copies the passwords over to the Row Binders.
        public void PopulatePasswordView(ObservableCollection<PasswordRow> source, string search_criteria = "")
        {
            source.Clear(); // Clear the existing rows.

            var filteredPasswords = string.IsNullOrWhiteSpace(search_criteria)
                ? _passwords
                : _passwords.Where(p =>
                      p.Platform.Contains(search_criteria, StringComparison.OrdinalIgnoreCase) ||
                      p.UserName.Contains(search_criteria, StringComparison.OrdinalIgnoreCase));

            foreach (var password in filteredPasswords)
            {
                source.Add(new PasswordRow(password));  // Pass PasswordModel directly to PasswordRow constructor
            }

            Console.WriteLine($"Populated Password Rows: {source.Count}"); // Debug output
        }



        // Adds a new password to the list.
        public void AddPassword(string platform, string username, string password)
        {
            var newPassword = new PasswordModel
            {
                ID = counter++, // Auto-increment ID.
                Platform = platform,
                UserName = username,
                Password = password
            };

            _passwords.Add(newPassword);
        }

        // Retrieves a password by ID.
        public PasswordModel? GetPassword(int ID)
        {
            return _passwords.FirstOrDefault(p => p.ID == ID);
        }

        // Updates an existing password entry.
        public bool UpdatePassword(PasswordModel changes)
        {
            var password = GetPassword(changes.ID);

            if (password != null)
            {
                password.Platform = changes.Platform;
                password.UserName = changes.UserName;
                password.Password = changes.Password;
                return true;
            }

            return false;
        }

        // Removes a password by ID.
        public bool RemovePassword(int ID)
        {
            var password = GetPassword(ID);

            if (password != null)
            {
                _passwords.Remove(password);
                return true;
            }

            return false;
        }

        // Generates a set of random passwords for the test user.
        public void GenTestPasswords()
        {
            Console.WriteLine("GenTestPasswords called.");
            _passwords.Clear();
            counter = 1; // Reset the counter.

            var testPlatforms = new[] { "Google", "Facebook", "Twitter", "LinkedIn", "Instagram" };
            var testUsernames = new[] { "user1", "user2", "admin", "test", "example" };

            var random = new Random();
            for (int i = 0; i < 5; i++)
            {
                string platform = testPlatforms[random.Next(testPlatforms.Length)];
                string username = testUsernames[random.Next(testUsernames.Length)];
                string password = GenerateRandomPassword();

                AddPassword(platform, username, password);
                Console.WriteLine($"Added Password: {platform}, {username}, {password}"); // Debug output
            }
        }

        private string GenerateRandomPassword(int length = 12)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }



    }
}
