using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wy.Hr.Infrastructure.Abstract
{
    public interface IAuthProvider
    {
        bool Authenticate(string userName, string password);

    }
}
