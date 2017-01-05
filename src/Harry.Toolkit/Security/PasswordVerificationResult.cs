using System;


namespace Harry.Toolkit.Security
{
    public enum PasswordVerificationResult
    {

        Failed = 0,

        Success = 1,

        SuccessRehashNeeded = 2
    }
}
