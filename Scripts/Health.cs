using UnityEngine;

namespace WolKwangChulHyeol.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float mHealthMaxPoints = 100.0f;
        [SerializeField] private float mHealthCurrentPoints;

        private bool mbDead = false;
        private bool mbHited = false;

        private void OnEnable()
        {
            mHealthCurrentPoints = mHealthMaxPoints;
        }

        public float MHealthMaxPoints
        {
            get { return mHealthMaxPoints; }
            set { mHealthMaxPoints = value; }
        }

        public float MHealthCurrentPoints
        {
            get { return mHealthCurrentPoints; }
            set { mHealthCurrentPoints = value; }
        }

        public bool IsFullHealth()
        {
            return mHealthCurrentPoints >= mHealthMaxPoints ? true : false;
        }

        public bool IsDead()
        {
            return mbDead;
        }

        public void TakeDamage(float damage, bool hit)
        {
            mbHited = hit;

            mHealthCurrentPoints = Mathf.Max(mHealthCurrentPoints - damage, 0);

            if (mHealthCurrentPoints == 0)
            {
                die();
            }
        }

        private void die()
        {
            if (mbDead) return;

            mbDead = true;
        }
    }
}
