using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdoNetLesson3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            #region DataViewer

            //using (SqlConnection conn=new SqlConnection())
            //{
            //    conn.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
            //    DataSet set = new DataSet();
            //    var da = new SqlDataAdapter("select Id,Name,Pages,YearPress from Books",conn);
            //    da.Fill(set, "Books");
            //    DataViewManager dvm = new DataViewManager(set);
            //    dvm.DataViewSettings["Books"].RowFilter = "Pages>300";
            //    dvm.DataViewSettings["Books"].Sort = "Pages ASC";
            //    DataView dv = dvm.CreateDataView(set.Tables["Books"]);
            //    mygrid.ItemsSource = dv;
            //}
            #endregion


            #region Transactions

            SqlTransaction sqlTransaction=null;
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
                conn.Open();
                sqlTransaction = conn.BeginTransaction();
                
                SqlCommand comm = new SqlCommand("insert into Press(Id,Name) values(@Id,@Name)", conn);
                comm.Transaction = sqlTransaction;
                SqlParameter param1 = new SqlParameter();
                param1.Value = 1126;
                param1.ParameterName = "@Id";
                param1.SqlDbType = SqlDbType.Int;

                SqlParameter param2 = new SqlParameter();
                param2.Value = "John";
                param2.ParameterName = "@Name";
                param2.SqlDbType = SqlDbType.NVarChar;

                comm.Parameters.Add(param1);
                comm.Parameters.Add(param2);
                SqlCommand sqlCommand = new SqlCommand("sp_UpdateBook", conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Transaction = sqlTransaction;
                SqlParameter p1 = new SqlParameter();
                p1.Value = 1;
                p1.ParameterName = "@bookId";
                p1.SqlDbType = SqlDbType.Int;

                SqlParameter p2 = new SqlParameter();
                p2.Value = 100;
                p2.ParameterName = "@page";
                p2.SqlDbType = SqlDbType.Int;
                sqlCommand.Parameters.Add(p1);
                sqlCommand.Parameters.Add(p2);

                try
                {
                     sqlCommand.ExecuteNonQuery();
                     comm.ExecuteNonQuery();
                     sqlTransaction.Commit();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                   sqlTransaction.Rollback();
                }
                finally
                {
                    MessageBox.Show("oKAY");
                    conn.Close();
                }

            }
            #endregion
        }
    }
}
