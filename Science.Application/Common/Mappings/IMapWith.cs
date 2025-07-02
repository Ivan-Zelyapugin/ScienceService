using AutoMapper;

namespace Science.Application.Common.Mappings
{
    /// <summary>
    /// Интерфейс для автоматического маппинга объектов на тип <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Тип, на который выполняется маппинг.</typeparam>
    public interface IMapWith<T>
    {
        /// <summary>
        /// Настраивает маппинг между типом <typeparamref name="T"/> и текущим типом.
        /// </summary>
        /// <param name="profile">Профиль AutoMapper для настройки маппинга.</param>
        void Mapping(Profile profile) =>
            profile.CreateMap(typeof(T), GetType());
    }
}
