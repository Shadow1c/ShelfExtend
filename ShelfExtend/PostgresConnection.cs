using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ShelfExtend
{
    public static class PostgresConnection
    {
        static string stringConnection = "User ID = kbdqewln; Password = 0sz7xXfaP49UwO41tkddPpniCQr2b7DL; Host = surus.db.elephantsql.com; Port = 5432; Database = kbdqewln;";
        public static IDbConnection Connection = new NpgsqlConnection(stringConnection);

        public static NpgsqlConnection EstablishConnection()
        {
            return new NpgsqlConnection(stringConnection);
        }

        public static List<T> GetListFromDB<T>(string SQLCommand)
        {
            var list = new List<T>();
            using (var loginConnection = PostgresConnection.EstablishConnection())
            {
                try
                {
                    list = loginConnection.Query<T>($"{SQLCommand}").ToList();//Stored Procedure
                }
                catch
                {
                    return list = null;
                }
            }
            return list;
        }
        public static bool isExecutSqlString(string SqlString)
        {
            using (var RegistrationConnection = PostgresConnection.EstablishConnection())
            {
                try
                {
                    RegistrationConnection.Execute($"{SqlString}");
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Nie udało się wykonac zawołnia. Naciśnij dowolny klawisz by kontynuować.");
                    Console.ReadKey();
                    return false;
                }
                Console.Clear();
                Console.WriteLine("Zapis powiodł się. Naciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                return true;
            }
        }
        public static int? GetCount(string SQLCommand)
        {
            using (var Connection = PostgresConnection.EstablishConnection())
            {
                try
                {
                    int? DBOutput = Connection.QueryFirst<int>(SQLCommand);
                    return DBOutput;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
