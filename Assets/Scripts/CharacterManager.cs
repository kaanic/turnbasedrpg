using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using Unity.Mathematics;
using UnityEngine.Rendering.Universal;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject joinPopup;
    [SerializeField] private TextMeshProUGUI joinPopupText;

    private bool infrontOfPartyMember;
    private GameObject joinableMember;
    private PlayerControls playerControls;
    private List<GameObject> overworldCharacters = new List<GameObject>();

    private const string PARTY_JOINED_MESSAGE = " joined the party!";
    private const string NPC_JOINABLE_TAG = "NPCJoinable";

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    void Start()
    {
        playerControls.Player.Interact.performed += _ => Interact(); // keybind assignment to the function
        SpawnOverworldMembers();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Interact()
    {
        if (infrontOfPartyMember == true && joinableMember != null)
        {
            MemberJoined(joinableMember.GetComponent<JoinableCharacter>().memberToJoin);
            infrontOfPartyMember = false;
            joinableMember = null;
        }
    }

    private void MemberJoined(PartyMemberInfo partyMember)
    {
        GameObject.FindFirstObjectByType<PartyManager>().AddMemberToPartyByName(partyMember.MemberName);
        joinableMember.GetComponent<JoinableCharacter>().CheckIfJoined();
        joinPopup.SetActive(true);
        joinPopupText.text = partyMember.MemberName + PARTY_JOINED_MESSAGE;
        SpawnOverworldMembers();
    }

    private void SpawnOverworldMembers()
    {
        for (int i = 0; i < overworldCharacters.Count; i++)
        {
            Destroy(overworldCharacters[i]);
        }
        overworldCharacters.Clear();

        List<PartyMember> currentParty = GameObject.FindFirstObjectByType<PartyManager>().GetCurrentParty();

        for (int i = 0; i < currentParty.Count; i++)
        {
            if (i == 0) // first member = player, else follower
            {
                GameObject player = gameObject;

                GameObject playerVisual = Instantiate(currentParty[i].MemberOverworldVisualPrefab, player.transform.position, Quaternion.identity);

                playerVisual.transform.SetParent(player.transform);

                player.GetComponent<PlayerController>().SetOverworldVisuals(playerVisual.GetComponent<Animator>(), playerVisual.GetComponent<SpriteRenderer>());
                playerVisual.GetComponent<MemberFollowAI>().enabled = false;
                overworldCharacters.Add(playerVisual);                
            }
            else
            {
                Vector3 positionToSpawn = transform.position;
                positionToSpawn.x -= 1;

                GameObject tempFollower = Instantiate(currentParty[i].MemberOverworldVisualPrefab, positionToSpawn, Quaternion.identity);

                tempFollower.GetComponent<MemberFollowAI>().SetFollowDistance(i);
                overworldCharacters.Add(tempFollower);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == NPC_JOINABLE_TAG)
        {
            infrontOfPartyMember = true;
            joinableMember = other.gameObject;
            joinableMember.GetComponent<JoinableCharacter>().ShowInteractPrompt(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == NPC_JOINABLE_TAG)
        {
            infrontOfPartyMember = false;
            joinableMember.GetComponent<JoinableCharacter>().ShowInteractPrompt(false);
            joinableMember = null;
        }
    }
}
