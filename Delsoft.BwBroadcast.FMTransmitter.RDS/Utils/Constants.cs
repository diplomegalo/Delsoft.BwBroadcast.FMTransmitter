using System;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS.Utils
{
    public static class Constants
    {
        public static class Transmitter
        {
            public static class Routes
            {
                private const string SetParameter = "/api/setparameter?id={0}&value={1}";

                private const string GetParameter = "/api/getparameter?id={0}";

                private const string Authenticate = "/api/auth?password={0}";

                public static string BuildSetParameterUri(string parameter, string value) => string.Format(Routes.SetParameter, parameter, value);

                public static string BuildGetParameterUri(string parameter) => string.Format(GetParameter, parameter);

                public static string BuildAuthenticateUri(string password) => string.Format(Authenticate, password);
            }

            public static class Parameters
            {
                public const string RadioText = "rds.dsn[1].psn[0].rt";
            }
        }
    }
}