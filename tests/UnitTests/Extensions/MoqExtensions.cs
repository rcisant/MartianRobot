using Moq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UnitTests.Extensions
{
    public static class MoqExtensions
    {
        public static void SetupAsync<T>(this Mock<T> instance, Expression<Func<T, Task>> expression, Action callback = null) where T : class
        {
            Task mockTask = new Task(() => { });
            mockTask.Start();
            if (callback == null)
            {
                instance.Setup(expression).Returns(mockTask);
            }
            else
            {
                instance.Setup(expression).Returns(mockTask).Callback(callback);
            }
        }

        public static void ReturnAsync<T, TResult>(this Mock<T> instance, Expression<Func<T, Task<TResult>>> expression, TResult result, Action callback = null) where T : class
        {
            if (callback == null)
            {
                instance.Setup(expression).ReturnsAsync(result);
            }
            else
            {
                instance.Setup(expression).ReturnsAsync(result).Callback(callback);
            }
        }
    }
}
