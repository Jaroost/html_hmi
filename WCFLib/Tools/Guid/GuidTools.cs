using System;

namespace WCFLib
{
    public static class GuidTools
    {
        public static Guid NewGuid()
        {
            return Guid.NewGuid();
        }

        public static string NewGuidStr()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
