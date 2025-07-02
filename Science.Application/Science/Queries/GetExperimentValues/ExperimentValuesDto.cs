using AutoMapper;
using Science.Application.Common.Mappings;
using Science.Domain.Entity;

namespace Science.Application.Science.Queries.GetExperimentValues
{
    /// <summary>
    /// DTO для представления значений эксперимента.
    /// </summary>
    public class ExperimentValuesDto : IMapWith<ExperimentValue>
    {
        /// <summary>
        /// Дата и время проведения эксперимента.
        /// </summary>
        public DateTime ExperimentDateTime { get; set; }

        /// <summary>
        /// Длительность эксперимента в секундах.
        /// </summary>
        public int DurationSeconds { get; set; }

        /// <summary>
        /// Показатель эксперимента.
        /// </summary>
        public double Indicator { get; set; }

        /// <summary>
        /// Настраивает маппинг между сущностью <see cref="ExperimentValue"/> и текущим DTO.
        /// </summary>
        /// <param name="profile">Профиль AutoMapper для настройки маппинга.</param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ExperimentValue, ExperimentValuesDto>()
                .ForMember(experimentValueDto => experimentValueDto.ExperimentDateTime,
                    opt => opt.MapFrom(experimentValue => experimentValue.ExperimentDateTime))
                .ForMember(experimentValueDto => experimentValueDto.DurationSeconds,
                    opt => opt.MapFrom(experimentValue => experimentValue.DurationSeconds))
                .ForMember(experimentValueDto => experimentValueDto.Indicator,
                    opt => opt.MapFrom(experimentValue => experimentValue.Indicator));
        }
    }
}
