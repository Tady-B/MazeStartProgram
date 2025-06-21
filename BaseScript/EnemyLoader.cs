using System.Collections.Generic;
using UnityEngine;

public class EnemyLoader : MonoBehaviour
{
    private static Dictionary<string, EnemyConfig> configCache =
        new Dictionary<string, EnemyConfig>();

    public static EnemyAttributes LoadAttributes(string enemyType)
    {
        // ºÏ≤Èª∫¥Ê
        if (configCache.ContainsKey(enemyType))
            return configCache[enemyType].attributes;

        // ¥”Resourcesº”‘ÿ
        EnemyConfig config = Resources.Load<EnemyConfig>($"EnemyConfigs/{enemyType}");
        if (config != null)
        {
            configCache[enemyType] = config;
            return config.attributes;
        }

        // ∑µªÿƒ¨»œ≈‰÷√
        Debug.LogWarning($"No config found for {enemyType}, using default");
        return new EnemyAttributes { enemyType = "Default" };
    }

    public static RuntimeAnimatorController LoadAnimatorController(string enemyType)
    {
        if (configCache.ContainsKey(enemyType))
            return configCache[enemyType].animatorController;

        // º”‘ÿ≈‰÷√
        LoadAttributes(enemyType);

        if (configCache.ContainsKey(enemyType))
            return configCache[enemyType].animatorController;

        return null;
    }
}
