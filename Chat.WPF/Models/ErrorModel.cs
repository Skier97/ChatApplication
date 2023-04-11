using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.WPF.Models
{
    /// <summary>
    /// Класс для возвращения контента и ошибки, которые нужно отобразить при работе на стороне бека
    /// </summary>
    public class ErrorModel
    {
        public string Error { get; set; }

        public object Entity { get; set; }
    }
}
