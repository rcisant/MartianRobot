using System;
using System.Linq.Expressions;

namespace MartianRobot.Core.Validations
{
    public static class Guard
    {
        public static void NotNull(string value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void NotNull(Guid value, string paramName)
        {
            if (Guid.Empty == value)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void NotNull(object value, string paramName)
        {
            if (value is null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void NotNullZero(object value, string paramName)
        {
            if (value is null || Convert.ToInt32(value) == 0)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void Validate<TValue>(TValue arg, Expression<Func<TValue, bool>> expression)
        {
            var func = expression.Compile();
            var isValid = func(arg);
            if (!isValid)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
