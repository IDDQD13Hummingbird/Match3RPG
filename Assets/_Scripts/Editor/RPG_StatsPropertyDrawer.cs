using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(RPG_stats))]
public class RPG_StatsPropertyDrawer : PropertyDrawer
{    private void CalculateEnemyStats(SerializedProperty levelProp, SerializedProperty maxHealthProp, 
        SerializedProperty damageProp, SerializedProperty maxSpeedProp, 
        SerializedProperty currentHealthProp, SerializedProperty currentSpeedProp)
    {
        if (levelProp == null || levelProp.intValue <= 0) return;
        
        int level = levelProp.intValue;
        float health = level * 15f;
        float damage = level * 5f;
        int speed = level * 3;
        
        // Apply calculated values
        if (maxHealthProp != null)
            maxHealthProp.floatValue = health;
        if (damageProp != null)
            damageProp.floatValue = damage;
        if (maxSpeedProp != null)
            maxSpeedProp.intValue = speed;
        
        // Sync current values
        if (currentHealthProp != null)
            currentHealthProp.floatValue = health;
        if (currentSpeedProp != null)
            currentSpeedProp.intValue = speed;
    }
    
    private void CalculateHeroStats(SerializedProperty levelProp, SerializedProperty heroTypeProp, SerializedProperty maxHealthProp, 
        SerializedProperty damageProp, SerializedProperty maxSpeedProp, 
        SerializedProperty currentHealthProp, SerializedProperty currentSpeedProp)
    {
        if (levelProp == null || levelProp.intValue <= 0 || heroTypeProp == null) return;
        
        int level = levelProp.intValue;
        float health, damage;
        int speed;        // Calculate stats based on hero type (enum order: 0=Necromancer, 1=Barbarian, 2=Poisoner, 3=Healer)
        switch (heroTypeProp.enumValueIndex)
        {
            case 0: // Necromancer
                health = level * 12f;
                damage = level * 6f;
                speed = level * 4;
                break;
            case 1: // Barbarian
                health = level * 14f;
                damage = level * 5f;
                speed = level * 5;
                break;
            case 2: // Poisoner
                health = level * 11f;
                damage = level * 3f;
                speed = level * 7;
                break;
            case 3: // Healer
                health = level * 10f;
                damage = level * 8f;
                speed = level * 3;
                break;
            default:
                return; // Unknown hero type
        }
        
        // Apply calculated values
        if (maxHealthProp != null)
            maxHealthProp.floatValue = health;
        if (damageProp != null)
            damageProp.floatValue = damage;
        if (maxSpeedProp != null)
            maxSpeedProp.intValue = speed;
        
        // Sync current values
        if (currentHealthProp != null)
            currentHealthProp.floatValue = health;
        if (currentSpeedProp != null)
            currentSpeedProp.intValue = speed;
    }
    
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
            var teamProp = property.FindPropertyRelative("team");
            var heroTypeProp = property.FindPropertyRelative("heroType");
            var aliveProp = property.FindPropertyRelative("alive");
            var currentHealthProp = property.FindPropertyRelative("currentHealth");
            var currentSpeedProp = property.FindPropertyRelative("currentSpeed");
            
