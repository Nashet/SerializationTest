using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace BinarySerializer
{
    public static class BinarySerializerExtensions
    {
        internal static T GetObjectByID<T>(this Dictionary<int, T> dictionary, int id) where T : new()
        {            
            if (dictionary.TryGetValue(id, out T value))
            {
                return value;
            }
            else
            {

                var f = new T();
                dictionary.Add(id, f);
                return f;
            }
        }

        internal static ListNode DeserializeNode(this BinaryReader reader, Dictionary<int, ListNode> restoredElements, int counter, sbyte isNullFlag)
        {
            if (reader.BaseStream.Length == 0)
                return null;

            byte[] buffer = new byte[4];
            reader.Read(buffer, 0, 4);

            int randField = BitConverter.ToInt32(buffer, 0);

            bool isNullString = reader.ReadSByte() == isNullFlag;


            var text = reader.ReadString(); // have to read to skip some bytes

            var newNode = restoredElements.GetObjectByID(counter);

            if (isNullString)
                newNode.Data = null;
            else
                newNode.Data = text;

            if (randField != isNullFlag)
                newNode.Rand = restoredElements.GetObjectByID(randField);

            return newNode;
        }
    }
}