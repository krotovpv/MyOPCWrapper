using OPCAutomation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOPCWrapperLib
{
    public class MyOPCGroup
    {
        private OPCGroup group;
        private List<MyOPCItem> MyOPCTags = new List<MyOPCItem>();
        public bool IsActive
        {
            get => group.IsActive;
            set => group.IsActive = value;
        }
        public bool IsSubscribed
        {
            get => group.IsSubscribed;
            set => group.IsSubscribed = value;
        }
        public int UpdateRate
        {
            get => group.UpdateRate;
            set => group.UpdateRate = value;
        }
        public MyOPCGroup(OPCGroup Group, params string[] ItemIDs)
        {
            group = Group;
            for (int i = 0; i < ItemIDs.Length; i++)
            {
                MyOPCTags.Add(new MyOPCItem(Group.OPCItems.AddItem(ItemIDs[i], MyOPCTags.Count)));
            }
            group.DataChange += Group_DataChange;
        }

        private void Group_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            for (int i = 1; i < NumItems + 1; i++)
            {
                MyOPCTags[(int)ClientHandles.GetValue(i)].Value = ItemValues.GetValue(i).ToString();
            }
        }

        public MyOPCItem GetMyOPCTag(string Name)
        {
            return MyOPCTags.First(x => x.ItemID == Name);
        }

        //public void AddMyOPCTag(string ItemID, string Comment = "")
        //{
        //    MyOPCTags.Add(new MyOPCTag(Group.OPCItems.AddItem(ItemID, MyOPCTags.Count), Comment));
        //}

        //public void AddMyOPCTags(string[] ItemID)
        //{
        //    for (int i = 0; i < ItemID.Length; i++)
        //    {
        //        MyOPCTags.Add(new MyOPCTag(Group.OPCItems.AddItem(ItemID[i], MyOPCTags.Count)));
        //    }
        //}

        //public void AddMyOPCTags(params(string ItemID, string Comment)[] Tags)
        //{
        //    for (int i = 0; i < Tags.Length; i++)
        //    {
        //        MyOPCTags.Add(new MyOPCTag(Group.OPCItems.AddItem(Tags[i].ItemID, MyOPCTags.Count), Tags[i].Comment));
        //    }
        //}
    }
}
