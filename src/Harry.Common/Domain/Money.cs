#if !NET20
using System;
using System.Collections.Generic;

namespace Harry.Domain
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

        protected virtual void validate(decimal value)
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
#endif
