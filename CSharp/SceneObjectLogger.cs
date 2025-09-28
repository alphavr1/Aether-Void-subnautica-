using UnityEngine;

namespace Violet.Testbiome
{
    public class SceneObjectLogger : MonoBehaviour
    {
        public bool logEveryFrame = false;
        public float logInterval = 1f;
        private float timer;

        void Start()
        {
            LogAllObjects();
        }

        void Update()
        {
            if (!logEveryFrame) return;

            timer += Time.deltaTime;
            if (timer >= logInterval)
            {
                timer = 0f;
                LogAllObjects();
            }
        }

        private void LogAllObjects()
        {
            GameObject[] allObjects = FindObjectsOfType<GameObject>();

            foreach (var obj in allObjects)
            {
                if (obj.activeInHierarchy)
                {
                    Vector3 pos = obj.transform.position;
                    Vector3 rot = obj.transform.eulerAngles;
                    Debug.Log($"[SceneObjectLogger] {obj.name} - Position: {pos}, Rotation: {rot}");
                }
            }
        }
    }
}
