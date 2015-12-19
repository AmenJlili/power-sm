using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerSM;


namespace Power_SM_Unit_Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod()
        {
            Power_SM_Form form = new Power_SM_Form();
            form.ShowDialog();
            
        }
    }
}
