using UnityEngine;

namespace ng
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance; // Важно

        public GameObject[] prefabs; // Важно

        private void Start()
        {
            Instance = this;
        }

        public static GameObject GetPrefab(string name)
        {
            return Get(name + " (UnityEngine.GameObject)", Instance.prefabs); // Важно
        }

        public static T Get<T>(string name, T[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].ToString() == name)
                {
                    return array[i];
                }
            }
            return default(T);
        }
    }
}
