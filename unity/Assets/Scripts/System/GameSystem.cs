using UnityEngine;

namespace AI.System
{
    public class GameSystem : MonoBehaviour
    {

        void Start()
        {
            if (Display.displays.Length > 1)
                Display.displays[1].Activate();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
