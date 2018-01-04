using Intacct.SDK.Functions;

namespace Intacct.Examples
{
    public abstract class AbstractTestObject : AbstractFunction
    {
        protected const string IntegrationName = "test_object";
        
        public int Id;

        public string Name;
        
        protected AbstractTestObject(string controlId = null) : base(controlId)
        {
        }
    }
}