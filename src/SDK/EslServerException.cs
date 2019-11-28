using System;
using System.Net;
using System.IO;
using Silanis.ESL.API;
using Newtonsoft.Json;

namespace Silanis.ESL.SDK
{
    public class EslServerException:EslException
    {

        public ServerError ServerError
        {
            get; set;
        }
    
        public EslServerException(string message, ServerError serverError, EslServerException cause):base(message, cause)
        {
            this.ServerError = serverError;
        }

        public EslServerException(string message, string errorDetails, WebException cause):base(message, cause)
        {
            Error e;

            try
            {
                e = JsonConvert.DeserializeObject<Error>(errorDetails);
            }
            catch(Exception)
            {
                e = new Error() { Code = 500 };
            }
            this.ServerError = new ErrorConverter(e).ToServerError();
        }
        
    }
}
