using System.Collections.Generic;
using UnityEngine;

public class JoinableCharacter : MonoBehaviour
{
    public PartyMemberInfo memberToJoin;
    [SerializeField] private GameObject interactPrompt;
    
    void Start()
    {
        CheckIfJoined();
    }

    public void ShowInteractPrompt(bool showPrompt)
    {
        if (showPrompt)
        {
            interactPrompt.SetActive(true);
        }
        else
        {
            interactPrompt.SetActive(false);
        }
    }

    public void CheckIfJoined()
    {
        List<PartyMember> currentParty = GameObject.FindFirstObjectByType<PartyManager>().GetCurrentParty();

        for (int i = 0; i < currentParty.Count; i++)
        {
            if (currentParty[i].MemberName == memberToJoin.MemberName)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
