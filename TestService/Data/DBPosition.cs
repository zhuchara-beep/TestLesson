using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using TestService.Models;

namespace TestService.Data
{
    public static class DBPosition
    {
        /// <summary>
        /// Добавление должности в справочник
        /// </summary>
        /// <param name="name">Наименование должности</param>
        /// <returns></returns>
        public static Result Add(string name)
        {
            Result result = new Result();

            Position position = null;

            try
            {
                using (SqlConnection con = new SqlConnection(Program.Cnf["Database"]))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[PR_PositionAdd]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter p_name = new SqlParameter
                        {
                            ParameterName = "@p_name",
                            SqlDbType = SqlDbType.VarChar,
                            Direction = ParameterDirection.Input,
                            Value = name
                        };
                        cmd.Parameters.Add(p_name);

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
                                        position = new Position
                                        {
                                            Uid = reader["Uid"].ToString(),
                                            Name = reader["Name"].ToString()
                                        };
                                    }
                                    result.Data = position;
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
        /// Получение справочника должностей
        /// </summary>
        /// <returns></returns>
        public static Result GetAll()
        {
            Result result = new Result();

            List<Position> positions = new List<Position>();

            try
            {
                using (SqlConnection con = new SqlConnection(Program.Cnf["Database"]))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[PR_Positions]", con))
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
                                        Position position = new Position
                                        {
                                            Uid = reader["Uid"].ToString(),
                                            Name = reader["Name"].ToString()
                                        };
                                        positions.Add(position);
                                    }
                                    result.Data = positions;
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
