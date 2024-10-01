using RoboticsContainer.Application.Specification.GenericSpecificationWrappers;
using RoboticsContainer.Core.Models;

namespace RoboticsContainer.Application.Specification.ContainerEntrySpecification
{
    public class StateOfChargeSpecification : MinMaxSpecification<ContainerEntry, float>
    {
        public StateOfChargeSpecification(float? minStateOfCharge, float? maxStateOfCharge)
            : base(entry => entry.StateOfCharge, minStateOfCharge, maxStateOfCharge)
        {
        }
    }
}
