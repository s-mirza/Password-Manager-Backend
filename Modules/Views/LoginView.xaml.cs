namespace CSC317PassManagerP2Starter.Modules.Views;
using CSC317PassManagerP2Starter.Modules.Controllers; 
using CSC317PassManagerP2Starter.Modules.Models; 
using Microsoft.Maui.Controls;

public partial class LoginView : ContentPage
{
    public LoginView()
    {
        InitializeComponent();
    }

    private void ProcessLogin(object sender, EventArgs e)
    {
        // Retrieve the username and password entered by the user
        string username = txtUserName.Text; // Bind to x:Name="txtUserName" in XAML
        string password = txtPassword.Text; // Bind to x:Name="txtPassword" in XAML

        // Ensure fields are not empty
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ShowErrorMessage("Username and password cannot be empty.");
            return;
        }

        // Authenticate using the LoginController
        var authenticationResult = App.LoginController.Authenticate(username, password);

        // Handle authentication result
        switch (authenticationResult)
        {
            case AuthenticationError.NONE:
                // Navigate to PasswordListView on successful login
                App.Current.MainPage = new PasswordListView(); // Adjust navigation logic as needed
                break;

            case AuthenticationError.INVALIDUSERNAME:
                ShowErrorMessage("Invalid username. Please try again.");
                break;

            case AuthenticationError.INVALIDPASSWORD:
                ShowErrorMessage("Invalid password. Please try again.");
                break;

            default:
                ShowErrorMessage("An unexpected error occurred. Please try again.");
                break;
        }
    }

    private void ShowErrorMessage(string message)
    {
        // Update the error label with the provided message
        lblError.Text = message; // Bind to x:Name="lblError" in XAML
        lblError.IsVisible = true; // Make the error label visible
    }
}
