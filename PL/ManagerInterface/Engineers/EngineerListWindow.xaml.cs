using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace PL.Engineer;

/// <summary>
/// Interaction logic for EngineerListWindow.xaml
/// </summary>
public partial class EngineerListWindow : Window 
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public EngineerListWindow()
    {
        InitializeComponent();
        EngineerList = s_bl.Engineer.ReadAll()!;
    }
    public IEnumerable<BO.Engineer> EngineerList
    {
        get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
        set { SetValue(EngineerListProperty, value); }

    }
    
    public static readonly DependencyProperty EngineerListProperty =
        DependencyProperty.Register("EngineerList",
        typeof(IEnumerable<BO.Engineer>),
        typeof(EngineerListWindow),
        new PropertyMetadata(null));

    public BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.All;
    
    private void cbEngineerExperienceSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        EngineerList = (Level == BO.EngineerExperience.All) ?
            (s_bl?.Engineer.ReadAll()!) : (s_bl?.Engineer.ReadAll(item => item.Level == Level)!);
    }

    private void click_update(object sender, MouseButtonEventArgs e)
    {
        BO.Engineer? engineer = (sender as ListView)?.SelectedItem as BO.Engineer;
        if (engineer != null)
        {
            this.Close();
            Window Update = new EngineerWindow(engineer.Id);
       
            
            Update.ShowDialog();
            EngineerList=s_bl.Engineer.ReadAll();

        }
        
    }

    private void add_engineer(object sender, RoutedEventArgs e)
    {
        this.Close();
        new EngineerWindow().ShowDialog();
        EngineerList = s_bl.Engineer.ReadAll();
    }
}
