/* Name: Sarah Mirza
 * ID: W10178836
 * Description: Developing the backend functionality for a basic password manager application.
 */

namespace CSC317PassManagerP2Starter.Modules.Views;
using CSC317PassManagerP2Starter.Modules.Controllers;
using CSC317PassManagerP2Starter.Modules.Models;
using Microsoft.Maui.Controls;



public partial class AddPasswordView : ContentPage
{
    private Color baseColorHighlight;
    private bool generatedPassword;

    public AddPasswordView(PasswordModel password = null)
    {
        InitializeComponent();

        // Stores the original color of the text boxes
        baseColorHighlight = (Color)Application.Current.Resources["ErrorEntryHighlightBG"];

        // If a password is provided, pre-fill the fields with its data
        if (password != null)
        {
            // Use the 'password' parameter correctly
            txtNewPlatform.Text = password.Platform; // Ensure 'Platform' is a property of 'PasswordModel'
            txtNewUserName.Text = password.UserName; // Ensure 'UserName' is a property of 'PasswordModel'

            // Create a PasswordRow instance and decrypt the password
            var passwordRow = new PasswordRow(password);  // Create an instance of PasswordRow
            txtNewPassword.Text = passwordRow.DecryptPassword(password.Password); // Decrypt using PasswordRow
            txtNewPasswordVerify.Text = txtNewPassword.Text;
        }

        generatedPassword = false;
    }


    private void ButtonCancel(object sender, EventArgs e)
    {
        // Navigate back to the previous page or the main password list view
        App.Current.MainPage = new PasswordListView();
    }

    private void ButtonSubmitExisting(object sender, EventArgs e)
    {
        //Called when the Submit button is clicked for a password manually
        //entered.  (i.e., existing password).
        if (string.IsNullOrWhiteSpace(txtNewPlatform.Text) ||
             string.IsNullOrWhiteSpace(txtNewUserName.Text) ||
             string.IsNullOrWhiteSpace(txtNewPassword.Text) ||
             txtNewPassword.Text != txtNewPasswordVerify.Text)
        {

            lblErrorExistingPassword.Text = "Invalid input or passwords do not match!";
            lblErrorExistingPassword.IsVisible = true;
            return;
        }


        App.PasswordController.AddPassword(
            txtNewPlatform.Text,
            txtNewUserName.Text,
            txtNewPassword.Text
        );

        Application.Current.MainPage = new PasswordListView();
    }

    private void ButtonSubmitGenerated(object sender, EventArgs e)
    {

        if (!generatedPassword || string.IsNullOrWhiteSpace(lblGenPassword.Text) || lblGenPassword.Text == "<No Password Generated>")
        {
            lblErrorGeneratedPassword.Text = "Please generate a password first.";
            lblErrorGeneratedPassword.IsVisible = true;
            return;
        }


        App.PasswordController.AddPassword(
            txtNewPlatform.Text,
            txtNewUserName.Text,
            lblGenPassword.Text
        );


        Application.Current.MainPage = new PasswordListView();
    }

 
    private void ButtonGeneratePassword(object sender, EventArgs e)
    {
        //Called when the Generate Password button is clicked.
        string newPassword = PasswordGeneration.BuildPassword(
            chkUpperLetter.IsChecked,
            chkDigit.IsChecked,
            txtReqSymbols.Text,
            (int)sprPassLength.Value
         );

        // Display the generated password
        lblGenPassword.Text = newPassword;
        generatedPassword = true;
        lblErrorGeneratedPassword.IsVisible = false;
    }

    private string GenerateSecurePassword(bool requireUpper, bool requireDigit, bool requireSymbols, string validSymbols, int minLength)
    {
        const string lower = "abcdefghijklmnopqrstuvwxyz";
        const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string digits = "0123456789";
        string symbols = validSymbols.Length > 0 ? validSymbols : "!@#$%^&*";

        string charSet = lower;
        if (requireUpper) charSet += upper;
        if (requireDigit) charSet += digits;
        if (requireSymbols) charSet += symbols;

        if (charSet.Length == 0)
            return "<Invalid Criteria>";

        var random = new Random();
        return new string(Enumerable.Repeat(charSet, minLength)
            .Select(s => s[random.Next(s.Length)])
            .ToArray());
    }

    private void ShowErrorMessage(Label errorLabel, string message)
    {
        errorLabel.Text = message;
        errorLabel.IsVisible = true;
    }
}
