using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using TestService.Models;

namespace TestService.Data
{
    public class DBPerson
    {
        /// <summary>
        /// Добавление сотрудника в справочник
        /// </summary>
        /// <param name="name">ФИО сотрудника</param>
        /// <param name="PositionUid">Идентификатор должности</param>
        /// <returns></returns>
        public static Result Add(string name, string PositionUid = "")
        {
            Result result = new Result();
            Person person = null;

            try
            {
                using (SqlConnection con = new SqlConnection(Program.Cnf["Database"]))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[PR_PersonAdd]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter p_name = new SqlParameter
                        {
                            ParameterName = "@p_name",
                            SqlDbType = SqlDbType.VarChar,
                            Direction = ParameterDirection.Input,
                            Value = name
                        };
                        SqlParameter p_pos = new SqlParameter
                        {
                            ParameterName = "@p_pos",
                            SqlDbType = SqlDbType.VarChar,
                            Direction = ParameterDirection.Input,
                            Value = PositionUid
                        };
                        cmd.Parameters.Add(p_name);
                        cmd.Parameters.Add(p_pos);

                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                result.Code = reader.GetInt32(reader.GetOrdinal("Code"));
                                result.Info = reader["Info"].ToString();
                            }

                            if (result.Code == 0)
                            {
                                reader.NextResult();
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        person = new Person
                                        {
                                            Uid = reader["Uid"].ToString(),
                                            Name = reader["Name"].ToString()
                                        };
                                    }
                                    reader.NextResult();
                                    if (reader.HasRows)
                                    {
                                        while (reader.Read())
                                        {
                                            person.Position = new Position
                                            {
                                                Uid = reader["Uid"].ToString(),
                                                Name = reader["Name"].ToString()
                                            };
                                        }
                                    }

                                    result.Data = person;
                                }

                            }
                        }
                        else
                        {
                            reader.Close();
                            con.Close();

                            result.Code = 102;
                            result.Info = "Нет данных";
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.Logger.Error(ex.Message);
                result.Code = 103;
                result.Info = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// Получение справочника сотрудников
        /// </summary>
        /// <returns></returns>
        public static Result GetAll()
        {
            Result result = new Result();

            List<Person> persons = new List<Person>();

            try
            {
                using (SqlConnection con = new SqlConnection(Program.Cnf["Database"]))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[PR_Persons]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                result.Code = reader.GetInt32(reader.GetOrdinal("Code"));
                                result.Info = reader["Info"].ToString();
                            }

                            if (result.Code == 0)
                            {
                                reader.NextResult();
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        Person person = new Person
                                        {
                                            Uid = reader["Uid"].ToString(),
                                            Name = reader["Name"].ToString(),
                                            Position = new Position
                                            {
                                                Uid = reader["UidPosition"].ToString()
                                            }
                                        };
                                        persons.Add(person);
                                    }
                                    result.Data = persons;
                                }

                            }
                        }
                        else
                        {
                            reader.Close();
                            con.Close();

                            result.Code = 102;
                            result.Info = "Нет данных";
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.Logger.Error(ex.Message);
                result.Code = 103;
                result.Info = ex.Message;
            }
            return result;
        }

    }
}
