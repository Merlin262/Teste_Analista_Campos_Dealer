using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TesteCamposDealer.Tests.Helpers
{
    public static class MockDbSetHelper
    {
        public static Mock<DbSet<T>> Create<T>(List<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var mock = new Mock<DbSet<T>>();

            mock.As<IDbAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(() => new TestDbAsyncEnumerator<T>(data.GetEnumerator()));

            mock.As<IQueryable<T>>().Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<T>(queryable.Provider));
            mock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            mock.Setup(m => m.Include(It.IsAny<string>())).Returns(mock.Object);

            mock.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(data.Add).Returns<T>(e => e);
            mock.Setup(m => m.Remove(It.IsAny<T>())).Callback<T>(e => data.Remove(e)).Returns<T>(e => e);
            mock.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<T>>()))
                .Callback<IEnumerable<T>>(items => { foreach (var i in items.ToList()) data.Remove(i); })
                .Returns<IEnumerable<T>>(e => e);

            return mock;
        }
    }

    internal class TestDbAsyncQueryProvider<TEntity> : IDbAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;
        public TestDbAsyncQueryProvider(IQueryProvider inner) => _inner = inner;

        public IQueryable CreateQuery(Expression expression)
            => new TestDbAsyncEnumerable<TEntity>(expression);

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            => new TestDbAsyncEnumerable<TElement>(expression);

        public object Execute(Expression expression) => _inner.Execute(expression);
        public TResult Execute<TResult>(Expression expression) => _inner.Execute<TResult>(expression);

        public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
            => Task.FromResult(Execute(expression));

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
            => Task.FromResult(Execute<TResult>(expression));
    }

    internal class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
    {
        public TestDbAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }
        public TestDbAsyncEnumerable(Expression expression) : base(expression) { }

        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
            => new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator() => GetAsyncEnumerator();

        IQueryProvider IQueryable.Provider => new TestDbAsyncQueryProvider<T>(this);
    }

    internal class TestDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;
        public TestDbAsyncEnumerator(IEnumerator<T> inner) => _inner = inner;
        public void Dispose() => _inner.Dispose();
        public Task<bool> MoveNextAsync(CancellationToken cancellationToken) => Task.FromResult(_inner.MoveNext());
        public T Current => _inner.Current;
        object IDbAsyncEnumerator.Current => Current;
    }
}
