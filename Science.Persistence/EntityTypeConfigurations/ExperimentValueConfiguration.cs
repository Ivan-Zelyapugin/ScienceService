using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Science.Domain.Entity;

namespace Science.Persistence.EntityTypeConfigurations
{
    /// <summary>
    /// Конфигурация сущности <see cref="ExperimentValue"/> для Entity Framework.
    /// </summary>
    public class ExperimentValueConfiguration : IEntityTypeConfiguration<ExperimentValue>
    {
        /// <summary>
        /// Настраивает свойства и связи сущности <see cref="ExperimentValue"/>.
        /// </summary>
        /// <param name="builder">Построитель конфигурации для сущности.</param>
        public void Configure(EntityTypeBuilder<ExperimentValue> builder)
        {
            builder.HasKey(value => value.Id);
            builder.HasIndex(value => value.Id).IsUnique();

            // Обязательные поля значений эксперимента
            builder.Property(value => value.ExperimentDateTime).IsRequired();
            builder.Property(value => value.DurationSeconds).IsRequired();
            builder.Property(value => value.Indicator).IsRequired();

            // Индекс для ускорения запросов по внешнему ключу
            builder.HasIndex(x => x.FileId).HasDatabaseName("IX_Values_FileId");

            // Связь с сущностью FileMetadata
            builder.HasOne(value => value.FileMetadata)
                .WithMany()
                .HasForeignKey(value => value.FileId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
