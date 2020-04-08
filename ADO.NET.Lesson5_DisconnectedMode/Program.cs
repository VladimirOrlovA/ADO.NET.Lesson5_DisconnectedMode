using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data;
using System.Configuration;

/// <summary>
/// Отсоединенный режим - Disconnected MOde
/// </summary>
namespace ADO.NET.Lesson5_DisconnectedMode
{
    class Program
    {
        static string connStr = "";
        static void Main(string[] args)
        {
            connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            Example01();

            Console.ReadKey();
        }

        static void Example01()
        {
            SqlConnection conn = new SqlConnection(connStr);

            SqlDataAdapter adapter = new SqlDataAdapter("select top 5 * from newEquipment", conn);

            DataSet ds = new DataSet();

            ds.DataSetName = "Table Equipment";
            ds.Locale = new System.Globalization.CultureInfo("ru-RU");

            adapter.Fill(ds);

            foreach (DataTable dt in ds.Tables)
            {
                Console.WriteLine(dt.TableName);

                foreach (DataColumn column in dt.Columns)
                {
                    Console.WriteLine("\t{0,30} - \t DataType: {1}",
                        column.ColumnName,
                        column.DataType.ToString());
                }

                Console.WriteLine("\n\n" + new string('=', 100) + "\n\n");

                foreach (DataColumn column in dt.Columns)
                {
                    Console.Write("\t{0,30}",
                        column.ColumnName);
                }

                foreach (DataRow row in dt.Rows)
                {
                    var cells = row.ItemArray;

                    foreach (object cell in cells)
                    {
                        Console.Write("\t{0,15}", cell);
                    }
                    Console.WriteLine();
                }
            }

            // принять изменения
            ds.AcceptChanges();

            // отклонить изменения
            ds.RejectChanges();

            // получить измененные изменения
            ds.GetChanges();

            // слияние данных - копирование
            DataSet dsCopy = new DataSet();
            dsCopy.Merge(ds);

            // Восстановление оригинального состояния объекта 
            ds.Reset();

            // ================================================================== //

            // СОБЫТИЯ
        }

    }
}
