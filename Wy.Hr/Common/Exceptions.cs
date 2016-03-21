using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wy.Hr.Common
{
    [Serializable]
    public class SDKRuntimeException : Exception
    {

        private const long serialVersionUID = 1L;

        public SDKRuntimeException(String str)
            : base(str)
        {

        }
    }
}
