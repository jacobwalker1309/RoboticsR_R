using RoboticsContainer.Application.Specification.GenericSpecificationWrappers;
using RoboticsContainer.Core.Models;

namespace RoboticsContainer.Application.Specification.ContainerEntrySpecification
{
    public class VoltageSpecification : MinMaxSpecification<ContainerEntry, float>
    {
        public VoltageSpecification(float? minVoltage, float? maxVoltage)
            : base(entry => entry.Voltage, minVoltage, maxVoltage)
        {
        }
    }
}
