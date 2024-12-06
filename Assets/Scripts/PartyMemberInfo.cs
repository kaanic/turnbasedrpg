using UnityEngine;

[CreateAssetMenu(menuName = "New Party Member")]
public class PartyMemberInfo : ScriptableObject
{
   public string MemberName;
   public int StartingLevel;
   public int BaseHealth;
   public int BaseStr;
   public int BaseInitiative;
   public GameObject MemberBattleVisualPrefab; // overworld prefab
   public GameObject MemberOverworldVisualPrefab; // battle scene prefab
}
