namespace RoboticsContainer.Infrastructure.Configuration
{
    public static class AppConstants
    {
        public static readonly string DOCKER_COMPOSE_FILE_NAME = "docker-compose.yml";
        public static readonly string GIT_DIRECTORY_NAME = ".git";
        public static readonly string VS_CODE_DIRECTORY_NAME = ".vscode";

        public static readonly string FIREWALL_RULES_FILE_NAME = "FirewallRules.json";
        public static readonly string FIREWALL_RULES_DIRECTORY = "Infrastructure/Firewall/Rules";

        public const string SQL_SERVER_CONTAINER_NAME = "mssql";
    }
}
