using MySql.Data.MySqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace SqlLibrary
{
    public class FormsHelper
    {

        //пример строки подключения в методе GetConnectionExample
        internal MySqlConnection Connection;


        //конструктор без аргументов, при использовании данного конструктора необходимо задать строку подключения методом SetConnection
        public FormsHelper() { }


        //конструктор с параметрами datasource, username, database, password
        public FormsHelper(string datasource, string username, string database, string password)
        {
            Connection = new
  ($"Datasource  =  {datasource}   ;    port   =   3306 ;  username  =  {username}; password ={password};database  =  {database}; ");
        }


        //конструктор с параметрами username, database, password
        public FormsHelper(string username, string database, string password)
        {
            Connection = new
  ($"Datasource  =  localhost   ;    port   =   3306 ;  username  =  {username}; password ={password};database  =  {database}; ");
        }


        //конструктор с параметрами database, password
        public FormsHelper(string database, string password)
        {
            Connection = new
  ($"Datasource  =  localhost   ;    port   =   3306 ;  username  =  root; password ={password};database  =  {database}; ");
        }

        //конструктор с параметров database
        public FormsHelper(string database)
        {
            Connection = new
  ($"Datasource  =  localhost   ;    port   =   3306 ;  username  =  root; password =;database  =  {database}; ");
        }

        //задает строку подключения для экземпляра класса
        public void SetConnection(string connection)
        {
            this.Connection = new(connection);
        }


        //выдает значение текущей строки подключения
        public string GetConnection() => Connection.ConnectionString;


        //выдает пример строки подключения
        public static string GetConnectionExample() => $"Datasource  =  @Datasource   ;    port   =   @port ;  username  =  @username; password = @password;database  =  @database;";

        //заполняет данными элемент DataGridView согласно запросу
        public void DataGridFiller(string queryString, DataGridView dataGridView)
        {
            dataGridView.DataSource = DataTableFiller(queryString);
        }


        //заполняет данными элемент DataTable согласно запросу
        public DataTable DataTableFiller(string queryString)
        {
            try
            {
                Connection.Close();
                Connection.Open();
                var command = new MySqlCommand(queryString, Connection);
                command.ExecuteNonQuery();
                var dataTable = new DataTable();
                new MySqlDataAdapter(command).Fill(dataTable);
                Connection.Close();
                return dataTable;

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            return new DataTable();
        }


        //возвращает результат 1 строки 1 столбца запроса
        public string QueryResult(string queryString)
        {
            try
            {
                Connection.Close();
                Connection.Open();
                var command = new MySqlCommand(queryString, Connection);
                string? queryResult = command.ExecuteScalar().ToString();
                Connection.Close();
                if (queryResult != null)
                {
                    return queryResult;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);

            }
            return "error, result is null";

        }


        //выполняет команду, (необязательно к использованию) при успешном выполнение  отправляет код 0, при неуспешном - код -1
        public int CommandExecutor(string queryString)
        {
            try
            {
                Connection.Close();
                Connection.Open();
                var command = new MySqlCommand(queryString, Connection);
                command.ExecuteNonQuery();
                Connection.Close();
                return 0;

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return -1;
            }
        }


        //заполняет элемент ComboBox согласно запросу и указанию столбца-источника
        public async void ComboBoxFiller(ComboBox comboBox, string queryString, string ColumnName)
        {
            {
                Connection.Close();
                Connection.Open();
                MySqlCommand cmd = new(queryString, Connection);
                MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    comboBox.Items.Add(reader[ColumnName].ToString());
                }
                Connection.Close();
            }
        }


        //генерирует хэш
        public string GenerateMD5Hash(string input) => Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input)));



        //проверяет пароль
        public bool PasswordValidator(string password)
        {
            if (password.Length < 6)
            {
                return false;
            }
            var hasUpper = false;
            var hasLower = false;
            var hasDigit = false;
            var hasSpecial = false;

            foreach (char c in password)
            {
                if (char.IsDigit(c))
                {
                    hasDigit = true;
                }
                else if (char.IsUpper(c))
                {
                    hasUpper = true;
                }
                else if (char.IsLower(c))
                {
                    hasLower = true;
                }
                else if (!char.IsLetterOrDigit(c))
                {
                    hasSpecial = true;
                }

                if (hasDigit && hasUpper && hasLower && hasSpecial)
                {
                    return true;
                }
            }
            return false;
        }
    }
}