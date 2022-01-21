using System;
using MySql.Data.MySqlClient;
using System.Text;
using System.Runtime.InteropServices.ComTypes;

//참고 사이트
//https://www.csharpstudy.com/Practical/Prac-mysql.aspx
namespace MySqlUsing
{
    class Program
    {
        static void Main(string[] args)
        {
            int selection = 0;
            while (selection != 4)
            {
                Console.WriteLine("1. 테이블 목록 보기");
                Console.WriteLine("2. 테이블 데이터 확인");
                Console.WriteLine("3. 테이블 설명");
                Console.WriteLine("4. 종료");
                string strSel =  Console.ReadLine();
                try
                {
                    selection = Convert.ToInt32(strSel);
                }
                catch(Exception e)
                {
                    Console.WriteLine("올바른 값을 입력하여 주십시요.");
                }
                switch(selection)
                {
                    case 1:
                        ReadSqlCommand("show tables");
                        PauseCommand();
                        break;
                    case 2:
                        string command = "SELECT * FROM ";
                        Console.Write("테이블 명 : ");
                        string table = Console.ReadLine();
                        if(CheckTableName(table))
                        {
                            command += table;
                            Console.Write("조건 (없으면 0, and/or 로 구분) : ");
                            string where = Console.ReadLine();
                            if(where == "0") ReadSqlCommand(command);
                            command += " WHERE " + where;
                            ReadSqlCommand(command);
                        }
                        else
                        {
                            Console.WriteLine("테이블이 없습니다.");
                        }
                        PauseCommand();
                        break;
                    case 3:
                        command = "desc ";
                        Console.Write("테이블 명 : ");
                        table = Console.ReadLine();
                        if (CheckTableName(table))
                        {
                            command += table;
                            ReadSqlCommand(command);
                        }
                        else
                        {
                            Console.WriteLine("테이블이 없습니다.");
                        }
                        PauseCommand();
                        break;
                    case 4:
                        break;

                    default:
                        PauseCommand();
                        break;
                }

                Console.Clear();
            }
        }
        // 반환 값이 없는 커맨드
        static void NotReadSqlCommand(string sql)
        {
            string strConnect = "Server=localhost;Port=3306;Database=world;Uid=root;Pwd=root";
            using(MySqlConnection connection = new MySqlConnection(strConnect))
            {
                try // 연결 확인
                {
                    connection.Open();
                    //string sql = "show databases";
                    MySqlCommand mySqlCommand = new MySqlCommand(sql, connection);
                    if (mySqlCommand.ExecuteNonQuery() == 1)
                        // ExecuteNonQuery() 는 커맨드가 완수되면 1을 반환한다.
                        Console.WriteLine("실행 완료");
                    connection.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
        }
        // 반환 값이 있는 커맨드
        static void ReadSqlCommand(string sql) // 연결 모드
        {
            string strConnect = "Server=localhost;Port=3306;Database=world;Uid=root;Pwd=root";
            using (MySqlConnection connection = new MySqlConnection(strConnect))
            {
                try // 연결 확인
                {
                    connection.Open();
                    //string sql = "show databases";
                    MySqlCommand mySqlCommand = new MySqlCommand(sql, connection);
                    MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                    // 반환 값이 있는 커맨드는 MysqlDataReader 클래스로 받아옴
                    // ExecuteReader() 은 커맨드가 반환 값이 있으면 받아옴
                    while (mySqlDataReader.Read())
                    {
                        string str = string.Empty;
                        for (int i = 0; i < mySqlDataReader.FieldCount; i++)
                        {
                            if (i != mySqlDataReader.FieldCount - 1) str += mySqlDataReader[i] + " ; ";
                            else if (i == mySqlDataReader.FieldCount - 1) str += mySqlDataReader[i];
                        }
                        Console.WriteLine("{0}", str);
                    }
                    connection.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
        }
        // 테이블 명 확인
        static bool CheckTableName(string table) // 연결 모드
        {
            string strConnect = "Server=localhost;Port=3306;Database=world;Uid=root;Pwd=root";
            bool ok = false;
            using (MySqlConnection connection = new MySqlConnection(strConnect))
            {
                try // 연결 확인
                {
                    connection.Open();
                    string sql = "show tables";
                    MySqlCommand mySqlCommand = new MySqlCommand(sql, connection);
                    MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                    while (mySqlDataReader.Read())
                    {
                        if (table == (string)mySqlDataReader.GetValue(0))
                            ok = true;
                    }
                    connection.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            return ok;
        }
        static void PauseCommand()
        {
            Console.Write("계속 하시려면 아무 키나 입력해주세요...");
            Console.ReadKey();
        }
    }
}
