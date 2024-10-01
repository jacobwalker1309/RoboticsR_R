using RoboticsContainer.Application.Interfaces;
using RoboticsContainer.Core.Models;
using System.Linq.Expressions;

namespace RoboticsContainer.Application.Specification.ContainerEntrySpecification
{
    public class ContainerIDSpecification : ISpecification<ContainerEntry>
    {
        private readonly int _containerID;

        public ContainerIDSpecification(int containerID)
        {
            _containerID = containerID;
        }

        public bool IsSatisfiedBy(ContainerEntry entry)
        {
            return entry.ContainerID == _containerID;
        }

        public Expression<Func<ContainerEntry, bool>> ToExpression()
        {
            return entry => entry.ContainerID == _containerID;
        }
    }
}
