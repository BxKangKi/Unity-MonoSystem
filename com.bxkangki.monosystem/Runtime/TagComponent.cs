using UnityEngine;

namespace MonoSystem
{
    public class TagComponent : MonoBehaviour
    {
        public string[] tags;

        public virtual bool CheckTagEqual(string s)
        {
            return System.Array.Exists(tags, element => element == s);
        }

        public void Add(string s)
        {
            if (tags == null)
            {
                tags = new string[] { s };
            }
            else
            {
                System.Array.Resize(ref tags, tags.Length + 1);
                tags[tags.Length - 1] = s;
            }
        }

        public void Remove(string s)
        {
            if (tags != null)
            {
                tags = System.Array.FindAll(tags, tag => tag != s);
            }
        }
    }
}
