using Efiling.DataAccess.Interface;
using System.Configuration;
using Efiling.Common;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

namespace Efiling.DataAccess
{
    public class EfileDAL : IEfileDataAccess
    {
        private string _connStr;

        public EfileDAL()
        {
            _connStr = ConfigurationManager.ConnectionStrings["EfileDBConnStr"].ConnectionString;
        }

        public long SaveEfile(Efile efile)
        {
            //insert into db
            SqlConnection sqlconn = new SqlConnection(_connStr);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = sqlconn;
            cmd.CommandText = "dbo.sp_insertEfile";
            cmd.CommandType = CommandType.StoredProcedure;

            // 创建参数 
            IDataParameter[] parameters = {
                 new SqlParameter("@id", SqlDbType.Int,4) ,
                 new SqlParameter("@name", SqlDbType.NVarChar,20) ,
                 new SqlParameter("@transmissionId", SqlDbType.NVarChar, 20),
                 new SqlParameter("@efileStatus", SqlDbType.Int, 4),
                 new SqlParameter("@content", SqlDbType.NVarChar, 500)
             };

            // 设置参数类型 
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = efile.EfileName;
            parameters[2].Value = efile.TransmissionId.ToString();
            parameters[3].Value = EfileStatus.Created;
            parameters[4].Value = efile.Content;

            // 添加参数 
            cmd.Parameters.Add(parameters[0]);
            cmd.Parameters.Add(parameters[1]);
            cmd.Parameters.Add(parameters[2]);
            cmd.Parameters.Add(parameters[3]);
            cmd.Parameters.Add(parameters[4]);

            sqlconn.Open();

            // 执行存储过程并返回影响的行数 
            cmd.ExecuteNonQuery();

            efile.Id = (int)cmd.Parameters["@id"].Value;

            sqlconn.Close();

            return efile.Id;
        }

        public IEnumerable<Efile> GetEfileHistory(long id)
        {
            Efile efile = new Efile();
            yield return efile ;
        }

        public EfileStatus UpdateEfileHistory(Efile efile)
        {
            return EfileStatus.Created;
        }
    }
}
