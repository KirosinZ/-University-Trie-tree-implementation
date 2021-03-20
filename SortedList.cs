using System;
using System.Collections;
using System.Collections.Generic;

namespace STRIALG_TRIE
{
    class SortedList<K, V> : IEnumerable<V>
        where K : IComparable
    {
        class ListNode<Key, Value>
            where Key : IComparable
        {
            public Key Accessor { get; set; }
            public Value Data { get; set; }
            public ListNode<Key, Value> Next { get; set; }

            public ListNode(Key acc, Value data, ListNode<Key, Value> next = null)
            {
                Accessor = acc;
                Data = data;
                Next = next;
            }

            public static ListNode<Key, Value> operator ++(ListNode<Key, Value> arg) => arg.Next;

            public static implicit operator bool(ListNode<Key, Value> arg) => arg != null;
        }

        ListNode<K, V> head;
        int count = 0;

        public void Add(K key, V data)
        {
            if (!head)
            {
                head = new ListNode<K, V>(key, data);
                count++;
            }
            else if (key.CompareTo(head.Accessor) < 0)
            {
                head = new ListNode<K, V>(key, data, head);
                count++;
            }
            else
            {
                var i = head;
                for (; i.Next && key.CompareTo(i.Next.Accessor) > 0; i++) ;
                if (!i.Next || key.CompareTo(i.Next.Accessor) != 0)
                {
                    i.Next = new ListNode<K, V>(key, data, i.Next);
                    count++;
                }
            }
        }

        public void Edit(K key, V data)
        {
            if (!head) return;
            var i = head;
            for (; i && key.CompareTo(i.Accessor) > 0; i++) ;
            if (i && key.CompareTo(i.Accessor) == 0) i.Data = data;
        }

        public void Delete(K key)
        {
            if (!head) return;
            if (head.Accessor.CompareTo(key) == 0)
            {
                head = head.Next;
                count--;
            }
            else
            {
                var i = head;
                for (; i.Next && key.CompareTo(i.Next.Accessor) != 0; i++) ;
                if (i.Next)
                {
                    i.Next = i.Next.Next;
                    count--;
                }
            }
        }

        public V Find(K key)
        {
            for (var i = head; i; i++)
            {
                if (i.Accessor.CompareTo(key) == 0) return i.Data;
            }
            throw new Exception("Элемента с таким ключом нет.");
        }

        public bool Contains(K key)
        {
            for (var i = head; i; i++)
            {
                if (i.Accessor.CompareTo(key) == 0) return true;
            }
            return false;
        }

        public IEnumerator<V> GetEnumerator()
        {
            for (var i = head; i; i++)
            {
                yield return i.Data;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool IsEmpty => !head;

        public int Count => count;

        public V this[K key]
        {
            get => Find(key);
            set
            {
                if (Contains(key)) Edit(key, value);
                else Add(key, value);
            }
        }
    }
}
