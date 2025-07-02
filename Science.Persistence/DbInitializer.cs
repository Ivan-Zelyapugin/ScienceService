using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;

namespace Science.Persistence
{
    /// <summary>
    /// Класс инициализации базы данных. Проверяет наличие таблиц и при необходимости пересоздаёт базу.
    /// </summary>
    public class DbInitializer
    {
        /// <summary>
        /// Инициализирует базу данных: проверяет подключение, наличие таблиц и пересоздаёт базу при необходимости.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public static void Initialize(ExperimentsDbContext context)
        {
            if (!context.Database.CanConnect())
            {
                context.Database.EnsureCreated();
                return;
            }

            var modelTables = context.Model.GetEntityTypes()
                .Select(t => t.GetTableName())
                .Where(t => t != null)
                .ToList();

            var existingTables = context.Database
                .SqlQueryRaw<string>("SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%'")
                .ToList();

            if (modelTables.Any(table => !existingTables.Contains(table, StringComparer.OrdinalIgnoreCase)))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }
    }
}
