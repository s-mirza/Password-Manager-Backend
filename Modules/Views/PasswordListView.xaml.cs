/* Name: Sarah Mirza
 * ID: W10178836
 * Description: Developing the backend functionality for a basic password manager application.
 */


using System.Collections.ObjectModel;
using CSC317PassManagerP2Starter.Modules.Models;

namespace CSC317PassManagerP2Starter.Modules.Views;

public partial class PasswordListView : ContentPage
{
    private ObservableCollection<PasswordRow> _rows = new ObservableCollection<PasswordRow>();

    public PasswordListView()
    {
        InitializeComponent();

        Console.WriteLine("PasswordListView Constructor Called");

        // Generate test passwords when the view loads.
        App.PasswordController.GenTestPasswords();

        // Populate the list of passwords initially.
        App.PasswordController.PopulatePasswordView(_rows);

        // Bind the Collection View to the password rows.
        collPasswords.ItemsSource = _rows;

        Console.WriteLine($"Total Password Rows: {_rows.Count}");
    }

    private void TextSearchBar(object sender, TextChangedEventArgs e)
    {
        // Update the list based on the search criteria entered in the search bar.
        string searchCriteria = e.NewTextValue ?? string.Empty;
        App.PasswordController.PopulatePasswordView(_rows, searchCriteria);
    }

    private void CopyPassToClipboard(object sender, EventArgs e)
    {
        // Get the ID of the selected password.
        int ID = Convert.ToInt32((sender as Button).CommandParameter);

        // Look up the password by its ID.
        var passwordModel = App.PasswordController.GetPassword(ID);

        if (passwordModel != null)
        {
            // Copy the password to the clipboard.
            Clipboard.Default.SetTextAsync(passwordModel.Password);
            DisplayAlert("Success", "Password copied to clipboard.", "OK");
        }
        else
        {
            DisplayAlert("Error", "Failed to copy password.", "OK");
        }
    }


    private void EditPassword(object sender, EventArgs e)
    {
        // Get the button that generated the event
        var btnSender = sender as Button;
        if (btnSender == null) return;

        // Retrieve the ID of the password row
        int ID = Convert.ToInt32(btnSender.CommandParameter);

        // Get the password row by ID
        var passwordRow = _rows.FirstOrDefault(row => row.PasswordID == ID);
        if (passwordRow == null)
        {
            DisplayAlert("Error", "Password not found.", "OK");
            return;
        }

        // Toggle between Edit and Save modes
        if (passwordRow.Editing)
        {
            // Save changes and switch to ReadOnly mode
            passwordRow.SavePassword();
            btnSender.Text = "Edit"; // Update button text
        }
        else
        {
            // Switch to Edit mode
            passwordRow.Editing = true; // Enable edit mode
            btnSender.Text = "Save"; // Update button text
        }
    }

    private void DeletePassword(object sender, EventArgs e)
    {
        // Get the ID of the selected password.
        int ID = Convert.ToInt32((sender as Button).CommandParameter);

        // Remove the password from the controller.
        bool success = App.PasswordController.RemovePassword(ID);

        if (success)
        {
            // Remove the corresponding row from the UI.
            var passwordRow = _rows.FirstOrDefault(row => row.PasswordID == ID);
            if (passwordRow != null)
            {
                _rows.Remove(passwordRow);
            }

            DisplayAlert("Success", "Password deleted.", "OK");
        }
        else
        {
            DisplayAlert("Error", "Failed to delete password.", "OK");
        }
    }

    private void ButtonAddPassword(object sender, EventArgs e)
    {
        // Navigate to the AddPasswordView page.
        App.Current.MainPage = new AddPasswordView();
    }
}
