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
            //Example01();
            //Example02();
            Example03();

            Console.ReadKey();
        }

        // пример 1
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

        // пример 2 - создание таблицы и внесение данных
        static void Example02()
        {
            DataSet ds = new DataSet();

            // создаем таблицу

            DataTable dataTable = new DataTable("TrackEvolutionPart");
            dataTable.Columns.Add(new DataColumn("intEvolutionPartId", typeof(int)) { AllowDBNull = false, Unique = true });
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["intEvolutionPartId"] };
            dataTable.Columns.Add(new DataColumn("intEvolutionId", typeof(int)));
            dataTable.Columns.Add(new DataColumn("intMasterId", typeof(int)));
            dataTable.Columns.Add(new DataColumn("intEvolutionId1", typeof(int)));
            dataTable.Columns.Add(new DataColumn("stringDescription", typeof(string)) { MaxLength = 50 });

            // вносим данные

            // создаем строку
            DataRow row = dataTable.NewRow();

            row["intEvolutionPartId"] = 1;
            row["intEvolutionId"] = 1;
            row["intMasterId"] = 1;
            row["intEvolutionId"] = 1;
            row["stringDescription"] = "name";

            dataTable.Rows.Add(row);

            foreach (DataRow data in dataTable.Rows)
            {
                Console.WriteLine($"Состояние: {data.RowState}");
                Console.WriteLine();
                dataTable.AcceptChanges();
                Console.WriteLine($"Состояние: {data.RowState}");
            }

        }


        //===========================

        // пример 3
        static void Example03()
        {
            connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(connStr);

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "select top 5 * from newEquipment";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);

            DataSet dataSet = new DataSet();

            adapter.Fill(dataSet);  // сам открывает и сам закрывает соединение

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                //Console.WriteLine($"\t{row}");
                DisplayRow(row);
            }

            // удаляем строки

            DataRow row0 = dataSet.Tables[0].Rows[0];
            row0[1] = "999";
            row0[6] = "1250125";

            DataRow row4 = dataSet.Tables[0].Rows[4];
            dataSet.Tables[0].Rows.Remove(row4);

            Console.WriteLine(new string('-', 20));
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                DisplayRow(row);
            }
            Console.WriteLine(new string('-', 20));


            // выводим данные которые были изменены в DataSet
            Console.WriteLine("Сделаны изменения");
            var changes = dataSet.GetChanges();

            foreach (DataRow row in changes.Tables[0].Rows)
            {
                DisplayRow(row);
            }


        }

        static void DisplayRow(DataRow row)
        {
            Console.WriteLine("Гаражный номер: {0}, SN: {1}",                            row["intGarageRoom"], row["strSerialNo"]);

            var originalVersion = row[1, DataRowVersion.Original];
            var originalCurrent = row[1, DataRowVersion.Current];

        }
    }
}


// Default View - удобное представление в GridView WPF