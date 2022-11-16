using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using static Dapper.SqlMapper;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Entities.Models;
using Microsoft.Extensions.Configuration;

namespace Data
{
    public class DapperRepository : IDapperRepository, IDisposable
    {
        private readonly IConfiguration _config;
        private readonly ILogger<DapperRepository> _logger;
        private readonly string Connectionstring = "SqlConnection";

        public DapperRepository(IConfiguration config, IConnectionString connectionString, ILogger<DapperRepository> logger)
        {
            _config = config;
            Connectionstring = connectionString.connectionString;
            _logger = logger;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }



        public async Task<int> ExecuteAsync(string sp, object param = null, CommandType commandType = CommandType.StoredProcedure)
        {
            int i = -1;
            try
            {
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    i = await db.ExecuteAsync(sp, param, commandType: commandType);
                }
            }
            catch (Exception ex)
            {
                var w32ex = ex as SqlException;
                if (w32ex == null)
                {
                    w32ex = ex.InnerException as SqlException;
                }
                if (w32ex != null)
                {
                    i = w32ex.Number;
                }
                _logger.LogError(ex, ex.Message);
            }
            return i;
        }

        public async Task<T> GetAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            T result;
            try
            {
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    var response = await db.QueryAsync<T>(sp, parms, commandType: commandType);
                    result = response.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
            return result;
        }

