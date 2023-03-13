using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShelfExtend.Entity.Extension
{
    public static class StringExtension
    {
        public static bool isStringLengthCorrect(this string text, int maxLength) 
        {
            if (text.Length > maxLength)
            {  
                return false; 
            }
            return true;
        }
    }
}
