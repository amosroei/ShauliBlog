using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShauliBlog;
using System.Text;
using ShauliBlog.Models;

// Extracting frequent item-sets for Association Rule Learning.
// Extracting frequent item-sets from a set of transactions ( transaction is a sentence in a comment)
// Uses a variation of the 'Apriori' algorithm.

namespace ShauliBlog.Controllers
{
    public class AprioriAlgorithmController : Controller
    {

        private BlogDBContext db = new BlogDBContext();

        // Checks in the database if there any suggestions to the user before create the comment
        public JsonResult checkForAprioriSuggestions(string comment)
        {
            comment = comment.Replace('!', ' ');
            comment = comment.Replace('?', ' ');
            comment = comment.Replace(',', ' ');
            comment = comment.Replace('#', ' ');
            comment = comment.Replace('*', ' ');
            comment = comment.Replace(':', ' ');
            comment = comment.Replace((char)34, ' ');   // The character "
            comment = comment.Replace('\'', ' ');
            comment = comment.Replace('.', ' ');

            char[] spr = new char[] { ' ' };
            string[] words = comment.Split(spr);
            words = words.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            var wordsFromDb = from a in db.Apriori select a; //LINQ
            string data = "";

            foreach (string oneWord in words)
            {
                var wordContained = wordsFromDb.Where(p => p.words.Contains("|" + oneWord + "|"));
                
                if (wordContained.Count() > 0)
                {
                    List<AprioriAlgorithm> aa = wordContained.ToList();
                    foreach (AprioriAlgorithm a in aa)
                        data += a.words.ToString();
                    data = data.Replace("||", "|");
                    data = data.Replace(oneWord + "|", "");
                }
            }
            if (String.IsNullOrEmpty(data))
                data = "No suggestions.";
            return Json(new { comment = data }, JsonRequestBehavior.AllowGet);
        }

        public void newDataAddedToDb()
        {
            string data = "";
            var allComments = from a in db.Comment select a; //LINQ
            List<Comment> lstComments = allComments.ToList();

            foreach (Comment c in lstComments)
                data += c.CommentText + "\n";

            var allPosts = from a in db.Post select a; //LINQ
            List<Post> lstPosts = allPosts.ToList();

            foreach (Post p in lstPosts)
                data += p.PostText + "\n";

            Run(data);
        }

