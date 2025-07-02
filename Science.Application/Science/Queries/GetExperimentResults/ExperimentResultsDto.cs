using AutoMapper;
using Science.Application.Common.Mappings;
using Science.Domain.Entity;

namespace Science.Application.Science.Queries.GetExperimentResults
{
    /// <summary>
    /// DTO для представления результатов эксперимента.
    /// </summary>
    public class ExperimentResultsDto : IMapWith<ExperimentResult>
    {
        /// <summary>
        /// Время начала первого эксперимента.
        /// </summary>
        public DateTime FirstExperimentStart { get; set; }

        /// <summary>
        /// Время начала последнего эксперимента.
        /// </summary>
        public DateTime LastExperimentStart { get; set; }

        /// <summary>
        /// Максимальная длительность эксперимента
        /// </summary>
        public int MaxExperimentTime { get; set; }

        /// <summary>
        /// Минимальная длительность эксперимента
        /// </summary>
        public int MinExperimentTime { get; set; }

        /// <summary>
        /// Средняя длительность экспериментов
        /// </summary>
        public double AvgExperimentTime { get; set; }

        /// <summary>
        /// Среднее значение показателя эксперимента.
        /// </summary>
        public double AvgIndicator { get; set; }

        /// <summary>
        /// Медианное значение показателя эксперимента.
        /// </summary>
        public double MedianIndicator { get; set; }

        /// <summary>
        /// Максимальное значение показателя эксперимента.
        /// </summary>
        public float MaxIndicator { get; set; }

        /// <summary>
        /// Минимальное значение показателя эксперимента.
        /// </summary>
        public float MinIndicator { get; set; }

        /// <summary>
        /// Общее количество проведённых экспериментов.
        /// </summary>
        public int ExperimentCount { get; set; }

        /// <summary>
        /// Настраивает маппинг между сущностью <see cref="ExperimentResult"/> и текущим DTO.
        /// </summary>
        /// <param name="profile">Профиль AutoMapper для настройки маппинга.</param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ExperimentResult, ExperimentResultsDto>()
                .ForMember(experimentResultDto => experimentResultDto.FirstExperimentStart,
                    opt => opt.MapFrom(experimentResult => experimentResult.FirstExperimentStart))
                .ForMember(experimentResultDto => experimentResultDto.LastExperimentStart,
                    opt => opt.MapFrom(experimentResult => experimentResult.LastExperimentStart))
                .ForMember(experimentResultDto => experimentResultDto.MaxExperimentTime,
                    opt => opt.MapFrom(experimentResult => experimentResult.MaxExperimentTime))
                .ForMember(experimentResultDto => experimentResultDto.MinExperimentTime,
                    opt => opt.MapFrom(experimentResult => experimentResult.MinExperimentTime))
                .ForMember(experimentResultDto => experimentResultDto.AvgExperimentTime,
                    opt => opt.MapFrom(experimentResult => experimentResult.AvgExperimentTime))
                .ForMember(experimentResultDto => experimentResultDto.AvgIndicator,
                    opt => opt.MapFrom(experimentResult => experimentResult.AvgIndicator))
                .ForMember(experimentResultDto => experimentResultDto.MedianIndicator,
                    opt => opt.MapFrom(experimentResult => experimentResult.MedianIndicator))
                .ForMember(experimentResultDto => experimentResultDto.MaxIndicator,
                    opt => opt.MapFrom(experimentResult => experimentResult.MaxIndicator))
                .ForMember(experimentResultDto => experimentResultDto.MinIndicator,
                    opt => opt.MapFrom(experimentResult => experimentResult.MinIndicator))
                .ForMember(experimentResultDto => experimentResultDto.ExperimentCount,
                    opt => opt.MapFrom(experimentResult => experimentResult.ExperimentCount));
        }
    }
}
