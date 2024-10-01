using RoboticsContainer.Application.Specification.GenericSpecificationWrappers;
using RoboticsContainer.Core.Models;

namespace RoboticsContainer.Application.Specification.ContainerEntrySpecification
{
    public class TemperatureSpecification : MinMaxSpecification<ContainerEntry, float>
    {
        public TemperatureSpecification(float? minTemperature, float? maxTemperature)
            : base(entry => entry.Temperature, minTemperature, maxTemperature)
        {
        }
    }

}
