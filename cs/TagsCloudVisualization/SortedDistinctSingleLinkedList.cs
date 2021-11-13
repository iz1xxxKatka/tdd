﻿using System;
using System.Collections.Generic;

namespace TagsCloudVisualization
{
    public class SortedDistinctSingleLinkedList<TValue>
    {
        private readonly Func<TValue, TValue, bool> comparator;
        private readonly HashSet<TValue> values;
        private Node<TValue> root;

        public SortedDistinctSingleLinkedList(Func<TValue, TValue, bool> comparator)
        {
            this.comparator = comparator ?? throw new ArgumentException("Comparator mustn't be null");
            values = new HashSet<TValue>();
        }

        public void Add(TValue value)
        {
            if (!IsNewValue(value))
                return;

            var node = new Node<TValue>(value);
            if (root is null)
            {
                root = node;
                return;
            }

            Node<TValue> previousNode = null;
            var currentNode = root;
            while (currentNode != null && comparator(currentNode.Value, node.Value))
            {
                previousNode = currentNode;
                currentNode = currentNode.Next;
            }

            if (previousNode is null)
            {
                node.Next = root;
                root = node;
                return;
            }

            previousNode.Next = node;
            node.Next = currentNode;
        }

        private bool IsNewValue(TValue value)
        {
            if (values.Contains(value))
                return false;
            values.Add(value);
            return true;
        }

        public IEnumerable<TValue> ToEnumerable()
        {
            var node = root;
            while (node != null)
            {
                yield return node.Value;
                node = node.Next;
            }
        }
    }

    public class Node<TValue>
    {
        public readonly TValue Value;
        public Node<TValue> Next;

        public Node(TValue value, Node<TValue> next = null)
        {
            Value = value;
            Next = next;
        }
    }
}