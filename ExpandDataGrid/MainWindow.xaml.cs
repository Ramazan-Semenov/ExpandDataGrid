using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExpandDataGrid
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
       public ObservableCollection<Employee> employees { get; set; } = new ObservableCollection<Employee>();
        public ObservableCollection<string> TP { get; set; } = new ObservableCollection<string>();
        private string _selectedTP;
        public string SelectedTP { get {

                if (_selectedTP!=null)
                {
                    collectionView.Filter = item => { Employee employee = item as Employee; return employee.Name_TP == _selectedTP; };
                    NotifyPropertyChanged("collectionView");

                    //this.Source.Filter = item => {
                    //    ViewItem vitem = item as ViewItem;
                    //    return vItem != null && vitem.Name.Contains("A");
                    }
                return _selectedTP;
            
            } set => _selectedTP = value; }
        public string LoadData { get; set; }
       public CollectionView collectionView { get; set; } 

        public bool isexp { get; set; } = false;
       
        public MainWindow()
        {
            InitializeComponent();
            System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\lenovo\source\repos\ConsoleApp1\ConsoleApp1\Database1.mdf;Integrated Security=True;Connect Timeout=30");
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("Select * from dbo.EKS", sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {

                employees.Add(new Employee { 
                    PK=reader[0].ToString(),
                    KM = reader[1].ToString(), 
                    GR_20 = reader[2].ToString(),
                    GR_21 = reader[3].ToString(),
                    GR_22 = reader[4].ToString(),
                    GR_23 = reader[5].ToString(),
                    Name_TP = reader[6].ToString(),
                    Code_TP = reader[7].ToString(),
                    Type_Matrix = reader[8].ToString(),
                    id=Convert.ToInt32( reader[14])
                });
            }

            TP =  new ObservableCollection<string>( employees.Select(x => x.Name_TP).Distinct().ToList()) ;
            LoadData = "Начало загрузки";

            //employees.Add(new Employee { ID = 1, Name = "Mike", IsMale = true, Type = EmployeeType.Normal, SiteID = new Uri("http://localhost/4322"), BirthDate = new DateTime(1980, 1, 1) });
            //employees.Add(new Employee { ID = 2, Name = "George", IsMale = true, Type = EmployeeType.Manager, SiteID = new Uri("http://localhost/4432"), BirthDate = new DateTime(1984, 2, 1) });
            //employees.Add(new Employee { ID = 3, Name = "Vicky", IsMale = false, Type = EmployeeType.Supervisor, SiteID = new Uri("http://localhost/4872"), BirthDate = new DateTime(1975, 3, 1) });
            //employees.Add(new Employee { ID = 4, Name = "Michael", IsMale = true, Type = EmployeeType.Normal, SiteID = new Uri("http://localhost/4322"), BirthDate = new DateTime(1988, 1, 1) });
            //employees.Add(new Employee { ID = 5, Name = "Martin", IsMale = true, Type = EmployeeType.Normal, SiteID = new Uri("http://localhost/4432"), BirthDate = new DateTime(1989, 2, 1) });
            //employees.Add(new Employee { ID = 6, Name = "Lucy", IsMale = false, Type = EmployeeType.Supervisor, SiteID = new Uri("http://localhost/4872"), BirthDate = new DateTime(1967, 3, 1) });
            //employees.Add(new Employee { ID = 7, Name = "Brian", IsMale = true, Type = EmployeeType.Normal, SiteID = new Uri("http://localhost/4322"), BirthDate = new DateTime(1942, 1, 1) });
            //employees.Add(new Employee { ID = 8, Name = "Santa", IsMale = true, Type = EmployeeType.Normal, SiteID = new Uri("http://localhost/4432"), BirthDate = new DateTime(1976, 2, 1) });
            //employees.Add(new Employee { ID = 9, Name = "Ruby", IsMale = false, Type = EmployeeType.Normal, SiteID = new Uri("http://localhost/4872"), BirthDate = new DateTime(1990, 3, 1) });



            //collectionView.GroupDescriptions.Add(new PropertyGroupDescription("Type"));

            //dataGrid.ItemsSource = mycollection.View;
            //Thread thread = new Thread(() =>
            //{
            //    CollectionView collectionView;
            //    Console.WriteLine("000000000000000000000000000000");
            //    collectionView =/*(CollectionView) CollectionViewSource.GetDefaultView */new ListCollectionView(employees);

            //    App.Current.Dispatcher.Invoke(() =>
            //    {
            //        this.collectionView = collectionView;
            //        NotifyPropertyChanged("collectionView");
            //        Console.WriteLine("111111111111111111111");
            //        //gh.ItemsSource=

            //    });
            //    Console.WriteLine("2222222222222222222222222");

            //});
            //thread.Start();
            //  myDataGrid.ItemsSource = collectionView;
            ExcelToSqlServer();
            DataContext = this;

        }
        public class Employee
        {
            public int id { get; set; }
            public string PK {get;set;}
            public string KM {get;set;}
            public string GR_20{get;set;}
            public string GR_21 { get;set;}
            public string GR_22 { get;set;}
            public string GR_23 { get;set;}
            public string Name_TP {get;set;}
            public string Code_TP {get;set;}
            
            public string Type_Matrix {get;set;}
            public string OKR {get;set;}
            public string PC {get;set;}
        }
        private void ExcelToSqlServer()
        {


        }
        public enum EmployeeType
        {
            Normal,
            Supervisor,
            Manager
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            bool df = (sender as Expander).IsExpanded;
            isexp = df;
            Debug.WriteLine(isexp);
            NotifyPropertyChanged(nameof(isexp));
        }

        private void Expander_Expanded_1(object sender, RoutedEventArgs e)
        {
            bool df = (sender as Expander).IsExpanded;
            isexp = df;
            Debug.WriteLine(isexp);
            NotifyPropertyChanged(nameof(isexp));
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(  () =>  {
                ObservableCollection<Employee> employees = new ObservableCollection<Employee>();
                employees = this.employees;
                CollectionView collectionViewregion;
                //for (int i = 0; i < 2000; i++)
                //{
                //    employees.Add(new Employee { ID = 9, Name = "Ruby", IsMale = false, Type = EmployeeType.Normal, SiteID = new Uri("http://localhost/4872"), BirthDate = new DateTime(1990, 3, 1) });

                //}
                CollectionViewSource mycollection = new CollectionViewSource();

                mycollection.Source = employees;
                mycollection.GroupDescriptions.Add(new PropertyGroupDescription("Name_TP"));
                collectionViewregion = (CollectionView)mycollection.View;

                collectionView = collectionViewregion;
                LoadData = "Данные загружены";
              //  employees = null;
                this.Dispatcher.Invoke(()=> { 
                    NotifyPropertyChanged("collectionView");
                    NotifyPropertyChanged("LoadData");
                
                });
                //collectionViewregion = null;

            });

        }

        private void ListBox_Selected(object sender, RoutedEventArgs e)
        {

        }

        private  void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string sl = (sender as ComboBox).Text as string;

            //Dictionary<string, string> pairs = new Dictionary<string, string>();
            ////pairs.Add("ТП", "Name_TP");
            string b = "";
            //if (pairs[sl]!=null)
            //{
            //    b = pairs[sl];
            //}
            // Task.Run(() =>
            //{
                ObservableCollection<Employee> employees = new ObservableCollection<Employee>();
                employees = this.employees;
                CollectionView collectionViewregion;

                CollectionViewSource mycollection = new CollectionViewSource();

                mycollection.Source = employees;
                mycollection.GroupDescriptions.Add(new PropertyGroupDescription("Name_TP"));
                collectionViewregion = (CollectionView)mycollection.View;

                collectionView = collectionViewregion;
                LoadData = "Данные загружены";
                //  employees = null;
                this.Dispatcher.Invoke(() =>
                {
                    NotifyPropertyChanged("collectionView");
                    NotifyPropertyChanged("LoadData");

                });
                //collectionViewregion = null;
           // });
        }
    }
}
