using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIScript : MonoBehaviour
{
    private GameObject[] upgradeSlots;

    private GameObject player;
    private PlayerController playerCon;
    private UpgradeScriptableObject[] upgradeArray;

    [SerializeField] private GameObject UI;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerCon = player.GetComponent<PlayerController>();
        upgradeArray = playerCon.upgradeSelection;

        upgradeSlots = GameObject.FindGameObjectsWithTag("UpgradeContainer");


        int arraySlot = 0;
        foreach (var slot in upgradeSlots)
        {
            UpgradeScriptableObject upgrade = upgradeArray[arraySlot];
            FillContainer(slot, upgrade);

            if (arraySlot < upgradeArray.Length -1)
            {
                arraySlot++;
            }
        }
    }

    private void FillContainer(GameObject slot, UpgradeScriptableObject up)
    {
        TextMeshProUGUI nameDisplay = slot.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>();
        nameDisplay.text = up.upgradeName;

        TextMeshProUGUI descriptionDisplay = slot.transform.Find("Description").gameObject.GetComponent<TextMeshProUGUI>();
        descriptionDisplay.text = up.description;

        RawImage upgradeColor = slot.transform.Find("Icon").gameObject.GetComponent<RawImage>();
        upgradeColor.color = up.color;

        Button pickButton = slot.transform.Find("Pick").gameObject.GetComponent<Button>();
        pickButton.onClick.AddListener(delegate { ApplyUpgrade(up); });
    }

    public void ApplyUpgrade(UpgradeScriptableObject up)
    {
        playerCon.AddUpgrade(up);

        UIScript UserIntContr = GetUIScript();
        //UserIntContr.ResetHP(playerCon.currentHealth, playerCon.currentMaxHealth);

        Destroy(gameObject);
        //StartCoroutine(GameUnpauseDelay());
        Time.timeScale = 1;

    }

    private UIScript GetUIScript()
    {
        return UI.GetComponent<UIScript>();
    }

    IEnumerator GameUnpauseDelay()
    {
        yield return new WaitForSeconds(1.5f);
    }
}
