using System.Collections.Generic;

using UnityEngine;

namespace AI.Unit
{
    public class TargetView : MonoBehaviour
    {
        private List<Collider> target = new List<Collider>();

        public List<Collider> Target { get { return target; } private set { target = value; } }

        private void OnTriggerEnter(Collider other)
        {
            if (transform.parent.tag == "Player" && other.tag == "Enemy")
                Target.Add(other);

            if (transform.parent.tag == "Enemy" && other.tag == "Player")
                Target.Add(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (transform.parent.tag == "Player" && other.tag == "Enemy")
                Target.Remove(other);

            if (transform.parent.tag == "Enemy" && other.tag == "Player")
                Target.Remove(other);
        }
    }
}