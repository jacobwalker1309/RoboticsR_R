using RoboticsContainer.Application.Interfaces;
using RoboticsContainer.Infrastructure.Configuration;

public class PathService : IPathService
{
    // Static variable to hold the project root once it's resolved
    private static string _projectRoot;
    public string GetProjectRoot()
    {
        if (!string.IsNullOrEmpty(_projectRoot))
        {
            return _projectRoot;
        }
        // this is going to need to be stored once in redis on even in just a global variable here somewhere
        // in case redis service goes down
        // Start from the current base directory (bin folder in most cases)
        var currentDirectory = AppContext.BaseDirectory;

        // Traverse upwards to the root folder until we find "RoboticsContainer.Infrastructure"
        while (!Directory.Exists(Path.Combine(currentDirectory, "RoboticsContainer.Infrastructure")))
        {
            // Go up one directory level
            currentDirectory = Directory.GetParent(currentDirectory)?.FullName;

            // Safety check to avoid an infinite loop if the directory doesn't exist
            if (string.IsNullOrEmpty(currentDirectory))
            {
                throw new DirectoryNotFoundException("Unable to locate the RoboticsContainer.Infrastructure directory.");
            }
        }

        _projectRoot = currentDirectory;

        // for single instance run, i.e this one its ideal, if more instances in the future will need to run redis
        // can't see this being an issue, its more the database side of things that will need redis/elastisearch/apache etc

        return currentDirectory;
    }

    public string GetDockerComposeFilePath()
    {
        var rootPath = GetProjectRoot();
        return Path.Combine(rootPath, AppConstants.DOCKER_COMPOSE_FILE_NAME);
    }

    public string GetFirewallRulesFilePath()
    {
        var projectRoot = GetProjectRoot();
        var filePath = Path.Combine(projectRoot, "RoboticsContainer.Infrastructure", "Firewall", "Rules", "FirewallRules.json");

        // Normalize the path
        filePath = Path.GetFullPath(filePath);

        return filePath;
    }
}
