
using System;
using System.Collections.Generic;
using System.IO;


public class ListNode
{
    public ListNode Prev;
    public ListNode Next;
    public ListNode Rand; //произвольный элемент внутри  списка
    public string Data;
}
//fixit exceptions                                
//fixit arguments   
//fixit complexity
public class ListRand
{    
    //private const sbyte isNullFlag = -1, isNotNullFlag = 0;

    public ListNode Head;
    public ListNode Tail;
    public int Count;

    public void Serialize(FileStream s)
    {
        // each node is written in following format:
        // first 4 bytes - Rand field written as ID (integer) counting from 0
        // if Rand is null than -1 is written
        // next 1 byte - "following string is null" flag, -1 - is null, other value - not null 
        // next is a length-prefixed (4 bytes) string containing Data field

        // default encoding is UTF-8


        // associate each node with unique ID
        // complexity - O(n)
        var nodeIDs = new Dictionary<ListNode, int>(Count);
        var nextElement = Head;
        int counter = 0;
        while (nextElement != null)
        {
            nodeIDs.Add(nextElement, counter); //complexity - O(1) because capacity is set correctly
            nextElement = nextElement.Next;
            counter++;
        }

        //write data to stream, complexity - O(n) 
        using (BinaryWriter writer = new BinaryWriter(s))
        {
            var nextNode = Head;//start with Head
            //run over all elements
            while (nextNode != null)
            {
                if (nextNode.Rand == null)
                    writer.Write(-1);
                else
                    writer.Write(nodeIDs[nextNode.Rand]); //dictionary access - O(1)

                if (nextNode.Data == null)
                {
                    writer.Write((sbyte)-1);
                    writer.Write("");
                }
                else
                {
                    writer.Write((sbyte)0);
                    writer.Write(nextNode.Data);
                }
                nextNode = nextNode.Next;
            }
        }
    }

    public void Deserialize(FileStream s)
    {
        // keeps already restored nodes with IDs
        var restoredElements = new Dictionary<int, ListNode>();
        using (BinaryReader reader = new BinaryReader(s))
        {
            ListNode firstNode = reader.DeserializeNode(restoredElements, 0, -1);

            if (firstNode != null)
            {
                Count++;
                Tail = firstNode;
                Head = firstNode;

                var previousNode = firstNode;
                while (s.Position != s.Length)
                {
                    var currentNode = reader.DeserializeNode(restoredElements, Count, -1);
                    currentNode.Prev = previousNode;
                    previousNode.Next = currentNode;


                    previousNode = currentNode;
                    Count++;
                    Tail = currentNode;
                }
            }
        }
    }
}

