using BlueNoah.Math.FixedPoint;

namespace BlueNoah.AI.RTS
{

    public enum MoveSpeed
    {
        Run, Walk
    }
    public class ActorAttribute
    {
        static long index = 0;
        #region 1.Basic Attribute.
        public int playerId;
        public int actorTypeId;
        public long actorId;
        public int isBuilding;
        public int isWall;
        public int wallHeight;
        public int isStair;
        public int stairDirect;
        public int sizeX;
        public int sizeZ;
        public FixedPoint64 currentHealth = 200;
        public FixedPoint64 maxHealth = 200;
        public FixedPoint64 currentAttack = 10;
        public FixedPoint64 maxAttack = 10;
        public FixedPoint64 currentDefence = 1;
        public FixedPoint64 maxDefence = 1;
        #endregion

        #region 2.Plus Attribute.
        public FixedPoint64 runSpeed;
        public FixedPoint64 walkSpeed;
        public FixedPoint64 CurrentSpeed(MoveSpeed moveSpeed)
        {
            FixedPoint64 currentSpeed;
            switch (moveSpeed)
            {
                case MoveSpeed.Run:
                    currentSpeed = runSpeed;
                    break;
                case MoveSpeed.Walk:
                    currentSpeed = walkSpeed;
                    break;
                default:
                    currentSpeed = walkSpeed;
                    break;
            }
            return currentSpeed * 0.5f + currentSpeed * 0.5f * FixedPointMath.Max(currentStrength, 1) / FixedPointMath.Max(maxStrength, 1);
        }
        public FixedPoint64 currentStrength;
        public FixedPoint64 maxStrength;
        public void OnRun()
        {
            currentStrength -= 0.005f;
        }
        public void OnAction()
        {
            currentStrength -= 0.01f;
        }
        #endregion

        #region 3.Addition Attribute
        //TODO Base system type;
        #endregion

        public ActorAttribute()
        {
            actorId = index;
            index++;
        }

        public bool IsDead
        {
            get{ return currentHealth <= 0; }
        }
        //dead or effected.
        public bool IsActive
        {
            get { return !IsDead; }
        }

        public void OnDamage(FixedPoint64 damage)
        {
            currentHealth -= damage;
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }
        }
    }
}
