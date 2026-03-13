using System;
using UnityEngine;

namespace NTPackage.Functions
{
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class NTShowIfAttribute : Attribute
{
    public string ConditionFieldNameBool;
    public bool ExpectedValueBool;

    public string ConditionFieldNameInt;
    public int ExpectedValueInt;

    public NTShowIfAttribute(string conditionFieldNameBool, bool expectedValue = true)
    {
        ConditionFieldNameBool = conditionFieldNameBool;
        ExpectedValueBool = expectedValue;
    }

    public NTShowIfAttribute(string conditionFieldNameInt, int expectedValueInt)
    {
        ConditionFieldNameInt = conditionFieldNameInt;
        ExpectedValueInt = expectedValueInt;
    }
}
}