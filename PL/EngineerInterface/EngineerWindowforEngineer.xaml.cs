using System.Windows;
using System.Windows.Controls;

namespace PL.EngineerInterface;

/// <summary>
/// Interaction logic for EngineerWindowForEngineer.xaml
/// </summary>
public partial class EngineerWindowForEngineer : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public BO.Engineer CurrentEngineer
    {
        get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
        set { SetValue(CurrentEngineerProperty, value); }
    }

    public static readonly DependencyProperty CurrentEngineerProperty =
        DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerWindowForEngineer), new PropertyMetadata(null));
    public EngineerWindowForEngineer(int Id = 0)
    {
        InitializeComponent();
        if (Id == 0)
        {
            CurrentEngineer = new BO.Engineer
            {
                Id = 0,
                Name = "",
                Email = "",
                Cost = 0,
                Level = BO.EngineerExperience.All,
                Task = null
            };
        }
        else
            CurrentEngineer = s_bl.Engineer.Read(Id);
    }

    int Id = 0;
    public BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.All;

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    /// <summary>
    /// add or update engineer values if their is an error in tha data it will theow a message of the problem 
    /// </summary>
    private void AddOrUpdate_Button(object sender, RoutedEventArgs e)
    {
        BO.Engineer engineer = new BO.Engineer
        {
            Id = CurrentEngineer.Id,
            Name = CurrentEngineer.Name,
            Level = CurrentEngineer.Level,
            Cost = CurrentEngineer.Cost,
            Task = null,
            Email = CurrentEngineer.Email
        };

        string? buttonText = (sender as Button)?.Content?.ToString();
        try
        {
            if (buttonText == "Add")
            {
                s_bl.Engineer.Create(engineer!);
                MessageBox.Show("The engineer was successfully added");
            }
            else if (buttonText == "Update")
            {
                s_bl.Engineer.Update(engineer!);
                MessageBox.Show("The engineer was successfully updated");
            }
            Close();
            new EngineerListWindowForEngineer().Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }

}
