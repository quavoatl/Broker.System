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
        
        public static class Question
        {
            public const string GetAll = Base + "/questions/{id}";
        }
        
        public static class Identity
        {
            public const string Login = Base + "/identity/login";
            public const string Register = Base + "/identity/register";
            public const string Refresh = Base + "/identity/refresh";
        }
    }
}