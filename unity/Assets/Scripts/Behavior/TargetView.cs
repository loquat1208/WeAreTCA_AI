﻿using System.Collections.Generic;

using UnityEngine;

namespace AI.Unit
{
    public class TargetView : MonoBehaviour
    {
        private List<Collider> target = new List<Collider>();

        public List<Collider> Target { get { return target; } private set { target = value; } }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Enemy" && !Target.Contains(other))
                Target.Add(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Enemy")
                Target.Remove(other);
        }
    }
}