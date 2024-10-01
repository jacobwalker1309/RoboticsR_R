using RoboticsContainer.Application.Specification.GenericSpecificationWrappers;
using RoboticsContainer.Core.Models;

namespace RoboticsContainer.Application.Specification.ContainerEntrySpecification
{
    public class DateInsertedSpecification : MinMaxSpecification<ContainerEntry, DateTime>
    {
        public DateInsertedSpecification(DateTime? minDateInserted, DateTime? maxDateInserted)
            : base(entry => entry.DateInserted, minDateInserted, maxDateInserted)
        {
        }
    }
}
