using System.Collections.Generic;

namespace GB2260
{
    public class Gb2260Factory
    {
        private static readonly IDictionary<Revision, Gb2260> Dictionary = new Dictionary<Revision, Gb2260>();
        public static Gb2260 Create(Revision revision = Revision.V201607)
        {
            Gb2260 result;
            if(Dictionary.TryGetValue(revision, out result))
            {
                return result;
            }
            result = new Gb2260(revision);
            Dictionary.Add(revision, result);
            return result;
        }
    }
}