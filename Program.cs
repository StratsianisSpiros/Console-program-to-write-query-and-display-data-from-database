using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace DataConnectionConsole
{
    class Program
    {
        public static readonly string conn = @"Data Source = mySource; Initial Catalog = myDatabase; Integrated Security = True";

        static void Main(string[] args)
        {   //It is a programm to write a custom query and bring data from database (if you know the tables) using datatables. No modifying of database allowed
            while (true)
            {
                //Array to check for words that modify database (hope i have them all)
                string[] words = new string[] { "delete", "update", "insert", "alter", "drop", "truncate", "create" };

                Console.Clear();
                Console.WriteLine("Enter an SQL query, or exit");
                string query = Console.ReadLine();
                bool check = !words.Any(query.Contains);

                if (query.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    Environment.Exit(0);

                if (check)
                {
                    DataTable dataTable = GetDataTable(query, conn);
                    DislpayDataTable(dataTable);
                }
                else
                    Console.WriteLine("You are not allowed to modify entries in database");

                Console.ReadKey(true);
            }
        }

        protected static DataTable GetDataTable(string cmd, string conn)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(conn))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(cmd, connection))
                    {

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return dataTable;
        }

        public static void DislpayDataTable(DataTable dataTable)
        {
            //Loop that displays column names
            foreach (DataColumn column in dataTable.Columns)
            {
                Console.Write(column.ColumnName + " -- ");
            }

            Console.Write(Environment.NewLine);

            DateTime date;
            int count = 1;

            //Loop  tha displays datable rows values
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Console.Write($"{count}. ");
                for (int i = 0; i < dataRow.ItemArray.Length; i++)
                {
                    if (DateTime.TryParse(dataRow[i].ToString(), out date))
                    {
                        Console.Write(date.ToString("dd-MM-yyyy") + " || ");
                    }
                    else
                    {
                        Console.Write(dataRow[i].ToString() + " || ");
                    }
                }
                count++;
                Console.WriteLine();
            }
        }
    }
}
