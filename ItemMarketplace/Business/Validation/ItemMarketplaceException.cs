using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validation
{
    [Serializable()]
    public class ItemMarketplaceException : Exception
    {
        public ItemMarketplaceException() { }
        public ItemMarketplaceException(string message) : base(message) { }
        public ItemMarketplaceException(string message, Exception inner) : base(message, inner) { }
        protected ItemMarketplaceException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
