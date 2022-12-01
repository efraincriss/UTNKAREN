using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Constantes
{
    public static class StringHelper
    {
        public static string FormtSixDigits(int number)
        {
            var result = String.Format("{0:000000}", number);
            return result;
        }
    }
}
