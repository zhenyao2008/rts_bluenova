///
/// @file   Identity.cs
/// @author Ying YuGang
/// @date   
/// @brief  
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///

namespace BlueNoah.RPG.PathFinding
{ 
    public class Identity 
    {
        int _Value;

        PathAgentAction<int> OnReset;

        public Identity(PathAgentAction<int> onReset)
        {
            _Value = int.MinValue;
            OnReset = onReset;
        }

        public Identity(int value)
        {
            _Value = value;
        }

        public int Increase()
        {
            if (_Value == int.MaxValue)
            {
                _Value = 0;
                if (OnReset != null)
                {
                    OnReset(_Value);
                }
            }
            _Value++;
            return _Value;
        }

        public int Value
        {
            get
            {
                return _Value;
            }
        }

        public static Identity operator ++(Identity a)
        {
            a.Increase();
            return a;
        }

        public static implicit operator Identity(int value)
        {
            return new Identity(value);
        }

        public static implicit operator int(Identity identity)
        {
            return identity.Value;
        }
    }
}