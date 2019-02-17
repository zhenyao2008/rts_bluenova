using BlueNoah.Math.FixedPoint;

namespace BlueNoah.PathFinding.FixedPoint
{

    public class FixedPointGridSetting 
    {

        public FixedPoint64 nodeWidth = 1;
        //diagonal cost.
        public FixedPoint64 diagonalPlus = 1.4f;

        public FixedPointVector3 startPos = new FixedPointVector3(0,0,0);

        public FixedPointVector3 offsetPos = new FixedPointVector3(0,0,0);

        public uint xCount = 100;

        public uint zCount = 50;

        public int neighborCount = 8;

    }
}

