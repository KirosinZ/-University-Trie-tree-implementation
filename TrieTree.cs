using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STRIALG_TRIE
{
    public class TrieTree : IEnumerable<string>
    {
        class TrieTreeNode : IEnumerable<string>
        {
            char Data { get; set; }
            public SortedList<char, TrieTreeNode> Children { get; private set; }
            bool EndMark { get; set; }

            public TrieTreeNode(char data, bool ifend)
            {
                Data = data;
                Children = new SortedList<char, TrieTreeNode>();
                EndMark = ifend;
            }

            public void Add(string s)
            {
                char c = s[0];
                s = s.Remove(0, 1);
                if (!Children.Contains(c)) Children[c] = new TrieTreeNode(c, s == "");
                if (s != "") Children[c].Add(s);
            }

            public bool Find(string s)
            {
                if (s == "") return EndMark;
                char c = s[0];
                s = s.Remove(0, 1);
                if (Children.Contains(c)) return Children[c].Find(s);
                return false;
            }

            public bool Delete(string s)
            {
                if (s == "")
                    return EndMark;
                char c = s[0];
                s = s.Remove(0, 1);
                bool flag = Children.Contains(c) && Children[c].Delete(s);
                if (flag)
                {
                    if (Children[c].Children.IsEmpty)
                    {
                        Children.Delete(c);
                        return !EndMark;
                    }
                    else
                    {
                        Children[c].EndMark = false;
                        return false;
                    }
                }
                return false;
            }

            public bool DeleteByLetter(char c)
            {
                if (Data == c) return true;
                foreach (var node in Children)
                {
                    if (node.DeleteByLetter(c)) Children.Delete(node.Data);
                }
                if (IsTerminal) return !EndMark;
                return false;
            }

            public void Traverse(ref string buf, string curword)
            {
                curword += Data;
                if (EndMark) buf += curword;
                foreach (var node in Children)
                {
                    node.Traverse(ref buf, curword);
                }
            }

            public TreeNode ToNode()
            {
                if (IsTerminal) return new TreeNode(Data.ToString() + (EndMark ? "." : ""));
                else
                {
                    TreeNode[] tmp = new TreeNode[Children.Count];
                    int i = 0;
                    foreach (var t in Children)
                    {
                        tmp[i] = t.ToNode();
                        i++;
                    }
                    return new TreeNode(Data.ToString() + (EndMark ? "." : ""), tmp);
                }
            }

            public IEnumerable<string> RecursiveEnumeration(string buf)
            {
                if (Data!=' ') buf += Data;
                if (EndMark) yield return buf;
                if (!IsTerminal)
                {
                    foreach (var t in Children)
                    {
                        foreach (string s in t.RecursiveEnumeration(buf))
                        {
                            yield return s;
                        }
                    }
                }
            }

            public IEnumerator<string> GetEnumerator()
            {
                return RecursiveEnumeration("").GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public bool IsTerminal => Children.IsEmpty;
        }

        TrieTreeNode head = new TrieTreeNode(' ', false);

        public void AddWord(string Text) => head.Add(Text.ToWord());

        public void AddWords(string Text)
        {
            foreach (string s in Text.ToWords())
            {
                head.Add(s);
            }
        }

        public bool FindWord(string Text) => head.Find(Text.ToWord());

        public bool[] FindWords(string Text) => Text.ToWords()?.Select((s) => FindWord(s)).ToArray();

        public void DeleteWord(string Text) => head.Delete(Text.ToWord());

        public void DeleteWords(string Text)
        {
            foreach (string s in Text.ToWords())
            {
                head.Delete(s);
            }
        }

        public void ClearTree() => head = new TrieTreeNode(' ', false);

        public string[] Traverse()
        {
            string res = "";
            head.Traverse(ref res, "");
            return res.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public string[] Suggest(string s)
        {
            if (s.ToWord() == "") return new string[0];
            TrieTreeNode tmp = head;
            foreach (char c in s.ToWord())
            {
                if (tmp.Children.Contains(c)) tmp = tmp.Children[c];
                else return new string[0];
            }
            string res = "";
            foreach (var t in tmp.Children)
            {
                t.Traverse(ref res, " ");
            }
            return res.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public void DeleteByLetter(char c)
        {
            if (c == ' ') return;
            if (head.DeleteByLetter(c)) head = new TrieTreeNode(' ', false);
        }

        public TreeNode ToNode(string s)
        {
            TreeNode res = head.ToNode();
            res.Text = s;
            return res;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return head.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool IsEmpty => head.IsTerminal;
    }
}
