using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using FallBackTool.Producer.Models;
using FallBackTool.Converter.Models;
using log4net;

namespace FallBackTool.Producer
{
    public class DBProducer
    {
        private DBProducerInput dbInput;
        private SqlConnection conn;
        private long lastSyncVersion;

        public DataTable results;

        public DBProducer()
        {
            dbInput = new DBProducerInput();
            dbInput.ValidInputs();

            ValidConnection();
        }
        public DBProducer(DBProducerInput setting)
        {
            dbInput = setting;
            dbInput.ValidInputs();

            ValidConnection();
        }
        ~DBProducer() {
            //conn.Close();
            //conn.Dispose();
        }

        #region private functions
        private SqlConnection ValidConnection()
        {
            if (conn == null || conn.State == ConnectionState.Closed)
            {
                Console.WriteLine("[DB Producer]:Start initialize DB connection...");
                conn = new SqlConnection(dbInput.ConnectionString);

                conn.Open();
                conn.ChangeDatabase(dbInput.DBName);
                Console.WriteLine("[DB Producer]:DB Connection initialized");
            }

            return conn;
        }

        private bool IsChangeTrackingEnabledOnDatabase()
        {
            ValidConnection();

            using (var command = conn.CreateCommand())
            {
                command.CommandText = "SELECT count(1) FROM SYS.CHANGE_TRACKING_DATABASES WHERE DATABASE_ID = DB_ID(@databaseName)";
                command.Parameters.AddWithValue("databaseName", dbInput.DBName);
                int count;
                return Int32.TryParse(command.ExecuteScalar().ToString(), out count) && count == 1;
            }
        }

        private bool IsChangeTrackingEnabledOnTable()
        {
            ValidConnection();

            using (var command = conn.CreateCommand())
            {
                command.CommandText = "SELECT count(1) FROM SYS.CHANGE_TRACKING_TABLES WHERE OBJECT_ID = OBJECT_ID(@tableName)";
                command.Parameters.AddWithValue("tableName", dbInput.TableName);
                int count;
                return Int32.TryParse(command.ExecuteScalar().ToString(), out count) && count == 1;
            }
        }

        private bool IsChangedOrExist()
        {
            if (!string.IsNullOrEmpty(dbInput.ValueList) && !string.IsNullOrEmpty(dbInput.FilterKey))
                return GetDataByFilter();

            if (!string.IsNullOrEmpty(dbInput.LastSyncDate))
                return GetCurrentVersion() > GetLastSyncVersionbyDate() ? true : false;
            else if (!string.IsNullOrEmpty(dbInput.LastSyncId))
                return GetCurrentVersion() > GetLastSyncVersionbyId() ? true : false;
            else
                return GetCurrentVersion() > GetLastSyncVersion() ? true : false;
        }

        private bool GetDataByFilter()
        {
            ValidConnection();

            using (var command = conn.CreateCommand())
            {
                command.CommandText = "select * from " + dbInput.TableName + " where " + dbInput.FilterKey + " in (" + dbInput.ValueList + ")";
                using (var adapter = new SqlDataAdapter(command))
                {
                    results = new DataTable();
                    adapter.Fill(results);

                    if (results.Rows.Count>0)
                        return true;
                    else
                        return false;
                }
            }
        }

        private long GetCurrentVersion()
        {
            ValidConnection();

            using (var command = conn.CreateCommand())
            {
                command.CommandText = "select CHANGE_TRACKING_CURRENT_VERSION()";

                return long.Parse(command.ExecuteScalar().ToString());
            }
        }

        private long GetLastSyncVersionbyDate()
        {
            ValidConnection();

            using (var command = conn.CreateCommand())
            {
                command.CommandText = "select count(1) from " + dbInput.TableName + " where lastmodified<'" + dbInput.LastSyncDate + "'";
                if (int.Parse(command.ExecuteScalar().ToString()) > 0)
                {
                    command.CommandText = "select top 1 " + dbInput.PrimaryKey + " from " + dbInput.TableName + " where lastmodified<'" + dbInput.LastSyncDate + "' order by lastmodified desc";
                    int recordId;
                    if (int.TryParse(command.ExecuteScalar().ToString(), out recordId))
                    {
                        command.CommandText = "SELECT C.SYS_CHANGE_VERSION FROM CHANGETABLE(VERSION " + dbInput.TableName + ", (" + dbInput.PrimaryKey + "), (@recordId)) as C";
                        //command.Parameters.AddWithValue("tableName", dbInput.TableName);
                        //command.Parameters.AddWithValue("primaryKey", dbInput.PrimaryKey);
                        //command.Parameters.AddWithValue("lastSyncDate", dbInput.LastSyncDate);
                        command.Parameters.AddWithValue("recordId", recordId);

                        lastSyncVersion = long.Parse(command.ExecuteScalar().ToString());
                        return lastSyncVersion;
                    }
                }

                return long.MaxValue;
            }
        }

