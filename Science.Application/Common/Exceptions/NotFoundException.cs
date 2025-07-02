namespace Science.Application.Common.Exceptions
{
    /// <summary>
    /// Исключение, возникающее при отсутствии запрашиваемого файла.
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Инициализирует новый экземпляр исключения с указанием имени файла.
        /// </summary>
        /// <param name="name">Имя файла, который не был найден.</param>
        public NotFoundException(string name)
            : base($"Файл \"{name}\" не найден.") { }
    }
}