            // Auto-initialize alive to true for new characters
            if (aliveProp != null && !aliveProp.boolValue)
            {
                aliveProp.boolValue = true;
            }if (characterNameProp != null)
            {
                // Dynamic label based on team
                string nameLabel = "Character Name";
                if (teamProp != null)
                {
                    nameLabel = teamProp.enumValueIndex == 0 ? "Hero Name" : "Enemy Name";
                }
                EditorGUI.BeginChangeCheck();
                EditorGUI.PropertyField(new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight), characterNameProp, new GUIContent(nameLabel));
                if (EditorGUI.EndChangeCheck() && !string.IsNullOrEmpty(characterNameProp.stringValue))
                {
                    // When character name is set, ensure they are alive
                    if (aliveProp != null)
                        aliveProp.boolValue = true;
                }
                yOffset += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }              if (levelProp != null)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.PropertyField(new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight), levelProp);
                if (EditorGUI.EndChangeCheck())
                {                    // When level is set to a positive value, ensure character is alive
                    if (levelProp.intValue > 0 && aliveProp != null)
                        aliveProp.boolValue = true;
                      // Check if team is Enemy and auto-calculate stats
                    if (teamProp != null && teamProp.enumValueIndex == 1) // Enemy team
                    {
                        CalculateEnemyStats(levelProp, maxHealthProp, damageProp, maxSpeedProp, currentHealthProp, currentSpeedProp);
                    }
                    // Check if team is Hero and auto-calculate stats
                    else if (teamProp != null && teamProp.enumValueIndex == 0) // Hero team
                    {
                        CalculateHeroStats(levelProp, heroTypeProp, maxHealthProp, damageProp, maxSpeedProp, currentHealthProp, currentSpeedProp);
                    }
                    
                    // Sync current values with max values for all teams
                    if (currentHealthProp != null && maxHealthProp != null)
                        currentHealthProp.floatValue = maxHealthProp.floatValue;
                    if (currentSpeedProp != null && maxSpeedProp != null)
                        currentSpeedProp.intValue = maxSpeedProp.intValue;
                }
                yOffset += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }if (maxHealthProp != null)
            {
                // Make read-only if team is Enemy
                bool isEnemy = teamProp != null && teamProp.enumValueIndex == 1;
                if (isEnemy) EditorGUI.BeginDisabledGroup(true);
                
                EditorGUI.BeginChangeCheck();
                EditorGUI.PropertyField(new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight), maxHealthProp);                if (EditorGUI.EndChangeCheck() && currentHealthProp != null)
                {
                    // Sync currentHealth with maxHealth when maxHealth changes
                    currentHealthProp.floatValue = maxHealthProp.floatValue;
                    // Ensure character is alive when health is set to a positive value
                    if (maxHealthProp.floatValue > 0 && aliveProp != null)
                        aliveProp.boolValue = true;
                }
                
                if (isEnemy) EditorGUI.EndDisabledGroup();
                yOffset += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            
            if (damageProp != null)
            {
                // Make read-only if team is Enemy
                bool isEnemy = teamProp != null && teamProp.enumValueIndex == 1;
                if (isEnemy) EditorGUI.BeginDisabledGroup(true);
                
                EditorGUI.PropertyField(new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight), damageProp);
                
                if (isEnemy) EditorGUI.EndDisabledGroup();
                yOffset += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
              if (maxSpeedProp != null)
            {
                // Make read-only if team is Enemy
                bool isEnemy = teamProp != null && teamProp.enumValueIndex == 1;
                if (isEnemy) EditorGUI.BeginDisabledGroup(true);
                
                EditorGUI.BeginChangeCheck();
                EditorGUI.PropertyField(new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight), maxSpeedProp);
                if (EditorGUI.EndChangeCheck() && currentSpeedProp != null)
                {
                    // Sync currentSpeed with maxSpeed when maxSpeed changes
                    currentSpeedProp.intValue = maxSpeedProp.intValue;
                }
                  if (isEnemy) EditorGUI.EndDisabledGroup();
                yOffset += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            
            if (teamProp != null)
            {                EditorGUI.BeginChangeCheck();
                EditorGUI.PropertyField(new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight), teamProp);                if (EditorGUI.EndChangeCheck())
                {
                    // When team is set, ensure character is alive
                    if (aliveProp != null)
                        aliveProp.boolValue = true;
                      if (teamProp.enumValueIndex == 1) // Changed to Enemy
                    {
                        // Auto-calculate enemy stats when team is changed to Enemy
                        if (levelProp != null && levelProp.intValue > 0)
                        {
                            CalculateEnemyStats(levelProp, maxHealthProp, damageProp, maxSpeedProp, currentHealthProp, currentSpeedProp);
                        }
                    }
                    else if (teamProp.enumValueIndex == 0) // Changed to Hero
                    {
                        // Auto-calculate hero stats when team is changed to Hero
                        if (levelProp != null && levelProp.intValue > 0)
                        {
                            CalculateHeroStats(levelProp, heroTypeProp, maxHealthProp, damageProp, maxSpeedProp, currentHealthProp, currentSpeedProp);
                        }
                    }
                }
                yOffset += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                  // Only show heroType if team is Hero (enum value 0)
                if (heroTypeProp != null && teamProp.enumValueIndex == 0)
                {
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.PropertyField(new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight), heroTypeProp);
                    if (EditorGUI.EndChangeCheck())
                    {
                        // Auto-calculate hero stats when hero type is changed
                        if (levelProp != null && levelProp.intValue > 0)
                        {
                            CalculateHeroStats(levelProp, heroTypeProp, maxHealthProp, damageProp, maxSpeedProp, currentHealthProp, currentSpeedProp);
                        }
                    }
                    yOffset += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }
            }
            
            if (aliveProp != null)
            {
                EditorGUI.BeginDisabledGroup(true); // Make read-only
                EditorGUI.PropertyField(new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight), aliveProp);
                EditorGUI.EndDisabledGroup();
                yOffset += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
              if (currentHealthProp != null)
            {
                EditorGUI.BeginDisabledGroup(true); // Make read-only
                EditorGUI.PropertyField(new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight), currentHealthProp);
                EditorGUI.EndDisabledGroup();
                yOffset += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            
            if (currentSpeedProp != null)
            {
                EditorGUI.BeginDisabledGroup(true); // Make read-only
                EditorGUI.PropertyField(new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight), currentSpeedProp);
                EditorGUI.EndDisabledGroup();
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
