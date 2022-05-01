using System.Collections.Generic;
using System.Linq;

namespace Orbit.Application.Shared
{
    public class PowerCollection<TKey, TValue> : List<KeyValuePair<TKey, TValue>>
    {
        public void Add(TKey key, TValue value)
        {
            var element = new KeyValuePair<TKey, TValue>(key, value);
            Add(element);
        }

        public bool ContainsKey(TKey key) => this.Any(x => x.Key.Equals(key));

        public IEnumerable<TValue> this[TKey key]
        {
            get
            {
                if (ContainsKey(key))
                    return this.Where(x => x.Key.Equals(key)).Select(x => x.Value);
                return new List<TValue>();
                //throw new KeyNotFoundException(
                //        string.Format(
                //            "The given key {0} was not found in the collection.", key));
            }
        }

        public IEnumerable<TKey> Keys => this.Select(x => x.Key);

        public IEnumerable<TValue> Values => this.Select(x => x.Value);

    }
}
