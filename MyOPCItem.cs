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
        private object value = "";

        /// <summary>
        /// Предоставляет родительскую группу
        /// </summary>
        public MyOPCGroup Parent { get; }

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
        /// Путь доступа, указанный клиентом в функции добавления.
        /// </summary>
        public string AccessPath { get => item.AccessPath; }

        /// <summary>
        /// Возвращает права доступа к этому элементу.
        /// </summary>
        public int AccessRights { get => item.AccessRights; }

        /// <summary>
        /// Состояние сбора данных для этого элемента.
        /// </summary>
        public bool IsActive { get => item.IsActive; set => item.IsActive = value; }

        public short RequestedDataType { get => item.RequestedDataType; set => item.RequestedDataType = value; }

        /// <summary>
        /// Возвращает собственный тип данных на сервере.
        /// </summary>
        public short CanonicalDataType { get => item.CanonicalDataType; }

        /// <summary>
        /// Имя тэга описанного в ОРС сервере
        /// </summary>
        public object ItemID
        {
            get => item.ItemID;
        }

        /// <summary>
        /// Текущее значение тэга
        /// </summary>
        public object Value
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
        /// Возвращает качество последнего обращения к тэгу
        /// </summary>
        public int Quality { get => item.Quality; }

        /// <summary>
        /// Возвращает последнюю метку времени, считанную с сервера.
        /// </summary>
        public DateTime TimeStamp { get => item.TimeStamp; }

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
        /// Отображает тип информации Engineering Units (EU), содержащейся в EUInfo <br/>
        /// 0 - No - Информация о EU недоступна (EUInfo будет VT_EMPTY). <br/>
        /// 1 - Analog - EUInfo будет содержать SAFEARRAY состоящий из двух doubles (VT_ARRAY | VT_R8) соответствующий LOW and HI EU range. <br/>
        /// 2 - Enumerated - EUInfo будет содержать SAFEARRAY из strings (VT_ARRAY | VT_BSTR) который содержит список строк (Example: “OPEN”, “CLOSE”, “IN TRANSIT”, etc.)
        ///     соответствующих последовательностям (0, 1, 2, etc.)
        /// </summary>
        public short EUType { get => item.EUType; }
        /// <summary>
        /// Содержит информацию об EU (Engineering Units - инженерных единицах)
        /// </summary>
        public dynamic EUInfo { get => item.EUInfo; }

        /// <summary>
        /// Текущее значение изменено
        /// </summary>
        public event Action<object> ValueChanged;

        public MyOPCItem(MyOPCGroup Parent, OPCItem Item, string Comment = "")
        {
            this.Parent = Parent;
            item = Item;
            this.Comment = Comment;
        }

        /// <summary>
        /// Выполняет блокирующий вызов для чтения этого элемента с сервера. Чтение может быть вызвано только с источником (Source) (либо OPCCache, либо OPCDevice)
        /// </summary>
        /// <param name="Source">Источник данных (OPC_DS_CACHE или OPC_DS_DEVICE)</param>
        /// <param name="Value"></param>
        /// <param name="Quality"></param>
        /// <param name="TimeStamp"></param>
        public void Read(short Source, out object Value, out object Quality, out object TimeStamp)
        {
            object value = null;
            item.Read(Source, out value, out Quality, out TimeStamp);
            this.Value = value;
            Value = value;
        }
    }
}
