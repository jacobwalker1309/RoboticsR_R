using RoboticsContainer.Application.Interfaces;

namespace RoboticsContainer.Application.Specification.SpecificationLayoutComponents
{
    public class SpecificationFilteringOperators
    {
        public class TrueSpecification<T> : ISpecification<T>
        {
            public bool IsSatisfiedBy(T item) => true; // Always returns true
        }
        public class AndSpecification<T> : ISpecification<T>
        {
            private readonly ISpecification<T> _left;
            private readonly ISpecification<T> _right;

            public AndSpecification(ISpecification<T> left, ISpecification<T> right)
            {
                _left = left;
                _right = right;
            }

            public bool IsSatisfiedBy(T item) => _left.IsSatisfiedBy(item) && _right.IsSatisfiedBy(item);
        }

        public class OrSpecification<T> : ISpecification<T>
        {
            private readonly ISpecification<T> _left;
            private readonly ISpecification<T> _right;

            public OrSpecification(ISpecification<T> left, ISpecification<T> right)
            {
                _left = left;
                _right = right;
            }

            public bool IsSatisfiedBy(T item) => _left.IsSatisfiedBy(item) || _right.IsSatisfiedBy(item);
        }

        public class NotSpecification<T> : ISpecification<T>
        {
            private readonly ISpecification<T> _specification;

            public NotSpecification(ISpecification<T> specification)
            {
                _specification = specification;
            }

            public bool IsSatisfiedBy(T item) => !_specification.IsSatisfiedBy(item);
        }
    }

}
