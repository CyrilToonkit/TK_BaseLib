using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib
{
    public static class ExceptionHelper
    {
        public static string GetMessages(Exception e)
        {
            string message = e.Message;

            Exception curExcept = e.InnerException;

            while (curExcept != null)
            {
                message += string.Format("\nInnerException {0} :\n{1}", curExcept.Source , curExcept.Message);
                curExcept = curExcept.InnerException;
            }

            return message;
        }
    }
}
