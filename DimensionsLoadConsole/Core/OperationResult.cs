using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DimensionsLoadConsole.Core
{
    public class OperationResult
    {
        public OperationResult()
        {
            this.Success = true;
        }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
