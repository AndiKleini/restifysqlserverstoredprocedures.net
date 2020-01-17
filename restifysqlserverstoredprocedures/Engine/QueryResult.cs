using System.Collections.Generic;

namespace restifysqlserverstoredprocedures.Engine
{
    public class QueryResult
    { 
        public IEnumerable<object> Result { get; set; }
        public object OutputParameter { get; set; } 
        public int Return { get; set; }
    }
}