        public void Run(string data)
        {
            try
            {
                // Delete current data from old Apriori run
                db.Apriori.RemoveRange(db.Apriori.Where(x => x.words.Contains("|")));

                List<string> rawItems = new List<string>();
                data = data.Replace('\n', '.');
                data = data.Replace('!', '.');
                data = data.Replace('?', '.');
                data = data.Replace('#', ' ');
                data = data.Replace('*', ' ');
                char[] spr = new char[] { '.' };
                string[] lines = data.Split(spr);
                lines = lines.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                string[][] rawTransactions = new string[lines.Length][];
                char[] spr2 = new char[] { ' ' };
                int i1 = 0, j1 = 0;
                foreach (string line in lines)
                {
                    string lowLine = line.ToLower();
                    lowLine = lowLine.Replace(',', ' ');
                    lowLine = lowLine.Replace(':', ' ');

                    string[] words = lowLine.Split(spr2);
                    words = words.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                    // remove irrelevent words
                    words = words.Where(x => x != "to").ToArray();
                    words = words.Where(x => x != "the").ToArray();
                    words = words.Where(x => x != "and").ToArray();
                    words = words.Where(x => x != "are").ToArray();
                    words = words.Where(x => x != "is").ToArray();
                    words = words.Where(x => x != "am").ToArray();
                    words = words.Where(x => x != "did").ToArray();
                    words = words.Where(x => x != "didn\'t").ToArray();


                    List<string> wordsBank = words.ToList();

                    IEnumerable<string> distinctWordsBank = wordsBank.Distinct();
                    rawTransactions[i1] = new string[wordsBank.Count()];
                    foreach (string word in wordsBank)
                    {
                        rawTransactions[i1][j1] = word;
                        j1++;
                        rawItems.Add(word);
                    }
                    j1 = 0;
                    i1++;
                }

                IEnumerable<string> distinctRawItems = rawItems.Distinct();

                string[] rawItemsArr = distinctRawItems.ToArray();
                int N = distinctRawItems.Count(); // total number of items to deal with ( [0..11] )

                List<int[]> transactions = new List<int[]>();
                List<int> oneTrans = new List<int>();
                int wordIndex = 0;
                for (int i = 0; i < rawTransactions.Length; ++i)
                {
                    for (int j = 0; j < rawTransactions[i].Length; ++j)
                    {
                        wordIndex = findInRawData(rawItemsArr, rawTransactions[i][j]);
                        oneTrans.Add(wordIndex);
                    }
                    transactions.Add(oneTrans.ToArray());
                    oneTrans = new List<int>();
                }


                // double minSupportPct = 0.03; // minimum pct of transactions for an item-set to be 'frequent'
                double minSupportPct = 0.4; // minimum pct of transactions for an item-set to be 'frequent'

                int minItemSetLength = 2;
                int maxItemSetLength = 10;

                // everything happens here
                List<ItemSet> frequentItemSets =
                  GetFrequentItemSets(N, transactions, minSupportPct, minItemSetLength, maxItemSetLength);

                // Frequent item-sets
                StringBuilder itemSet = new StringBuilder();

                for (int i = 0; i < frequentItemSets.Count; ++i)
                {
                    itemSet.Append("|");
                    for (int j = 0; j < frequentItemSets[i].data.Length; ++j)
                    {
                        int v = frequentItemSets[i].data[j];
                        itemSet.Append(rawItems[v]);
                        itemSet.Append("|");
                    }
                    AprioriAlgorithm aprioriAlgorithm = new AprioriAlgorithm() { words = itemSet.ToString() };
                    db.Apriori.Add(aprioriAlgorithm);
                    itemSet.Clear();
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                //Console.ReadLine();
            }
        }

        public static int findInRawData(string[] rawItemsArr, string v)
        {
            int i = 0;
            for (i = 0; i < rawItemsArr.Length; i++)
                if (rawItemsArr[i] == v)
                    return i;
            return -1;
        }

        static List<ItemSet> GetFrequentItemSets(int N, List<int[]> transactions, double minSupportPct, int minItemSetLength, int maxItemSetLength)
        {
            // create a List of frequent ItemSet objects that are in transactions
            // frequent means occurs in minSupportPct percent of transactions
            // N is total number of items
            // uses a variation of the Apriori algorithm

            int minSupportCount = (int)(transactions.Count * minSupportPct);

            Dictionary<int, bool> frequentDict = new Dictionary<int, bool>(); // key = int representation of an ItemSet, val = is in List of frequent ItemSet objects
            List<ItemSet> frequentList = new List<ItemSet>(); // item set objects that meet minimum count (in transactions) requirement 
            List<int> validItems = new List<int>(); // inidividual items/values at any given point in time to be used to construct new ItemSet (which may or may not meet threshhold count)

            // get counts of all individual items
            int[] counts = new int[N]; // index is the item/value, cell content is the count
            for (int i = 0; i < transactions.Count; ++i)
            {
                for (int j = 0; j < transactions[i].Length; ++j)
                {
                    int v = transactions[i][j];
                    ++counts[v];
                }
            }
            // for those items that meet support threshold, add to valid list, frequent list, frequent dict
            for (int i = 0; i < counts.Length; ++i)
            {
                if (counts[i] >= minSupportCount) // frequent item
                {
                    validItems.Add(i); // i is the item/value
                    int[] d = new int[1]; // the ItemSet ctor wants an array
                    d[0] = i;
                    ItemSet ci = new ItemSet(N, d, 1); // an ItemSet with size 1, ct 1
                    frequentList.Add(ci); // it's frequent
                    frequentDict.Add(ci.hashValue, true); // 
                } // else skip this item
            }

            bool done = false; // done if no new frequent item-sets found
            for (int k = 2; k <= maxItemSetLength && done == false; ++k) // construct all size  k = 2, 3, 4, . .  frequent item-sets
            {
                done = true; // assume no new item-sets will be created
                int numFreq = frequentList.Count; // List size modified so store first

                for (int i = 0; i < numFreq; ++i) // use existing frequent item-sets to create new freq item-sets with size+1
                {
                    if (frequentList[i].k != k - 1) continue; // only use those ItemSet objects with size 1 less than new ones being created

                    for (int j = 0; j < validItems.Count; ++j)
                    {
                        int[] newData = new int[k]; // data for a new item-set

                        for (int p = 0; p < k - 1; ++p)
                            newData[p] = frequentList[i].data[p]; // old data in

                        if (validItems[j] <= newData[k - 2]) continue; // because item-values are in order we can skip sometimes

                        newData[k - 1] = validItems[j]; // new item-value
                        ItemSet ci = new ItemSet(N, newData, -1); // ct to be determined

                        if (frequentDict.ContainsKey(ci.hashValue) == true) // this new ItemSet has already been added
                            continue;
                        int ct = CountTimesInTransactions(ci, transactions); // how many times is the new ItemSet in the transactuions?
                        if (ct >= minSupportCount) // we have a winner!
                        {
                            ci.ct = ct; // now we know the ct
                            frequentList.Add(ci);
                            frequentDict.Add(ci.hashValue, true);
                            done = false; // a new item-set was created, so we're not done
                        }
                    } // j
                } // i

                // update valid items -- quite subtle
                validItems.Clear();
                Dictionary<int, bool> validDict = new Dictionary<int, bool>(); // track new list of valid items
                for (int idx = 0; idx < frequentList.Count; ++idx)
                {
                    if (frequentList[idx].k != k) continue; // only looking at the just-created item-sets
                    for (int j = 0; j < frequentList[idx].data.Length; ++j)
                    {
                        int v = frequentList[idx].data[j]; // item
                        if (validDict.ContainsKey(v) == false)
                        {
                            //Console.WriteLine("adding " + v + " to valid items list");
                            validItems.Add(v);
                            validDict.Add(v, true);
                        }
                    }
                }
                validItems.Sort(); // keep valid item-values ordered so item-sets will always be ordered
            } // next k

            // transfer to return result, filtering by minItemSetCount
            List<ItemSet> result = new List<ItemSet>();
            for (int i = 0; i < frequentList.Count; ++i)
            {
                if (frequentList[i].k >= minItemSetLength)
                    result.Add(new ItemSet(frequentList[i].N, frequentList[i].data, frequentList[i].ct));
            }

            return result;
        }

        static int CountTimesInTransactions(ItemSet itemSet, List<int[]> transactions)
        {
            // number of times itemSet occurs in transactions
            int ct = 0;
            for (int i = 0; i < transactions.Count; ++i)
            {
                if (itemSet.IsSubsetOf(transactions[i]) == true)
                    ++ct;
            }
            return ct;
        }
    } // program

