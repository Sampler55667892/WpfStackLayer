using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfStackLayer;

namespace WpfStackLayerUnitTest
{
    [TestClass]
    public class VisualTreeExtensionsTest
    {
        [TestMethod]
        public void FindFirstChild_NoPredicate()
        {
            var item1 = new Grid();
            var item2 = new TextBlock();
            item1.Children.Add( item2 );

            var found1 = item1.FindFirstChild<TextBlock>();
            found1.Is( item2 );
            var found2 = item1.FindFirstChild<Button>();
            found2.IsNull();
        }

        [TestMethod]
        public void FindFirstChild_TargetName_Success()
        {
            var name = "test";

            var item1 = new Grid();
            var item2 = new TextBlock { Name = name };
            var item3 = new TextBlock();

            item1.Children.Add( item2 );
            item1.Children.Add( item3 );

            var found = item1.FindFirstChild( name );
            found.Is( item2 );
        }

        [TestMethod]
        public void FindFirstChild_TargetName_Fail()
        {
            var name1 = "test1";
            var name2 = "test2";

            var item1 = new Grid();
            var item2 = new TextBlock { Name = name1 };
            var item3 = new TextBlock();

            item1.Children.Add( item2 );
            item1.Children.Add( item3 );

            var found = item1.FindFirstChild( name2 );
            found.IsNull();
        }

        [TestMethod]
        public void IsParentOf()
        {
            var item1 = new Grid();
            var item2 = new TextBlock();
            item1.Children.Add( item2 );

            item1.IsParentOf( item2 ).IsTrue();
            item2.IsParentOf( item1 ).IsFalse();
        }
    }
}
