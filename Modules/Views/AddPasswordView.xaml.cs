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

    public AddPasswordView()
    {
        InitializeComponent();

        // Stores the original color of the text boxes
        baseColorHighlight = (Color)Application.Current.Resources["ErrorEntryHighlightBG"];

        // Determines if a password was generated at least once
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
        // Generate a secure password based on selected criteria
        bool requireUpper = chkUpperLetter.IsChecked;
        bool requireDigit = chkDigit.IsChecked;
        bool requireSymbols = chkSymbols.IsChecked;
        string validSymbols = txtReqSymbols.Text ?? string.Empty;

        int minLength = (int)sprPassLength.Value;

        string generatedPassword = GenerateSecurePassword(requireUpper, requireDigit, requireSymbols, validSymbols, minLength);

        lblGenPassword.Text = generatedPassword;
        lblGenPassword.TextColor = Colors.Green;
        this.generatedPassword = true;
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
