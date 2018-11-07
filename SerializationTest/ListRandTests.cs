using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace SerializationTests
{

    [TestClass]
    public class ListRandTests
    {
        private static ListRand MakeSomeList()
        {
            var a = new ListNode();
            a.Data = "aaa";

            var b = new ListNode();
            b.Data = "bbbb";

            var c = new ListNode();
            c.Data = "ccccc";

            var d = new ListNode();
            d.Data = "dddddd";

            a.Next = b;
            a.Rand = c;

            b.Prev = a;
            b.Next = c;
            b.Rand = b;

            c.Prev = b;
            c.Next = d;

            d.Prev = c;
            d.Rand = b;

            var list = new ListRand
            {
                Head = a,
                Tail = d,
                Count = 4
            };

            return list;
        }

        [TestMethod]
        public void TestEmptyList()
        {
            var list = new ListRand();
            RunTest(list);
        }

        [TestMethod]
        public void TestOneElementList()
        {
            var list = new ListRand();
            var node = new ListNode { Data = "gora" };
            list.Head = node;
            list.Tail = node;
            list.Count = 1;
            RunTest(list);
        }

        [TestMethod]
        public void TestNumbersInData()
        {
            var list = new ListRand();
            var node = new ListNode { Data = "1234" };
            list.Head = node;
            list.Tail = node;
            list.Count = 1;
            RunTest(list);
        }

        [TestMethod]
        public void TestCase1()
        {
            var list = MakeSomeList();
            RunTest(list);
        }

        [TestMethod]
        public void TestLongStrings()
        {
            var list = MakeSomeList();

            list.Head.Data = new string('*', 5000);
            list.Head.Next.Data = new string('r', 500);
            list.Head.Next.Next.Data = new string('q', 1000);
            RunTest(list);
        }

        [TestMethod]
        public void TestEndLineStrings()
        {
            var list = MakeSomeList();

            list.Head.Data = "ddddd" + '\0' + "ssssss\r\t"+"gggg";
            list.Head.Next.Data = "nnnnnnn" + '\0' + "mmmmmmmm\n";
            
            RunTest(list);
        }

        [TestMethod]
        public void TestCase6()
        {
            var list = MakeSomeList();
            var newNode = new ListNode();
            newNode.Data = "some";

            var previous = list.Head.Next;

            list.Head.Next = newNode;
            newNode.Prev = list.Head;

            newNode.Next = previous;
            previous.Prev = newNode;
            list.Count++;

            RunTest(list);
        }

        [TestMethod]
        public void TestEmptyDataField()
        {

            var list = MakeSomeList();
            var newNode = new ListNode();
            //list.Head.Data = null;
            newNode.Data = "";

            var previous = list.Head.Next;

            list.Head.Next = newNode;
            newNode.Prev = list.Head;

            newNode.Next = previous;
            previous.Prev = newNode;
            list.Count++;

            RunTest(list);
        }

        [TestMethod]
        public void TestEmptyDataField2()
        {
            var list = MakeSomeList();
            var newNode = new ListNode();
            newNode.Data = null;

            var previous = list.Head.Next;

            list.Head.Next = newNode;
            newNode.Prev = list.Head;

            newNode.Next = previous;
            previous.Prev = newNode;
            list.Count++;

            RunTest(list);
        }


        [TestMethod]
        public void TestFileReadability()
        {
            var list2 = new ListRand();

            using (var readingStream = new FileStream("test.txt", FileMode.Open))
            {
                list2.Deserialize(readingStream);
            }
            Assert.IsTrue(true); // just too see if there are some exceptions or something

        }

        private void RunTest(ListRand list)
        {

            using (var writingStream = new FileStream("test.txt", FileMode.Create))
            {
                list.Serialize(writingStream);
            }


            var list2 = new ListRand();

            using (var readingStream = new FileStream("test.txt", FileMode.Open))
            {
                list2.Deserialize(readingStream);
            }            


            Assert.IsTrue(ListRandExtensions.IsSame(list, list2));

        }
    }
}
