namespace RoboticsContainer.Application.Interfaces
{
    public interface IDockerService
    {
        Task<(string Output, string Error)> RunDockerComposeUp();
        Task<(string Output, string Error)> RunDockerComposeDown();
    }
}
