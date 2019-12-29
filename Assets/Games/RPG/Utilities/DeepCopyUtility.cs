///
/// @file  DeepCopyUtility.cs
/// @author Ying YuGang
/// @date   
/// @brief For snapshot.
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///
namespace BlueNoah.RPG.AI
{
    public static class DeepCopyUtility 
    {
        public static T DeepClone<T>(T source) where T : class
        {
#if UNITY_EDITOR
            var memoryStream = new System.IO.MemoryStream();

            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            binaryFormatter.Serialize(memoryStream,source);

            memoryStream.Seek(0, System.IO.SeekOrigin.Begin);

            return (T)binaryFormatter.Deserialize(memoryStream);
#else
            return null;
#endif
        }
    }
}

