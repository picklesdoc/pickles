/***************************************************************************

Copyright (c) Microsoft Corporation 2011.

This code is licensed using the Microsoft Public License (Ms-PL).  The text of the license
can be found here:

http://www.microsoft.com/resources/sharedsource/licensingbasics/publiclicense.mspx

 This code was modified to include a comment that the usage of System.IO is a legitimate one.

***************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using StringWriter = System.IO.StringWriter;

namespace PicklesDoc.Pickles.DocumentationBuilders.Word.TableOfContentsAdder
{
    public static class PtExtensions
    {
        public static string StringConcatenate(this IEnumerable<string> source)
        {
            var sb = new StringBuilder();
            foreach (string s in source)
                sb.Append(s);
            return sb.ToString();
        }

        public static string StringConcatenate<T>(
            this IEnumerable<T> source,
            Func<T, string> projectionFunc)
        {
            return source.Aggregate(
                new StringBuilder(),
                (s, i) => s.Append(projectionFunc(i)),
                s => s.ToString());
        }

        public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, TResult> func)
        {
            IEnumerator<TFirst> ie1 = first.GetEnumerator();
            IEnumerator<TSecond> ie2 = second.GetEnumerator();
            while (ie1.MoveNext() && ie2.MoveNext())
                yield return func(ie1.Current, ie2.Current);
        }

        public static IEnumerable<IGrouping<TKey, TSource>> GroupAdjacent<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            TKey last = default(TKey);
            bool haveLast = false;
            var list = new List<TSource>();

            foreach (TSource s in source)
            {
                TKey k = keySelector(s);
                if (haveLast)
                {
                    if (!k.Equals(last))
                    {
                        yield return new GroupOfAdjacent<TSource, TKey>(list, last);
                        list = new List<TSource>();
                        list.Add(s);
                        last = k;
                    }
                    else
                    {
                        list.Add(s);
                        last = k;
                    }
                }
                else
                {
                    list.Add(s);
                    last = k;
                    haveLast = true;
                }
            }
            if (haveLast)
                yield return new GroupOfAdjacent<TSource, TKey>(list, last);
        }

        private static void InitializeReverseDocumentOrder(XElement element)
        {
            XElement prev = null;
            foreach (XElement e in element.Elements())
            {
                e.AddAnnotation(new ReverseDocumentOrderInfo { PreviousSibling = prev });
                prev = e;
            }
        }

        public static IEnumerable<XElement> ElementsBeforeSelfReverseDocumentOrder(
            this XElement element)
        {
            if (element.Annotation<ReverseDocumentOrderInfo>() == null)
                InitializeReverseDocumentOrder(element.Parent);
            XElement current = element;
            while (true)
            {
                XElement previousElement = current
                    .Annotation<ReverseDocumentOrderInfo>()
                    .PreviousSibling;
                if (previousElement == null)
                    yield break;
                yield return previousElement;
                current = previousElement;
            }
        }

        public static string ToStringNewLineOnAttributes(this XElement element)
        {
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            settings.NewLineOnAttributes = true;
            var stringBuilder = new StringBuilder();
            using (var stringWriter = new StringWriter(stringBuilder))
            using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
                element.WriteTo(xmlWriter);
            return stringBuilder.ToString();
        }

        public static IEnumerable<XElement> DescendantsTrimmed(this XElement element,
            XName trimName)
        {
            return DescendantsTrimmed(element, e => e.Name == trimName);
        }

        public static IEnumerable<XElement> DescendantsTrimmed(this XElement element,
            Func<XElement, bool> predicate)
        {
            var iteratorStack = new Stack<IEnumerator<XElement>>();
            iteratorStack.Push(element.Elements().GetEnumerator());
            while (iteratorStack.Count > 0)
            {
                while (iteratorStack.Peek().MoveNext())
                {
                    XElement currentXElement = iteratorStack.Peek().Current;
                    if (predicate(currentXElement))
                    {
                        yield return currentXElement;
                        continue;
                    }
                    yield return currentXElement;
                    iteratorStack.Push(currentXElement.Elements().GetEnumerator());
                }
                iteratorStack.Pop();
            }
        }

        public static IEnumerable<TResult> Rollup<TSource, TResult>(
            this IEnumerable<TSource> source,
            TResult seed,
            Func<TSource, TResult, TResult> projection)
        {
            TResult nextSeed = seed;
            foreach (TSource src in source)
            {
                TResult projectedValue = projection(src, nextSeed);
                nextSeed = projectedValue;
                yield return projectedValue;
            }
        }

        public static IEnumerable<TSource> SequenceAt<TSource>(this TSource[] source, int index)
        {
            int i = index;
            while (i < source.Length)
                yield return source[i++];
        }
    }

    public class ReverseDocumentOrderInfo
    {
        public XElement PreviousSibling;
    }

    public class GroupOfAdjacent<TSource, TKey> : IEnumerable<TSource>, IGrouping<TKey, TSource>
    {
        public GroupOfAdjacent(List<TSource> source, TKey key)
        {
            this.GroupList = source;
            this.Key = key;
        }

        private List<TSource> GroupList { get; }

        #region IEnumerable<TSource> Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<TSource>)this).GetEnumerator();
        }

        IEnumerator<TSource>
            IEnumerable<TSource>.GetEnumerator()
        {
            foreach (TSource s in this.GroupList)
                yield return s;
        }

        #endregion

        #region IGrouping<TKey,TSource> Members

        public TKey Key { get; }

        #endregion
    }


    public class XEntity : XText
    {
        public XEntity(string value) : base(value)
        {
        }

        public override void WriteTo(XmlWriter writer)
        {
            writer.WriteEntityRef(Value);
        }
    }
}
