using System.Collections.Generic;

namespace Restify3SP
{
    public class QueryResult
    { 
        public IEnumerable<object> Result { get; set; }
        public object OutputParameter { get; set; } 
        public int Return { get; set; }
    }
}
