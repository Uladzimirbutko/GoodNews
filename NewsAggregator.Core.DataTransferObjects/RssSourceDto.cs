using System;

namespace NewsAggregator.Core.DataTransferObjects
{
    public class RssSourceDto: IDtoModel // паттерн DTO
    /* использование где бы то нибыло entities плохо для отправки данных после покидания базы мы
    начинаем использовать dto которые не используют lazy loading, include и тд.не имеют отношения
    к базе, а просто модели которые предназначены для транспортировки данных*/
    {
        public Guid Id { get; set; }
        public string SourceName { get; set; }
        public string Url { get; set; }
    }
}
