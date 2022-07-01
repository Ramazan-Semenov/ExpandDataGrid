using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExpandDataGrid
{
    public static class d
    {
        private static readonly FieldInfo StorageField = typeof(DataColumn).GetField("_storage",
           BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly FieldInfo ValuesField =
            typeof(DataTable).Assembly.GetType("System.Data.Common.StringStorage")
                .GetField("values", BindingFlags.Instance | BindingFlags.NonPublic);
        public static DataTable Compact(this DataTable table)
        {
            if (table.Rows.Count == 0)
                return table;

            var exclusiveStrings = new Dictionary<string, string>();
            for (int column = 0; column < table.Columns.Count; ++column)
            {
                if (table.Columns[column].DataType == typeof(string))
                {
                    // Прямой доступ к массиву (вертикальное хранилище)
                    var values = (string[])ValuesField.GetValue(StorageField.GetValue(table.Columns[column]));
                    int rowCount = table.Rows.Count;
                    for (int row = 0; row < rowCount; ++row)
                    {
                        var value = values[row];
                        if (value != null)
                        {
                            string hashed;
                            if (!exclusiveStrings.TryGetValue(value, out hashed))
                                // строка встречается впервые
                                exclusiveStrings.Add(value, value);
                            else
                                // дубликат
                                values[row] = hashed;
                        }
                    }
                    exclusiveStrings.Clear();
                }
            }
            return table;
        }
    }
    public class t
    {
        public int id { get; set; } = 1;
        public int MyProperty { get; set; } = 2;
        public string Txt { get; set; } = "sdffffffffffffffffffffffffffffffffffffffffff" +
            "ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff" +
            "ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff" +
            "ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff" +
            "ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff" +
            "fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff";
    }
    public partial class MainWindow : Window
    {
        public DataTable Data { get; set; }
        public ObservableCollection<t> Ts { get; set; }
        // public CollectionView DataC { get; set; }
        public ObservableCollection<KeyValuePair<string,object>> keyValues = new ObservableCollection<System.Collections.Generic.KeyValuePair<string, object>>();
        public MainWindow()
        {
            InitializeComponent();

            SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=C:\USERS\LENOVO\SOURCE\REPOS\CONSOLEAPP1\CONSOLEAPP1\DATABASE1.MDF;Integrated Security=True");
            connection.Open();
             SqlCommand sqlDataAdapter = new SqlCommand(@"select * from( select * from dbo.EKS) as t
pivot(
max( [Формат])
for [Неделя] in ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[12],[13],[14],[15],[17],[18],[19],[20])
) as prt", connection);

            //SqlDataReader sqlDataRead = sqlDataAdapter.ExecuteReader();
            SqlDataAdapter adapter = new SqlDataAdapter(sqlDataAdapter);
            Data = new DataTable();
            adapter.Fill(Data);

            connection.Close();


             DataContext = this;
          
        }

        private void e_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
        }

        private void e_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void er_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

        }
        int selectedColumnIndex=0;
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            //selectedColumnIndex = 0;
            //DataGridColumnHeader columnHeader = DataGridHelper.GetColumnHeader(er, selectedColumnIndex);
            //Collection<TextBlock> textblocks = VisualTreeHelper.GetVisualChildren<TextBlock>(columnHeader);
            //TextBlock textblock = textblocks[0];
            //textblock.MouseLeftButtonDown += new MouseButtonEventHandler(textblock_MouseLeftButtonDown);
        }
        void textblock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
          
        }

        private void er_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        public IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            List<DataGridRow> dataGrids = new List<DataGridRow>();
            for (int i = 0; i < grid.Items.Count; i++)
            {
                var row = grid.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                if (null != row)
                {
                    dataGrids.Add(row);
                }
                /* yield return row;*/
            }
            return dataGrids;
        }
        bool isselectbutton = false;
        int indexcolumn = 0;


        private void er_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (isselectbutton)
            {
                var rows = GetDataGridRows(er);
                //  int index = 0;
                foreach (DataGridRow r in rows)
                {
                    if (r != null)
                    {
                        //   index++;
                        foreach (var item in ColorCol)
                        {
                            (er.Columns[item].GetCellContent(r) as TextBlock).Background = Brushes.Red;

                        }

                    }
                }
                //    Debug.WriteLine(index);
            }
            else
            {
                var rows = GetDataGridRows(er);

                foreach (DataGridRow r in rows)
                {
                    if (r != null)
                    {
                        foreach (var item in ColorNull)
                        {
                            (er.Columns[item].GetCellContent(r) as TextBlock).Background = Brushes.Transparent;
                        }
                    }
                }
            }

            // MessageBox.Show(index.ToString());
        }

        List<int> ColorCol = new List<int>();
        List<int> ColorNull = new List<int>();
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var index = er.Columns.Single(c => c.Header.ToString() == (sender as CheckBox).Content.ToString()).DisplayIndex;
            indexcolumn = index;
            ColorCol.Add(index);
            ColorNull.Remove(index);
            isselectbutton = true;
            er_ScrollChanged(null, null);


        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var rows = GetDataGridRows(er);
            var index = er.Columns.Single(c => c.Header.ToString() == (sender as CheckBox).Content.ToString()).DisplayIndex;
            indexcolumn = index;
            isselectbutton = false;
            ColorCol.Remove(index);
            ColorNull.Add(index);

            int i = 0;
            er_ScrollChanged(null,null);
       
        }

   
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Data.Dispose();
            //Data = null;
        }
    }
    public static class DataGridHelper
    {
        public static DataGridColumnHeader GetColumnHeader(DataGrid dataGrid, int index)
        {
            DataGridColumnHeadersPresenter presenter = FindVisualChild<DataGridColumnHeadersPresenter>(dataGrid);

            if (presenter != null)
            {
                return (DataGridColumnHeader)presenter.ItemContainerGenerator.ContainerFromIndex(index);
            }

            return null;
        }
        public static DataGridRow GetRow(DataGridCell dataGridCell)
        {
            int rowIndex = GetRowIndex(dataGridCell);
            DataGrid dataGrid = GetDataGridFromChild(dataGridCell);

            return GetRow(dataGrid, rowIndex);
        }
        public static int GetRowIndex(DataGridCell dataGridCell)
        {
            // Use reflection to get DataGridCell.RowDataItem property value.
            PropertyInfo rowDataItemProperty = dataGridCell.GetType().GetProperty("RowDataItem", BindingFlags.Instance | BindingFlags.NonPublic);

            DataGrid dataGrid = GetDataGridFromChild(dataGridCell);

            // Use DataGrid.Items.IndexOf(DataGridCell.RowDataItem) to get the cell's row index.
            return dataGrid.Items.IndexOf(rowDataItemProperty.GetValue(dataGridCell, null));
        }
        public static DataGrid GetDataGridFromChild(DependencyObject dataGridPart)
        {
            if (System.Windows.Media.VisualTreeHelper.GetParent(dataGridPart) == null)
            {
                return null;
            }
            if (System.Windows.Media.VisualTreeHelper.GetParent(dataGridPart) is DataGrid)
            {
                return (DataGrid)System.Windows.Media.VisualTreeHelper.GetParent(dataGridPart);
            }
            else
            {
                return GetDataGridFromChild(System.Windows.Media.VisualTreeHelper.GetParent(dataGridPart));
            }
        }
        public static DataGridRow GetRow(DataGrid dataGrid, int index)
        {
            DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(index);
            row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(index);
            return row;
        }
        public static DataGridCell GetCell(DataGridRow rowContainer, int column)
        {
            DataGridCellsPresenter presenter = GetCellsPresenter(rowContainer);
            if (presenter != null)
            {
                return presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
            }
            return null;
        }
        public static DataGridCellsPresenter GetCellsPresenter(Visual parent)
        {
            return FindVisualChild<DataGridCellsPresenter>(parent);
        }
        public static DataGridCell GetCell(DataGrid dataGrid, int row, int column)
        {
            DataGridRow rowContainer = GetRow(dataGrid, row);
            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                return cell;
            }
            return null;
        }
        public static childItem FindVisualChild<childItem>(DependencyObject obj)
        where childItem : DependencyObject
        {
            for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = System.Windows.Media.VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
    }
    public static class VisualTreeHelper
    {
        private static void GetVisualChildren<T>(DependencyObject current, Collection<T> children) where T : DependencyObject
        {
            if (current != null)
            {
                if (current.GetType() == typeof(T))
                {
                    children.Add((T)current);
                }

                for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(current); i++)
                {
                    GetVisualChildren<T>(System.Windows.Media.VisualTreeHelper.GetChild(current, i), children);
                }
            }
        }
        public static Collection<T> GetVisualChildren<T>(DependencyObject current) where T : DependencyObject
        {
            if (current == null)
            {
                return null;
            }

            Collection<T> children = new Collection<T>();

            GetVisualChildren<T>(current, children);

            return children;
        }
        public static T GetVisualChild<T, P>(P templatedParent)
            where T : FrameworkElement
            where P : FrameworkElement
        {
            Collection<T> children = VisualTreeHelper.GetVisualChildren<T>(templatedParent);

            foreach (T child in children)
            {
                if (child.TemplatedParent == templatedParent)
                {
                    return child;
                }
            }
            return null;
        }
    }
}
