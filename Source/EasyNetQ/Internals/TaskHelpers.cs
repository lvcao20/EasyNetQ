using System;
using System.Threading;
using System.Threading.Tasks;

namespace EasyNetQ.Internals
{
    public static class TaskHelpers
    {
        public static Func<T1, CancellationToken, Task<T2>> FromFunc<T1, T2>(Func<T1, CancellationToken, T2> func)
        {
            return (x, c) =>
            {
                try
                {
                    return Task.FromResult(func(x, c));
                }
                catch (OperationCanceledException exception)
                {
                    return Task.FromCanceled<T2>(exception.CancellationToken);
                }
                catch (Exception exception)
                {
                    return Task.FromException<T2>(exception);
                }
            };
        }

        public static Func<T1, T2, T3, CancellationToken, Task> FromAction<T1, T2, T3>(Action<T1, T2, T3, CancellationToken> action)
        {
            return (x, y, z, c) =>
            {
                try
                {
                    action(x, y, z, c);
                    return Task.CompletedTask;
                }
                catch (OperationCanceledException exception)
                {
                    return Task.FromCanceled(exception.CancellationToken);
                }
                catch (Exception exception)
                {
                    return Task.FromException(exception);
                }
            };
        }

        public static Func<T1, T2, CancellationToken, Task> FromAction<T1, T2>(Action<T1, T2, CancellationToken> action)
        {
            return (x, y, c) =>
            {
                try
                {
                    action(x, y, c);
                    return Task.CompletedTask;
                }
                catch (OperationCanceledException exception)
                {
                    return Task.FromCanceled(exception.CancellationToken);
                }
                catch (Exception exception)
                {
                    return Task.FromException(exception);
                }
            };
        }

        public static Func<T1, CancellationToken, Task> FromAction<T1>(Action<T1, CancellationToken> action)
        {
            return (x, c) =>
            {
                try
                {
                    action(x, c);
                    return Task.CompletedTask;
                }
                catch (OperationCanceledException exception)
                {
                    return Task.FromCanceled(exception.CancellationToken);
                }
                catch (Exception exception)
                {
                    return Task.FromException(exception);
                }
            };
        }

        public static Task FromCancelled()
        {
            return Task.FromCanceled(CancellationToken.None);
        }

        public static Task<T> FromCancelled<T>()
        {
            return Task.FromCanceled<T>(CancellationToken.None);
        }

        public static Task FromException(Exception exception)
        {
            return Task.FromException(exception);
        }

        public static Task<T> FromException<T>(Exception exception)
        {
            return Task.FromException<T>(exception);
        }

        public static Task<T> FromResult<T>(T result)
        {
            return Task.FromResult(result);
        }

        public static Task Completed { get; } = Task.CompletedTask;
    }
}
