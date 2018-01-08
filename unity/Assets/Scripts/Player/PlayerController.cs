using UnityEngine;

namespace AI.Player
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerModel Model { get; set; }

        void Start()
        {
            Model = new PlayerModel();
        }
    }
}
