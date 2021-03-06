﻿using System;
using System.Collections.Generic;
using System.IO;

public static class BinarySerializerExtensions
{
    /// <summary>
    /// Helps find object associated with given ID. If object isn't found - creates new one.
    /// </summary>    
    internal static T GetObjectByID<T>(this Dictionary<int, T> dictionary, int id) where T : new()
    {
        if (dictionary.TryGetValue(id, out T value))// complexity - O(1)
        {
            return value;
        }
        else
        {
            var newObject = new T();
            dictionary.Add(id, newObject);// Max complexity - O(n)
            return newObject;
        }
    }

    /// <summary>
    /// For unknown yet nodes creates record in dictionary associating node with ID.
    /// Next time you meet that ID - you take node from dictionary, keeping links correct.
    /// Doesn't restore Next and Prev fields.
    /// </summary>        
    internal static ListNode DeserializeNode(this BinaryReader reader, Dictionary<int, ListNode> restoredElements, int ID, sbyte isNullFlag)
    {
        if (reader.BaseStream.Length == 0)
            return null;

        byte[] buffer = new byte[4];
        reader.Read(buffer, 0, 4);

        int randField = BitConverter.ToInt32(buffer, 0);

        bool isNullString = reader.ReadSByte() == isNullFlag;

        var text = reader.ReadString(); // have to read to skip some bytes

        var newNode = restoredElements.GetObjectByID(ID);

        if (isNullString)
            newNode.Data = null;
        else
            newNode.Data = text;

        if (randField != isNullFlag)
            newNode.Rand = restoredElements.GetObjectByID(randField);

        return newNode;
    }
}
