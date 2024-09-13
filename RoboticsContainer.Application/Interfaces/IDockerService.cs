namespace RoboticsContainer.Application.Interfaces
{
    /// <summary>
    /// Used to control DockerCompose-Up/Down
    /// </summary>
    public interface IDockerService
    {
        Task<(string Output, string Error)> RunDockerComposeUp();
        Task<(string Output, string Error)> RunDockerComposeDown();
    }
}
