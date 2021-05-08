using CsvHelper;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace LabWork2
{
    class Program
    {
        static void Test()
        {
            //Insertion tests
            var t1 = new RedBlackTree("a");
            if (t1.root.value != "a" || t1.root.color != Color.Black)
                Console.WriteLine("Insert test 1 failed");
            t1.Add("f");
            if (t1.root.right.value != "f" || t1.root.right.color != Color.Red)
                Console.WriteLine("Insert test 2 failed");
            t1.Add("b");
            if (t1.root.value != "b" || t1.root.left.value != "a" || t1.root.left.color != Color.Red || t1.root.right.value != "f" || t1.root.right.color != Color.Red)
                Console.WriteLine("Insert test 3 failed");
            t1.Add("g");
            if (t1.root.value != "b" || t1.root.left.value != "a" || t1.root.left.color != Color.Black || t1.root.right.value != "f" || t1.root.right.color != Color.Black || t1.root.right.right.value != "g" ||
                t1.root.right.right.color != Color.Red)
                Console.WriteLine("Insert test 4 failed");
            t1.Add("c");
            if (t1.root.value != "b" || t1.root.left.value != "a" || t1.root.left.color != Color.Black || t1.root.right.value != "f" || t1.root.right.color != Color.Black || t1.root.right.right.value != "g" ||
                t1.root.right.right.color != Color.Red || t1.root.right.left.value != "c" || t1.root.right.left.color != Color.Red)
                Console.WriteLine("Insert test 5 failed");
            t1.Add("d");
            if (t1.root.value != "b" || t1.root.left.value != "a" || t1.root.left.color != Color.Black || t1.root.right.value != "f" || t1.root.right.color != Color.Red || t1.root.right.right.value != "g" ||
                t1.root.right.right.color != Color.Black || t1.root.right.left.value != "c" || t1.root.right.left.color != Color.Black || t1.root.right.left.right.value != "d" || t1.root.right.left.color != Color.Black)
                Console.WriteLine("Insert test 6 failed");
            t1.Add("e");
            if (t1.root.right.left.right.value != "e" || t1.root.right.left.right.color != Color.Red || t1.root.right.left.right.parent.value != "d")
                Console.WriteLine("Insert test 7 failed");
            t1.Add("h");
            if (t1.root.right.right.right.value != "h" || t1.root.right.right.right.color != Color.Red || t1.root.right.right.right.parent.value != "g")
                Console.WriteLine("Insert test 8 failed");


            //Find tests
            var a = t1.Find("a");
            if (a.parent.value != "b" || a.value != "a" || a.color != Color.Black)
                Console.WriteLine("Find test 1 failed");
            var b = t1.Find("b");
            if (b != t1.root)
                Console.WriteLine("Find test 2 failed");
            var c = t1.Find("z");
            if (c != null)
                Console.WriteLine("Find test 3 failed");
            var d = t1.Find("d");
            if (d.value != "d" || d.color != Color.Black || d.parent.value != "f" || d.right.value != "e" || d.left.value != "c")
                Console.WriteLine("Find test 4 failed");

            //Print test
            var p = t1.PrintTree();
            if (p != "abcdefgh")
                Console.WriteLine("Print test failed");
            //ToList test
            var l = string.Join("", t1.ToList().Select(x => x.value));
            if (l != "abcdefgh")
                Console.WriteLine("ToList test failed");
        }

        static void Main()
        {
            Test();

            
            //Experiment
            var minElemLen = 0;
            var maxElemLen = 20;
            var minLen = 0;
            var maxLen = 100;
            var repeat = 1;
            var diff = 1;
            var xDoc = new XmlDocument();
            xDoc.Load("exp.xml");
            var xRoot = xDoc.DocumentElement;
            foreach (XmlNode xNode in xRoot)
            {

                if (xNode.Name == "minElement")
                    minElemLen = Int32.Parse(xNode.InnerText);

                if (xNode.Name == "maxElement")
                    maxElemLen = Int32.Parse(xNode.InnerText);

                if (xNode.Name == "startLength")
                    minLen = Int32.Parse(xNode.InnerText);
                
                if (xNode.Name == "diff")
                    diff = Int32.Parse(xNode.InnerText);

                if (xNode.Name == "maxLength")
                    maxLen = Int32.Parse(xNode.InnerText);

                if (xNode.Name == "repeat")
                    repeat = Int32.Parse(xNode.InnerText);
            }

            using (var writer = new StreamWriter("res.csv",false))
            using (var csv = new CsvWriter(writer, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = "\\" }))
            {
                
                for (var size = minLen; size < maxLen; size += diff)
                {
                    var rbTree = new RedBlackTree();
                    var arr = MakeRandStrToArr(size, minElemLen, maxElemLen);
                    foreach (var e in arr)
                        rbTree.Add(e);
                    csv.WriteRecord(size);
                    csv.WriteRecord(rbTree.GetIerations());//надо как-то подсчитать итерации, затраченные на это
                    csv.NextRecord();
                }
                writer.Flush();
            }
            Console.WriteLine();
        }

        private static string[] MakeRandStrToArr(int size, int minElemLen, int maxElemLen)
        {
            var arr = new string[size];
            for (var i = 0; i < size; i++)
                arr[i] = MakeRndStr(minElemLen, maxElemLen);
            return arr;
        }

        private static string MakeRndStr(int minElemLen, int maxElemLen)
        {
            var rand = new Random();
            var l = rand.Next(minElemLen, maxElemLen);
            var builder = new StringBuilder(l);
            for (var i = 0; i < l; i++)
                builder.Append((char)rand.Next('a', 'z'));
            return builder.ToString();
        }
    }
}