        private long GetLastSyncVersionbyId()
        {
            ValidConnection();

            using (var command = conn.CreateCommand())
            {
                command.CommandText = "select count(1) from " + dbInput.TableName + " where " + dbInput.PrimaryKey + "='" + dbInput.LastSyncId + "'";
                if (int.Parse(command.ExecuteScalar().ToString()) > 0)
                {
                    command.CommandText = "SELECT C.SYS_CHANGE_VERSION FROM CHANGETABLE(VERSION " + dbInput.TableName + ", (" + dbInput.PrimaryKey + "), (@recordId)) as C";
                    //command.Parameters.AddWithValue("tableName", dbInput.TableName);
                    //command.Parameters.AddWithValue("primaryKey", dbInput.PrimaryKey);
                    command.Parameters.AddWithValue("recordId", dbInput.LastSyncId);

                    lastSyncVersion = long.Parse(command.ExecuteScalar().ToString());
                    return lastSyncVersion;
                }

                return long.MaxValue;
            }
        }

        private long GetLastSyncVersion()
        {
            lastSyncVersion = long.Parse(dbInput.LastSyncVersion);
            return lastSyncVersion;
        }

        private DataTable GetChangeResults()
        {
            if (results != null)
            {
                Console.WriteLine(string.Format("[DB Producer]:Loading complete. {0} records be loaded", results.Rows.Count.ToString()));
                return results;
            }

            ValidConnection();

            using (var command = conn.CreateCommand())
            {
                command.CommandText = @"SELECT tb.*, c.SYS_CHANGE_VERSION, c.SYS_CHANGE_OPERATION
                                            FROM CHANGETABLE (CHANGES " + dbInput.TableName + @", @last_sync_version) AS c
                                                LEFT JOIN " + dbInput.TableName + @" AS tb
                                                    ON tb." + dbInput.PrimaryKey + " = c." + dbInput.PrimaryKey
                                            + " where c.SYS_CHANGE_OPERATION<>'D'";
                //command.Parameters.AddWithValue("tableName", dbInput.TableName);
                //command.Parameters.AddWithValue("primaryKey", dbInput.PrimaryKey);
                command.Parameters.AddWithValue("last_sync_version", lastSyncVersion);

                using (var adapter = new SqlDataAdapter(command))
                {
                    DataTable changes = new DataTable();
                    adapter.Fill(changes);

                    Console.WriteLine(string.Format("[DB Producer]:Loading complete. {0} records be loaded", changes.Rows.Count.ToString()));
                    ILog log = LogManager.GetLogger(typeof(DBProducer));
                    log.Info(string.Format("[DB Producer]:Loading complete. {0} records be loaded", changes.Rows.Count.ToString()));

                    results = changes;
                    return changes;
                }
            }
        }

        private List<IDBModel> ConvertDataTable(DataTable dt)
        {
            List<IDBModel> list = new List<IDBModel>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    IDBModel obj;
                    switch (dbInput.TableName)
                    {
                        case "GL04Account":
                            obj = new AccountDBModel();
                            break;
                        case "GL04InternalOrder":
                            obj = new IODBModel();
                            break;
                        default:
                            obj = new ContactDBModel();
                            break;
                    }

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        //PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                        //propertyInfo.SetValue(obj, Convert.ChangeType(dr[prop.Name], propertyInfo.PropertyType), null);
                        if (prop.Name == "ActionCode")
                        {
                            if (dt.Columns.Contains("SYS_CHANGE_OPERATION"))
                                prop.SetValue(obj, (dr["SYS_CHANGE_OPERATION"].ToString() == "U" ? "1" : "0"));
                            else
                                prop.SetValue(obj, "0");
                        }
                        else if (prop.Name == "StateCode")
                        {
                            StateCode state = new StateCode
                            {
                                Value = dr[prop.Name].ToString()
                            };
                            prop.SetValue(obj, state);
                        }
                        else if (prop.Name == "StatusCode")
                        {
                            StatusCode status = new StatusCode
                            {
                                Value = dr[prop.Name].ToString()
                            };
                            prop.SetValue(obj, status);
                        }
                        else
                            prop.SetValue(obj, dr[prop.Name] == DBNull.Value ? null : dr[prop.Name]);
                    }
                    list.Add(obj);
                }

                if (dt.Columns.Contains("SYS_CHANGE_VERSION"))
                    dt.Columns.Remove("SYS_CHANGE_VERSION");
                if (dt.Columns.Contains("SYS_CHANGE_OPERATION"))
                    dt.Columns.Remove("SYS_CHANGE_OPERATION");
                dt.AcceptChanges();

                results = dt;
            }

            return list;
        }
        #endregion

        public List<IDBModel> Produce()
        {
            try
            {
                if (IsChangeTrackingEnabledOnDatabase() && IsChangeTrackingEnabledOnTable())
                {
                    Console.WriteLine("[DB Producer]:Change Tracking is enabled on Database and Table");
                    if (IsChangedOrExist())
                    {
                        Console.WriteLine("[DB Producer]:Changes or data be found by specific Date/Version/Filter");
                        Console.WriteLine("[DB Producer]:Loading data...");
                        ILog log = LogManager.GetLogger(typeof(DBProducer));
                        log.Info("[DB Producer]:Loading data...");

                        return ConvertDataTable(GetChangeResults());
                    }
                    else
                        Console.WriteLine("[DB Producer]:No change or data found by specific Date/Version/Filter");
                }
                else
                    throw new Exception("Change Tracking is not enabled either on Database or Table");
            }
            catch (Exception ex)
            {
                Console.Write(string.Format("Error: {0}", ex.Message));
                Console.WriteLine();
                Console.ReadKey();
                throw ex;
            }

            return null;
        }

    }
}
