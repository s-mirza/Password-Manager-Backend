using System.Collections.ObjectModel;
using CSC317PassManagerP2Starter.Modules.Models;

namespace CSC317PassManagerP2Starter.Modules.Views;

public partial class PasswordListView : ContentPage
{
    private ObservableCollection<PasswordRow> _rows = new ObservableCollection<PasswordRow>();

    public PasswordListView()
    {
        InitializeComponent();

        // Generate test passwords when the view loads.
        App.PasswordController.GenTestPasswords();

        // Populate the list of passwords initially.
        App.PasswordController.PopulatePasswordView(_rows);

        // Bind the Collection View to the password rows.
        collPasswords.ItemsSource = _rows;
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
        // Get the ID of the selected password.
        int ID = Convert.ToInt32((sender as Button).CommandParameter);

        // Find the row corresponding to the ID.
        var passwordRow = _rows.FirstOrDefault(row => row.PasswordID == ID);

        if (passwordRow != null)
        {
            // Toggle the editing mode.
            passwordRow.Editing = !passwordRow.Editing;

            if (!passwordRow.Editing)
            {
                // Save the updated password details when exiting editing mode.
                App.PasswordController.UpdatePassword(new PasswordModel
                {
                    ID = passwordRow.PasswordID,
                    Platform = passwordRow.Platform,
                    UserName = passwordRow.PlatformUserName,
                    Password = passwordRow.PlatformPassword
                });
            }
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