        public async Task<T> GetAsync<T>(string sp, object parms = null, CommandType commandType = CommandType.Text)
        {
            T result;
            try
            {
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    var response = await db.QueryAsync<T>(sp, parms, commandType: commandType);
                    result = response.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
            return result;
        }

        public IQueryable<T> GetAsQueryable<T>(string sp, object parms = null, CommandType commandType = CommandType.Text)
        {
            IQueryable<T> result;
            try
            {
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    var response = db.Query<T>(sp, parms, commandType: commandType);
                    result = response.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
            return result;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    var result = await db.QueryAsync<T>(sp, parms, commandType: commandType);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new List<T>();
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string sp, object parms = null, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    var result = await db.QueryAsync<T>(sp, parms, commandType: commandType);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new List<T>();
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(T entity, string sp)
        {
            try
            {
                var prepared = PrepareParameters(sp, entity.ToDictionary());
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    var result = await db.QueryAsync<T>(prepared.preparedQuery, prepared.dynamicParameters, commandType: CommandType.Text);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new List<T>();
            }
        }

        public async Task<T> InsertAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using (IDbConnection db = new SqlConnection(Connectionstring))
            {
                try
                {
                    if (db.State == ConnectionState.Closed)
                        db.Open();

                    using (var tran = db.BeginTransaction())
                    {
                        try
                        {
                            var resultAsync = await db.QueryAsync<T>(sp, parms, commandType: commandType, transaction: tran);
                            result = resultAsync.FirstOrDefault();
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            _logger.LogError(ex, ex.Message);
                            throw ex;
                        }
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    throw ex;
                }
                finally
                {
                    if (db.State == ConnectionState.Open)
                        db.Close();
                }
            }
            return result;
        }

        public async Task<T> UpdateAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using (IDbConnection db = new SqlConnection(Connectionstring))
            {
                try
                {
                    if (db.State == ConnectionState.Closed)
                        db.Open();

                    using (var tran = db.BeginTransaction())
                    {
                        try
                        {
                            var resultAsync = await db.QueryAsync<T>(sp, parms, commandType: commandType, transaction: tran);
                            result = resultAsync.FirstOrDefault();
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            _logger.LogError(ex, ex.Message);
                            throw ex;
                        }
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    throw ex;
                }
                finally
                {
                    if (db.State == ConnectionState.Open)
                        db.Close();
                }

            }

            return result;
        }

        public async Task<dynamic> GetMultipleAsync<T1, T2>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    var result = await db.QueryMultipleAsync(sp, parms, commandType: commandType).ConfigureAwait(false);
                    var res = new
                    {
                        Table1 = result.Read<T1>(),
                        Table2 = result.Read<T2>()
                    };
                    return res;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
        }



        public async Task<dynamic> GetMultipleAsync<T1, T2, TReturn>(string sp, object parms, Func<T1, T2, TReturn> p, string splitOn
            , CommandType commandType = CommandType.StoredProcedure)
        {
            parms = prepareParam((JSONAOData)parms);
            var res = new JDataTable<TReturn>
            {
                Data = new List<TReturn>(),
                PageSetting = new PageSetting()
            };
            try
            {
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    using (var reader = await db.QueryMultipleAsync(sp, param: parms, commandType: commandType))
                    {
                        var pgstng = reader.Read<PageSetting>();
                        var stuff = reader.Read<T1, T2, TReturn>(p, splitOn: splitOn).ToList();
                        res = new JDataTable<TReturn>
                        {
                            Data = stuff,
                            PageSetting = pgstng.FirstOrDefault()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }

        public async Task<dynamic> GetMultipleAsync<T1, T2, TReturn>(string sp, DynamicParameters parms, Func<T1, T2, TReturn> p, string splitOn
            , CommandType commandType = CommandType.StoredProcedure)
        {
            var res = new JDataTable<TReturn>
            {
                Data = new List<TReturn>(),
                PageSetting = new PageSetting()
            };
            try
            {
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    using (var reader = await db.QueryMultipleAsync(sp, parms, commandType: commandType))
                    {
                        var pgstng = reader.Read<PageSetting>();
                        var stuff = reader.Read<T1, T2, TReturn>(p, splitOn: splitOn).ToList();
                        res = new JDataTable<TReturn>
                        {
                            Data = stuff,
                            PageSetting = pgstng.FirstOrDefault()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }

        public async Task<dynamic> GetMultipleAsync<T1, T2, T3>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    var result = await db.QueryMultipleAsync(sp, parms, commandType: commandType).ConfigureAwait(false);
                    var res = new
                    {
                        Table1 = result.Read<T1>(),
                        Table2 = result.Read<T2>(),
                        Table3 = result.Read<T3>(),
                    };
                    return res;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

        public async Task<GridReader> GetMultipleAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    var result = await db.QueryMultipleAsync(sp, parms, commandType: commandType).ConfigureAwait(false);
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

        public DbConnection GetDbconnection() => new SqlConnection(_config.GetConnectionString(Connectionstring));

        public IDbConnection GetMasterConnection() => new SqlConnection(_config.GetConnectionString("MasterConnection"));

        public IEnumerable<TReturn> Get<T1, T2, TReturn>(string sqlQuery, Func<T1, T2, TReturn> p, string splitOn, DynamicParameters parms = null, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    var result = db.Query<T1, T2, TReturn>(sqlQuery, p, splitOn: splitOn, param: parms, commandType: commandType);
                    return result;
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

        public async Task<IEnumerable<TReturn>> GetAllAsync<T1, T2, TReturn>(string sqlQuery, object parms, Func<T1, T2, TReturn> p, string splitOn, CommandType commandType)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    var result = await db.QueryAsync<T1, T2, TReturn>(sqlQuery, p, splitOn: splitOn, param: parms, commandType: commandType);
                    return result;
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

        public async Task<IEnumerable<TReturn>> GetAllAsync<T1, T2, T3, TReturn>(string sqlQuery, object parms, Func<T1, T2, T3, TReturn> p, string splitOn, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    var result = await db.QueryAsync<T1, T2, T3, TReturn>(sqlQuery, p, splitOn: splitOn, param: parms, commandType: commandType);
                    return result;
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

        #region t1 To t7 Using Proc
        public async Task<IEnumerable<TReturn>> GetAllAsyncProc<T1, T2, TReturn>(T1 entity, string sqlQuery,
            DynamicParameters parms, Func<T1, T2, TReturn> p, string splitOn)
        {
            try
            {
                var prepared = PrepareParameters(sqlQuery, entity.ToDictionary());
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    var result = await db.QueryAsync<T1, T2, TReturn>(sqlQuery, p, splitOn: splitOn, param: parms, commandType: CommandType.StoredProcedure);
                    return result;
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
        }



        public async Task<IEnumerable<TReturn>> GetAllAsyncProc<T1, T2, T3, TReturn>(T1 entity, string sqlQuery,
            DynamicParameters parms, Func<T1, T2, T3, TReturn> p, string splitOn)
        {
            try
            {
                var prepared = PrepareParameters(sqlQuery, entity.ToDictionary());
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    var result = await db.QueryAsync<T1, T2, T3, TReturn>(sqlQuery, p, splitOn: splitOn, param: parms, commandType: CommandType.StoredProcedure);
                    return result;
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

        public async Task<IEnumerable<TReturn>> GetAllAsyncProc<T1, T2, T3, T4, TReturn>(T1 entity, string sqlQuery
            , DynamicParameters parms, Func<T1, T2, T3, T4, TReturn> p, string splitOn)
        {
            try
            {
                //var prepared = PrepareParameters(sqlQuery, entity.ToDictionary());
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    var result = await db.QueryAsync<T1, T2, T3, T4, TReturn>(sqlQuery, p, splitOn: splitOn, param: parms, commandType: CommandType.StoredProcedure);
                    return result;
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

        public async Task<IEnumerable<TReturn>> GetAllAsyncProc<T1, T2, T3, T4, T5, T6, T7, TReturn>(T1 entity, string sqlQuery,
            DynamicParameters parms, Func<T1, T2, T3, T4, T5, T6, T7, TReturn> p, string splitOn)
        {
            try
            {
                //var prepared = PrepareParameters(sqlQuery, entity.ToDictionary());
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    var result = await db.QueryAsync<T1, T2, T3, T4, T5, T6, T7, TReturn>(sqlQuery, p, splitOn: splitOn, param: parms, commandType: CommandType.StoredProcedure);
                    return result;
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
        }






        #endregion

        public async Task<IEnumerable<TReturn>> GetMultiSplit<T1, T2, TReturn>(string sqlQuery, Func<T1, T2, TReturn> p, string splitOn, DynamicParameters parms = null, CommandType commandType = CommandType.StoredProcedure)
        {

            try
            {
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    var result = await db.QueryAsync<T1, T2, TReturn>(sqlQuery, p, splitOn: splitOn, param: parms, commandType: commandType);
                    return result;
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
        }
        public async Task<JDataTable<T1>> GetJDatTableAsync<T1>(string sp, object parms, CommandType commandType = CommandType.StoredProcedure)
        {
            parms = prepareParam((JSONAOData)parms);
            var res = new JDataTable<T1>
            {
                Data = new List<T1>(),
                PageSetting = new PageSetting()
            };
            try
            {
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    using (var reader = await db.QueryMultipleAsync(sp, param: parms, commandType: commandType))
                    {
                        IEnumerable<T1> stuff = new List<T1>();
                        var pgstng = reader.Read<PageSetting>();
                        if (!reader.IsConsumed)
                        {
                            stuff = reader.Read<T1>();
                        }
                        res = new JDataTable<T1>
                        {
                            Data = stuff?.ToList(),
                            PageSetting = pgstng.FirstOrDefault()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }

        public async Task<dynamic> GetMultipleAsync<T1, T2, T3, TReturn>(string sp, object parms, Func<T1, T2, T3, TReturn> p, string splitOn
     , CommandType commandType = CommandType.StoredProcedure)
        {
            parms = prepareParam((JSONAOData)parms);
            var res = new JDataTable<TReturn>
            {
                Data = new List<TReturn>(),
                PageSetting = new PageSetting()
            };
            try
            {
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    using (var reader = await db.QueryMultipleAsync(sp, param: parms, commandType: commandType))
                    {
                        var pgstng = reader.Read<PageSetting>();
                        var stuff = reader.Read<T1, T2, T3, TReturn>(p, splitOn: splitOn).ToList();
                        res = new JDataTable<TReturn>
                        {
                            Data = stuff,
                            PageSetting = pgstng.FirstOrDefault()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }



        public async Task<IEnumerable<TReturn>> GetAsync<T1, T2, TReturn>(string sqlQuery, Func<T1, T2, TReturn> p, string splitOn, DynamicParameters parms = null, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    var result = await db.QueryAsync<T1, T2, TReturn>(sqlQuery, p, splitOn: splitOn, param: parms, commandType: commandType);
                    return result;
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

        public Parameters PrepareParameters(string sqlQuery, Dictionary<string, dynamic> args = null)
        {
            var result = new Parameters();
            try
            {
                string condtion = " Where 1=1 ";
                StringBuilder sb = new StringBuilder(condtion);
                List<arg> argList = new List<arg>();
                var dbParam = new DynamicParameters();
                if (args != null)
                {
                    string val = string.Empty;
                    foreach (var pair in args)
                    {
                        val = Convert.ToString(pair.Value);
                        if (!string.IsNullOrEmpty(val) && !val.Equals("false", StringComparison.OrdinalIgnoreCase) && val != "0")
                        {
                            val = val.Equals("true", StringComparison.OrdinalIgnoreCase) ? "1" : val;
                            dbParam.Add(pair.Key, pair.Value);
                            sb.Append(" and ");
                            sb.Append(pair.Key);
                            sb.Append("=");
                            sb.Append("@");
                            sb.Append(pair.Key);
                            //sb.Append("=");
                            //sb.Append("'");
                            //sb.Append(val);
                            //sb.Append("'");
                            argList.Add(new arg
                            {
                                Key = pair.Key,
                                Value = Convert.ToString(pair.Value)
                            });
                        }
                    }
                }
                string Concat = string.Concat(sqlQuery, sb);
                result = new Parameters
                {
                    dynamicParameters = dbParam,
                    preparedQuery = Concat,
                    arguments = argList
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }

            return result;
        }

        public Task<IEnumerable<TReturn>> GetAllAsyncProc<T1, T2, T3, TReturn>(T1 entity, string sqlQuery, Func<T1, T2, TReturn> p, string splitOn)
        {
            throw new NotImplementedException();
        }

        public Task<dynamic> GetMultipleAsync<T1, T2>(string sp, object parms, CommandType commandType = CommandType.StoredProcedure)
        {
            throw new NotImplementedException();
        }

        private DynamicParameters prepareDynamicParam(JSONAOData param)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add(nameof(param.draw), param.draw);
            return p;
        }

        private object prepareParam(JSONAOData param)
        {
            DynamicParameters p = new DynamicParameters();
            var _additional = new Dictionary<string, dynamic>();
            try
            {
                _additional = param.param?.ToDictionary() ?? _additional;
                foreach (var item in _additional)
                {
                    p.Add(item.Key, item.Value);
                }
                p.Add(nameof(param.start), param.start);
                p.Add(nameof(param.length), param.length);
                if (param.search != null && !string.IsNullOrEmpty(param.search.value))
                    p.Add("searchText", param.search.value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return p;
        }
    }
}
