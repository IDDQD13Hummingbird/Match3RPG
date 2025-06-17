using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(RPG_stats))]
public class RPG_StatsPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        // Draw a foldout for the RPG_stats
        property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), property.isExpanded, label);
        
        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;
            float yOffset = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            
            // Draw all properties
            var characterNameProp = property.FindPropertyRelative("characterName");
            var maxHealthProp = property.FindPropertyRelative("maxHealth");
            var damageProp = property.FindPropertyRelative("damage");
            var maxSpeedProp = property.FindPropertyRelative("maxSpeed");
            var levelProp = property.FindPropertyRelative("level");
            var aliveProp = property.FindPropertyRelative("alive");
            var teamProp = property.FindPropertyRelative("team");
            var heroTypeProp = property.FindPropertyRelative("heroType");
            var currentHealthProp = property.FindPropertyRelative("currentHealth");
            var currentSpeedProp = property.FindPropertyRelative("currentSpeed");
            
            if (characterNameProp != null)
            {
                EditorGUI.PropertyField(new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight), characterNameProp);
                yOffset += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            
            if (maxHealthProp != null)
            {
                EditorGUI.PropertyField(new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight), maxHealthProp);
                yOffset += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            
            if (damageProp != null)
            {
                EditorGUI.PropertyField(new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight), damageProp);
                yOffset += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            
            if (maxSpeedProp != null)
            {
                EditorGUI.PropertyField(new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight), maxSpeedProp);
                yOffset += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            
            if (levelProp != null)
            {
                EditorGUI.PropertyField(new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight), levelProp);
                yOffset += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            
            if (aliveProp != null)
            {
                EditorGUI.PropertyField(new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight), aliveProp);
                yOffset += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            
            if (teamProp != null)
            {
                EditorGUI.PropertyField(new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight), teamProp);
                yOffset += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                
                // Only show heroType if team is Hero (enum value 0)
                if (heroTypeProp != null && teamProp.enumValueIndex == 0)
                {
                    EditorGUI.PropertyField(new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight), heroTypeProp);
                    yOffset += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }
            }
            
            if (currentHealthProp != null)
            {
                EditorGUI.PropertyField(new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight), currentHealthProp);
                yOffset += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            
            if (currentSpeedProp != null)
            {
                EditorGUI.PropertyField(new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight), currentSpeedProp);
            }
            
            EditorGUI.indentLevel--;
        }
        
        EditorGUI.EndProperty();
    }
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded)
        {
            return EditorGUIUtility.singleLineHeight;
        }
        
        int fieldCount = 9; // Base number of fields
        
        var teamProp = property.FindPropertyRelative("team");
        if (teamProp != null && teamProp.enumValueIndex == 0) // Hero team
        {
            fieldCount += 1; // Add heroType field
        }
        
        return EditorGUIUtility.singleLineHeight * (fieldCount + 1) + EditorGUIUtility.standardVerticalSpacing * fieldCount;
    }
}
