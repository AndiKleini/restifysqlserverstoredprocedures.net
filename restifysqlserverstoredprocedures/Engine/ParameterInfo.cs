using System.Data;

namespace restifysqlserverstoredprocedures.Engine
{
    public class ParameterInfo
    {
        public static ParameterInfo From(string name, object parameterValue, ParameterDirection? direction = ParameterDirection.Input)
        {
            return new ParameterInfo() { Name = name, Value = parameterValue, Direction = direction };
        }
        public string Name { get; set; }
        public object Value { get; set; }
        public DbType? DbType { get; set; }
        public ParameterDirection? Direction { get; set; }
        public int? Size { get; set; }
    }
}
