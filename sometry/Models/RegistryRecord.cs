using System;

namespace RegistryMonitorWPF.Models
{
    /// <summary>
    /// Представляет запись в реестре.
    /// </summary>
    public class RegistryRecord
    {
        /// <summary>
        /// Дата регистрации заявления.
        /// </summary>
        public DateTime? ApplicationDate { get; set; }

        /// <summary>
        /// Дата государственной регистрации.
        /// </summary>
        public DateTime? RegistrationDate { get; set; }

        /// <summary>
        /// Дата внесения в реестр.
        /// </summary>
        public DateTime? InclusionDate { get; set; }

        /// <summary>
        /// Дата исключения из реестра.
        /// </summary>
        public DateTime? ExclusionDate { get; set; }

        /// <summary>
        /// Дата основного события (используется в DataGrid).
        /// </summary>
        public DateTime? Date => ExclusionDate ?? InclusionDate ?? RegistrationDate ?? ApplicationDate;

        /// <summary>
        /// Название программного обеспечения.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Уникальный идентификатор записи.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Тип события (включение, исключение и т. д.).
        /// </summary>
        public string Type { get; set; }
    }
}
