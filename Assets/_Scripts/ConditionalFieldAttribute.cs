using UnityEngine;

/// <summary>
/// Attribute to conditionally show/hide fields in the inspector based on another field's value
/// </summary>
public class ConditionalFieldAttribute : PropertyAttribute
{
    public string ConditionalSourceField { get; }
    public object CompareValue { get; }
    public bool HideWhenEqual { get; }

    public ConditionalFieldAttribute(string conditionalSourceField, object compareValue, bool hideWhenEqual = false)
    {
        ConditionalSourceField = conditionalSourceField;
        CompareValue = compareValue;
        HideWhenEqual = hideWhenEqual;
    }
}
