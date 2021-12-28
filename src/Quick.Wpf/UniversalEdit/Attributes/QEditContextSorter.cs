using System.Collections.Generic;
using System.Linq;

namespace Quick
{
    public static class QEditContextSorter
    {
        private const int DefaultGroupOrder = 9999;
        private static int GetGroupOrder(int? groupOrder)
        {
            return groupOrder == null ? DefaultGroupOrder : groupOrder.Value;
        }
        public static List<EditContextGroup> SortGroup(IEnumerable<QEditContext> contexts)
        {
            List<QEditContext> ctxList = contexts.ToList();
            Dictionary<QEditContext, int> ctxOrderDict = new Dictionary<QEditContext, int>();
            Dictionary<string, int> groupOrderDict = new Dictionary<string, int>();

            if (!contexts.Any(p => p.GetAttr().GroupHeader != null))
            {
                EditContextGroup ret = new EditContextGroup();
                ret.AddRange(contexts);
            }

            //原始组顺序
            int i = 1;
            foreach (var ctx in ctxList.Where(p => p.GetAttr().GroupHeader != null))
            {
                string header = ctx.GetAttr().GroupHeader;
                if (!groupOrderDict.ContainsKey(header))
                {
                    groupOrderDict.Add(header, i);
                    i++;
                }
            }

            //原始元素顺序
            for (int j = 0; j < ctxList.Count; j++)
            {
                ctxOrderDict.Add(ctxList[j], j);
            }

            List<EditContextGroup> results = new List<EditContextGroup>();
            //取出需要分组的
            var groups = contexts.Where(p => p.GetAttr().GroupHeader != null)
                                 .GroupBy(p => new { GroupHeader = p.GetAttr().GroupHeader, GroupOrder = GetGroupOrder(p.GetAttr().GroupOrder) })
                                 .OrderBy(p => p.Key.GroupOrder)
                                 .ThenBy(p => groupOrderDict[p.Key.GroupHeader]);

            foreach (var group in groups)
            {
                EditContextGroup ctxGroup = new EditContextGroup();
                ctxGroup.GroupHeader = group.Key.GroupHeader;
                var editCtxList = group.OrderBy(p => p.GetAttr().Order).ThenBy(p => ctxOrderDict[p]);
                ctxGroup.AddRange(editCtxList);

                results.Add(ctxGroup);
            }

            var noneGroupCtxList = contexts.Where(p => p.GetAttr().GroupHeader == null).OrderBy(p => p.GetAttr().Order).ThenBy(p => ctxOrderDict[p]);
            //对于没有分组的，找原始序号小于自己的分组里最大的一个加入
            List<QEditContext> otherList = new List<QEditContext>();
            foreach (var otherCtx in noneGroupCtxList)
            {
                int myOrder = ctxOrderDict[otherCtx];
                bool toGroup = false;
                foreach (var result in results.Reverse<EditContextGroup>())
                {
                    int minOrder = result.Min(p => ctxOrderDict[p]);
                    if (myOrder > minOrder)
                    {
                        result.Add(otherCtx);
                        toGroup = true;
                        break;
                    }
                }

                if (!toGroup)
                    otherList.Add(otherCtx);
            }

            //没有组的，单独构建一个无名称的组
            
            if(otherList.Any())
            {
                EditContextGroup otherGroup = new EditContextGroup();
                otherGroup.AddRange(otherList.OrderBy(p => ctxOrderDict[p]));
                results.Add(otherGroup);
            }
            
            return results;
        }
    }
    public class EditContextGroup : List<QEditContext>
    {
        public string GroupHeader { get; set; }
    }
}
