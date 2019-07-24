using FirebirdSql.Data.FirebirdClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PTMS.Common;
using PTMS.Persistance;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTMS.DataServices.Infrastructure
{
    public abstract class DataSyncService<TEntity> : DataSyncServiceEx<TEntity, int>
        where TEntity : class, new()
    {
        public DataSyncService(
            ApplicationDbContext dbContext,
            IOptions<AppSettings> appSettings)
            : base(dbContext, appSettings)
        {
        }
    }

    public abstract class DataSyncServiceEx<TEntity, TPKey> : IDataSyncServiceEx<TEntity, TPKey>
        where TEntity : class, new()
    {
        private string _tableName;
        private Dictionary<string, string> _columnDefenitions;
        private string _primaryKeyPropertyName;
        private string _primaryKeyFieldName;

        private readonly ApplicationDbContext _dbContext;
        private readonly FbConnectionStringBuilder _projectsConnection;
        private readonly FbConnectionStringBuilder _dataConnection;

        public DataSyncServiceEx(
            ApplicationDbContext dbContext,
            IOptions<AppSettings> appSettings)
        {
            _dbContext = dbContext;

            _tableName = GetTableName();
            _columnDefenitions = GetColumnDefinitions();
            _primaryKeyPropertyName = GetPrimaryKeyPropertyName();
            _primaryKeyFieldName = GetPrimaryKeyFieldName();

            _projectsConnection = new FbConnectionStringBuilder(appSettings.Value.ProjectsDatabaseConnection);
            _dataConnection = new FbConnectionStringBuilder(appSettings.Value.DataDatabaseConnection);
        }

        protected virtual List<string> GetPropertyNamesToSync()
        {
            return null;
        }

        protected virtual string GetInsertStatement(TEntity entity)
        {
            var columnsWithComma = string.Join(", ", _columnDefenitions.Keys);
            var primaryKeyValue = GetPrimaryKeyValue(entity);

            var sb = new StringBuilder();

            sb.AppendLine($@"
EXECUTE BLOCK 
AS
declare variable SQL_ varchar(500) = 'SELECT {columnsWithComma} FROM {_tableName} where {_primaryKeyFieldName} = {primaryKeyValue}';
");
            var variableDefenitions = _columnDefenitions.ToDictionary(x => "o_" + x.Key, x => x.Value);
            var variableWithComma = string.Join(", ", variableDefenitions.Keys.Select(x => ":" + x));

            foreach (var varName in variableDefenitions.Keys)
            {
                var varType = variableDefenitions[varName];

                sb.AppendLine($"declare variable {varName} {varType};");
            }

            sb.AppendLine($@"
BEGIN
    for execute statement (:SQL_)
    on external '{_projectsConnection.DataSource}/{_projectsConnection.Port}:{_projectsConnection.Database}'
    AS USER '{_projectsConnection.UserID}' PASSWORD '{_projectsConnection.Password}'
    into  {variableWithComma}
    do begin
       insert into {_tableName} ({columnsWithComma}) values ({variableWithComma});
    end
END
");

            var result = sb.ToString();
            return result;
        }

        protected virtual string GetDeleteStatement(TPKey id)
        {
            var sql = $"delete from {_tableName} where {_primaryKeyFieldName} = {id}";
            return sql;
        }

        public void SyncOnAdd(TEntity entity)
        {
            var sql = GetInsertStatement(entity);
            ExecuteSql(sql);
        }

        public void SyncOnUpdate(TEntity entity)
        {
            var id = GetPrimaryKeyValue(entity);
            SyncOnDelete(id);
            SyncOnAdd(entity);
        }

        public void SyncOnDelete(TPKey id)
        {
            var sql = GetDeleteStatement(id);
            ExecuteSql(sql);
        }
        
        private Dictionary<string, string> GetColumnDefinitions()
        {
            var entityType = _dbContext.Model.FindEntityType(typeof(TEntity));
            var result = new Dictionary<string, string>();
            var propertyNamesToSync = GetPropertyNamesToSync();

            foreach (var property in entityType.GetProperties())
            {
                if (propertyNamesToSync != null && !propertyNamesToSync.Contains(property.Name))
                {
                    continue;
                }

                var columnName = property.Relational().ColumnName;
                var columnType = property.Relational().ColumnType;

                result.Add(columnName, columnType);
            };

            return result;
        }

        private TPKey GetPrimaryKeyValue(TEntity entity)
        {
            return (TPKey)entity
                .GetType()
                .GetProperty(_primaryKeyPropertyName)
                .GetValue(entity, null);
        }

        private string GetPrimaryKeyPropertyName()
        {
            var keyName = _dbContext.Model
                .FindEntityType(typeof(TEntity))
                .FindPrimaryKey()
                .Properties
                .Select(x => x.Name)
                .Single();

            return keyName;
        }

        private string GetPrimaryKeyFieldName()
        {
            var keyName = _dbContext.Model
                .FindEntityType(typeof(TEntity))
                .FindPrimaryKey()
                .Properties
                .Select(x => x.Relational())
                .Single()
                .ColumnName;

            return keyName;
        }

        private string GetTableName()
        {
            var mapping = _dbContext.Model.FindEntityType(typeof(TEntity)).Relational();
            return mapping.TableName;
        }

        private void ExecuteSql(string sql)
        {
            using (var connection = new FbConnection(_dataConnection.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = new FbCommand(sql, connection, transaction))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                        }
                    }
                }
            }
        }
    }
}
