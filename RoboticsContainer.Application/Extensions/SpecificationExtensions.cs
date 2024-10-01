using RoboticsContainer.Application.Interfaces;
using RoboticsContainer.Application.Specification.SpecificationLayoutComponents;

namespace RoboticsContainer.Application.Extensions
{
    public static class SpecificationExtensions
    {
        public static ISpecification<T> True<T>()
        {
            return new SpecificationFilteringOperators.TrueSpecification<T>();
        }
        public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right)
        {
            return new SpecificationFilteringOperators.AndSpecification<T>(left, right);
        }

        public static ISpecification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right)
        {
            return new SpecificationFilteringOperators.OrSpecification<T>(left, right);
        }

        public static ISpecification<T> Not<T>(this ISpecification<T> specification)
        {
            return new SpecificationFilteringOperators.NotSpecification<T>(specification);
        }
    }

}
