using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace specch
{
    class Knock_Algorithm
    {
        public string[] RemoveStringArray(string[] Array, int index)
        {
            string[] arr = new string[Array.Length - 1];
            int id = 0;
            for (int i = 0; i < Array.Length; i++)
            {
                if (i != index)
                {
                    arr[id] = Array[i];
                    id++;
                }
            }
            return arr;
        }//     Remove Array
    }

}
