using OPCAutomation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOPCWrapperLib
{
    public class MyOPCItem
    {
        private OPCItem item;
        private string value = "";

        /// <summary>
        /// Уникальный идентификатор на стороне клиента
        /// </summary>
        public int ClientHandle
        {
            get => item.ClientHandle; set => item.ClientHandle = value;
        }

        /// <summary>
        /// Уникальный идентификатор на стороне сервера
        /// </summary>
        public int ServerHandle
        {
            get => item.ServerHandle;
        }

        /// <summary>
        /// Имя тэга описанного в ОРС сервере
        /// </summary>
        public string ItemID
        {
            get => item.ItemID;
        }

        /// <summary>
        /// Текущее значение тэга
        /// </summary>
        public string Value
        {
            get { return value; }
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    ValueChanged?.Invoke(value);
                }
            }
        }

        /// <summary>
        /// Прямое считывание значения тега с OPC сервера
        /// </summary>
        public string OriginValue
        {
            get => item.Value;
        }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment { get; private set; }

        /// <summary>
        /// Текущее значение изменено
        /// </summary>
        public event Action<string> ValueChanged;

        public MyOPCItem(OPCItem Item, string Comment = "")
        {
            item = Item;
            this.Comment = Comment;
        }
    }
}
