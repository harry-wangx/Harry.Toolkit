using System;


namespace Harry
{
#if COREFX

#else
    [Serializable]
#endif
    public abstract class ExceptionArgs
    {
        public virtual string Message { get { return string.Empty; } }
    }
}
