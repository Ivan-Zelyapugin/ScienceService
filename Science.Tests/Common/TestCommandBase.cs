using Science.Persistence;

namespace Science.Tests.Common
{
    /// <summary>
    /// Базовый класс для тестов команд (command-handlers).
    /// </summary>
    public abstract class TestCommandBase : IDisposable
    {
        protected readonly ExperimentsContextFactory Factory;
        protected readonly ExperimentsDbContext Context;

        /// <summary>
        /// Создаёт in-memory EF Core контекст и заполняет его тестовыми данными.
        /// </summary>
        public TestCommandBase()
        {
            Factory = new ExperimentsContextFactory();
            Context = Factory.Context;
        }

        public void Dispose()
        {
            Factory.Dispose();
        }
    }
}
