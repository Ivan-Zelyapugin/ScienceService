using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Science.Domain.Entity;

namespace Science.Persistence.EntityTypeConfigurations
{
    /// <summary>
    /// Конфигурация сущности <see cref="FileMetadata"/> для Entity Framework.
    /// </summary>
    public class FileMetadataConfiguration : IEntityTypeConfiguration<FileMetadata>
    {
        /// <summary>
        /// Настраивает свойства сущности <see cref="FileMetadata"/>.
        /// </summary>
        /// <param name="builder">Построитель конфигурации для сущности.</param>
        public void Configure(EntityTypeBuilder<FileMetadata> builder)
        {
            builder.HasKey(file => file.Id);
            builder.HasIndex(file => file.Id).IsUnique();

            // Настройка обязательных свойств с ограничением длины
            builder.Property(file => file.FileName).IsRequired().HasMaxLength(255);
            builder.Property(file => file.CreatedDate).IsRequired();
            builder.Property(file => file.AuthorName).IsRequired().HasMaxLength(100);
            builder.Property(file => file.FileType).IsRequired().HasMaxLength(50);
            builder.Property(file => file.FileSize).IsRequired();

            // Индекс по имени файла для оптимизации поиска
            builder.HasIndex(x => x.FileName).HasDatabaseName("IX_Files_FileName");

        }
    }
}
