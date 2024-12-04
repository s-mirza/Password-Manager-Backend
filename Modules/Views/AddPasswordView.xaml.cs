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
        // Validate the existing password fields
        string platform = txtNewPlatform.Text;
        string username = txtNewUserName.Text;
        string password = txtNewPassword.Text;
        string passwordVerify = txtNewPasswordVerify.Text;

        if (string.IsNullOrWhiteSpace(platform) || string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(passwordVerify))
        {
            ShowErrorMessage(lblErrorExistingPassword, "All fields must be filled out.");
            return;
        }

        if (password != passwordVerify)
        {
            ShowErrorMessage(lblErrorExistingPassword, "Passwords do not match.");
            return;
        }

        // Add the password using the PasswordController
        App.PasswordController.AddPassword(platform, username, password);
        App.Current.MainPage = new PasswordListView();
    }

    private void ButtonSubmitGenerated(object sender, EventArgs e)
    {
        if (!generatedPassword)
        {
            ShowErrorMessage(lblErrorGeneratedPassword, "You must generate a password before submitting.");
            return;
        }

        string platform = txtNewPlatform.Text;
        string username = txtNewUserName.Text;
        string password = lblGenPassword.Text;

        if (string.IsNullOrWhiteSpace(platform) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ShowErrorMessage(lblErrorGeneratedPassword, "All fields must be filled out.");
            return;
        }

        // Add the password using the PasswordController
        App.PasswordController.AddPassword(platform, username, password);
        App.Current.MainPage = new PasswordListView();
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
