using RoboticsContainer.Application.Interfaces;
using System.Linq.Expressions;

namespace RoboticsContainer.Application.Specification.GenericSpecificationWrappers
{
    public class MinMaxSpecification<T, TProperty> : ISpecification<T> where TProperty : struct
    {
        private readonly Func<T, TProperty?> _selector;
        private readonly TProperty? _min;
        private readonly TProperty? _max;

        public MinMaxSpecification(Func<T, TProperty?> selector, TProperty? min, TProperty? max)
        {
            _selector = selector;
            _min = min;
            _max = max;
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            return item =>
                (!_min.HasValue || Comparer<TProperty>.Default.Compare(_selector(item).Value, _min.Value) >= 0) &&
                (!_max.HasValue || Comparer<TProperty>.Default.Compare(_selector(item).Value, _max.Value) <= 0);
        }

        public bool IsSatisfiedBy(T item)
        {
            var value = _selector(item);

            if (value.HasValue)
            {
                if (_min.HasValue && _max.HasValue)
                {
                    return Comparer<TProperty>.Default.Compare(value.Value, _min.Value) >= 0 &&
                           Comparer<TProperty>.Default.Compare(value.Value, _max.Value) <= 0;
                }
                else if (_min.HasValue)
                {
                    return Comparer<TProperty>.Default.Compare(value.Value, _min.Value) >= 0;
                }
                else if (_max.HasValue)
                {
                    return Comparer<TProperty>.Default.Compare(value.Value, _max.Value) <= 0;
                }
            }

            return true; // No filtering
        }
    }
}