    public class ItemSet
    {
        public int N; // data items are [0..N-1]
        public int k; // number of items
        public int[] data; // ex: [0 2 5]
        public int hashValue; // "0 2 5" -> 520 (for hashing)
        public int ct; // num times this occurs in transactions

        public ItemSet(int N, int[] items, int ct)
        {
            this.N = N;
            this.k = items.Length;
            this.data = new int[this.k];
            Array.Copy(items, this.data, items.Length);
            this.hashValue = ComputeHashValue(items);
            this.ct = ct;
        }

        private static int ComputeHashValue(int[] data)
        {
            int value = 0;
            int multiplier = 1;
            for (int i = 0; i < data.Length; ++i) // actually working backward
            {
                value = value + (data[i] * multiplier);
                multiplier = multiplier * 10;
            }
            return value;
        }

        public override string ToString()
        {
            string s = "{ ";
            for (int i = 0; i < data.Length; ++i)
                s += data[i] + " ";
            return s + "}" + "   ct = " + ct; ;
        }

        public bool IsSubsetOf(int[] trans)
        {
            // 'trans' is an ordered transaction like [0 1 4 5 8]
            int foundIdx = -1;
            for (int j = 0; j < this.data.Length; ++j)
            {
                foundIdx = IndexOf(trans, this.data[j], foundIdx + 1);
                if (foundIdx == -1) return false;
            }
            return true;
        }

        private static int IndexOf(int[] array, int item, int startIdx)
        {
            for (int i = startIdx; i < array.Length; ++i)
            {
                if (i > item) return -1; // i is past where the target could possibly be
                if (array[i] == item) return i;
            }
            return -1;
        }


    } // ItemSet

}
