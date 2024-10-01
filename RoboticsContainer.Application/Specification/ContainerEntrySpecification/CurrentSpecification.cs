using RoboticsContainer.Application.Interfaces;
using RoboticsContainer.Application.Specification.GenericSpecificationWrappers;
using RoboticsContainer.Core.Models;

namespace RoboticsContainer.Application.Specification.ContainerEntrySpecification
{
    public class CurrentSpecification : MinMaxSpecification<ContainerEntry, float>
    {
        public CurrentSpecification(float? minCurrent, float? maxCurrent)
            : base(entry => entry.Current, minCurrent, maxCurrent)
        {
        }
    }
}
