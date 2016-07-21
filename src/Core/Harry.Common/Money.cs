using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harry
{

    public class Money : ValueObject<Money>
    {
        protected readonly decimal Value;

        public Money()
            : this(0m)
        {
        }

        public Money(decimal value)
        {
            validate(value);

            Value = value;
        }

        public Money Add(Money money)
        {
            return new Money(Value + money.Value);
        }

        public Money Subtract(Money money)
        {
            return new Money(Value - money.Value);
        }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<Object>() { Value };
        }

        private void validate(decimal value)
        {
            if (value % 0.01m != 0)
                throw new Exception("小数点后位数超过2位");
            if (value < 0)
            {
                throw new Exception("数值不能小于0");
            }
        }
    }

}
