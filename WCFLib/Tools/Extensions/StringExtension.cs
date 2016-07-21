using System;

namespace WCFLib
{
    public static class StringExtension
    {
        /// <summary>
        /// Effectue la fonction Equals mais en executant ToLower sur les 2 strings avant la fonction
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static bool LowerEquals(this String str1, String str2)
        {
            if(string.IsNullOrEmpty(str1) && string.IsNullOrEmpty(str2))
            {
                return true;
            }
            if(string.IsNullOrEmpty(str2) || string.IsNullOrEmpty(str1))
            {
                return false;
            }
            return str1.ToLower().Equals(str2.ToLower());
        }

        /// <summary>
        /// Effectue la fonction Contains mais en executant ToLower sur les 2 strings avant la fonction
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static bool LowerContains(this String str1, String str2)
        {
            if(string.IsNullOrEmpty(str1) && string.IsNullOrEmpty(str2))
            {
                return true;
            }
            if(string.IsNullOrEmpty(str2) || string.IsNullOrEmpty(str1))
            {
                return false;
            }
            return str1.ToLower().Contains(str2.ToLower());
        }

        /// <summary>
        /// Effectue la fonction EndsWith mais en executant ToLower sur les 2 strings avant la fonction
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static bool LowerEndsWith(this String str1, string str2)
        {
            if(string.IsNullOrEmpty(str1) && string.IsNullOrEmpty(str2))
            {
                return true;
            }
            if(string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2))
            {
                return false;
            }
            return str1.ToLower().EndsWith(str2.ToLower());
        }


        public static bool LowerStartsWith(this String str1, string str2)
        {
            if (string.IsNullOrEmpty(str1) && string.IsNullOrEmpty(str2))
            {
                return true;
            }
            if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2))
            {
                return false;
            }
            return str1.ToLower().StartsWith(str2.ToLower());
        }
    }
}
