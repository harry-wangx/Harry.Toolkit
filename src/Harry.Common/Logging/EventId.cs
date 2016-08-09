
namespace Harry.Logging
{
    /// <summary>
    /// 事件标识号.
    /// </summary>
    public struct EventId
    {
        private int _id;
        private string _name;

        public EventId(int id, string name = null)
        {
            _id = id;
            _name = name;
        }

        public int Id
        {
            get
            {
                return _id;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public static implicit operator EventId(int i)
        {
            return new EventId(i);
        }
    }
}

