using System;

namespace Chat.WebAPI.Models
{
    /// <summary>
    /// ¬спомогательный класс на возврат данных и ошибок
    /// </summary>
    public class ErrorViewModel
    {
        public string Error { get; set; }

        public object Entity { get; set; }
    }
}
