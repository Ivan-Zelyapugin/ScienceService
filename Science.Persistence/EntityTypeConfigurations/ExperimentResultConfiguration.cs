using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Science.Domain.Entity;

namespace Science.Persistence.EntityTypeConfigurations
{
    /// <summary>
    /// Конфигурация сущности <see cref="ExperimentResult"/> для Entity Framework.
    /// </summary>
    public class ExperimentResultConfiguration : IEntityTypeConfiguration<ExperimentResult>
    {
        /// <summary>
        /// Настраивает свойства и связи сущности <see cref="ExperimentResult"/>.
        /// </summary>
        /// <param name="builder">Построитель конфигурации для сущности.</param>
        public void Configure(EntityTypeBuilder<ExperimentResult> builder)
        {
            builder.HasKey(result => result.Id);
            builder.HasIndex(result => result.Id).IsUnique();

            // Обязательные поля результатов эксперимента
            builder.Property(result => result.FirstExperimentStart).IsRequired();
            builder.Property(result => result.LastExperimentStart).IsRequired();
            builder.Property(result => result.MaxExperimentTime).IsRequired();
            builder.Property(result => result.MinExperimentTime).IsRequired();
            builder.Property(result => result.AvgExperimentTime).IsRequired();
            builder.Property(result => result.AvgIndicator).IsRequired();
            builder.Property(result => result.MedianIndicator).IsRequired();
            builder.Property(result => result.MaxIndicator).IsRequired();
            builder.Property(result => result.MinIndicator).IsRequired();
            builder.Property(result => result.ExperimentCount).IsRequired();

            // Связь с сущностью FileMetadata (внешний ключ FileId)
            builder.HasOne(result => result.FileMetadata)
                   .WithMany()
                   .HasForeignKey(result => result.FileId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
