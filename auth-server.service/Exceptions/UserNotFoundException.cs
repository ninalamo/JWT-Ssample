using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace auth_server.service.Exceptions
{
    public class UserNotFoundException : Exception;
    public class SignInNotAllowedException : Exception;
    public class FailedToRegisterException : Exception;
}
