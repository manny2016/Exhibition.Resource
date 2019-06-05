namespace Exhibition.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading;

    public class DefaultHttpTransientFaultDetecter
        : ExceptionTransientFaultDetecter
    {
        public const int DEFAULT_RETRY_THRESHOLD = 5;
        public static readonly HttpStatusCode[] STATUS_SET = new HttpStatusCode[] {
           HttpStatusCode.BadRequest,
            //HttpStatusCode.Forbidden,
            // HttpStatusCode.InternalServerError,
        };

        public DefaultHttpTransientFaultDetecter()
            : base(DetectException) { }

        public static bool DetectException(Exception condition,bool ifHasDetailsErrorMessageThrow)
        {
            if (condition == null)
            {
                return false;
            }

            if (condition is WebException)
            {
                using (var response = (condition as WebException).Response as HttpWebResponse)
                {

                    if (response != null)
                    {
                        if (STATUS_SET.Any(s => s == response.StatusCode))
                        {
                            using (var reader = new StreamReader(response.GetResponseStream()))
                            {
                                var body = reader.ReadToEnd();
                                if (!string.IsNullOrEmpty(body)&&ifHasDetailsErrorMessageThrow)
                                {
                                    throw new NullReferenceException();
                                }
                            }
                            if (response.StatusCode == HttpStatusCode.Forbidden)
                            {
                                Thread.Sleep(1000 * 60 * 5);
                            }
                            else
                            {
                                Thread.Sleep(1000 * 10);
                            }
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}