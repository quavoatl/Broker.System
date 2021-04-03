namespace Broker.System.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public static class Limit
        {
            public const string GetAllByBroker = Base + "/limits/{brokerId}";
            public const string GetAll = Base + "/limits";
            public const string Get = Base + "/limit/{limitId}";
            public const string Update = Base + "/limit/{limitId}";
            public const string Create = Base + "/limit";
            public const string Delete = Base + "/limit/{limitId}";
        }
        
        public static class Cover
        {
            public const string GetAll = Base + "/covers";
        }

        public static class LoginComponentApi
        {
            public const string JwtTokenCookieKey = "jwt_token_key";
            public const string RefreshTokenCookieKey = "refresh_token_key";
            public const string Base = "http://localhost:5000/api/";
            public const string Register = Base + "register";
            public const string Login = Base + "login";
            public const string Refresh = Base + "refresh";
        }
    }
}