using System;
using System.Collections.Generic;
using System.Text;

namespace TTOptimize.Core.GameAbstractions
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DependsOnAttribute : Attribute
    {
        public string PropertyName { get; }
        public object ExpectedValue { get; }

        public DependsOnAttribute(string propertyName, object expectedValue)
        {
            PropertyName = propertyName;
            ExpectedValue = expectedValue;
        }
    }
}
