using System;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS.Utils
{
    public static class Constants
    {
        public static class Transmitter
        {
            public static class Routes
            {
                public const string SetParameter = "/api/setparameter?id={0}&value={1}";

                public const string GetParameter = "/api/getparameter?id={0}";

                public const string Authenticate = "/api/auth?password={0}";

                public static Uri BuildSetParameterUri(string parameter, string value) => new(string.Format(Routes.SetParameter, parameter, value));

                public static Uri BuildGetParameterUri(string parameter) => new(string.Format(GetParameter, parameter));

                public static Uri BuildAuthenticateUri(string password) => new(string.Format(Authenticate, password));
            }

            public static class Parameters
            {
                public const string RadioText = "rds.dsn[1].psn[0].rt";
            }
        }
    }
}