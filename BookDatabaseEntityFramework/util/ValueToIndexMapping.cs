using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDatabaseEntityFramework.util
{
    public class ValueToIndexMapping
    {
        private int nextIndex = 0;

        private Dictionary<string,int> valueMapping = new Dictionary<string,int>();

        private Dictionary<int, string> indexMapping = new Dictionary<int, string>();

        public ValueToIndexMapping()
        {

        }

        /// <summary>
        /// Возвращает значение по индексу 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string getValue(int index)
        {
            string value = indexMapping[index];
            return value;
        }

        /// <summary>
        /// Возвращает индекс по значению
        /// </summary>
        public int getIndex(string value)
        {
            int index = valueMapping[value];       
            if(index == null){
                index = nextIndex;
                valueMapping.Add(value,index);
                indexMapping.Add(index, value);
                nextIndex++;
            }
            return index;
        }

        /// <summary>
        /// Текущее количество элементов
        /// </summary>
        /// <returns></returns>
        public int getSize()
        {
            return valueMapping.Keys.Count;
        }
    }
}
