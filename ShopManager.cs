using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] Color InactiveColor;
    [SerializeField] Color ActiveColor;
    [SerializeField] List<Image> UpgradeImages = new List<Image>();
    [SerializeField] List<int> Costs = new List<int>();
    [SerializeField] int unlockedImages;
    [SerializeField] Animator UpgradeButtonAnim;
    [SerializeField] GameObject MoreCoinsButton;
    [SerializeField] TextMeshProUGUI CostText;
    public ItemType itemType;
    private void OnEnable()
    {
        if (itemType == ItemType.Jetpack)
        {
            unlockedImages = PlayerDataStorer.JetpackUpgradeLevel;
        }
        else if (itemType == ItemType.Grapple)
        {
            unlockedImages = PlayerDataStorer.GrappleUpgradeLevel;
        }
        
        for (int i = unlockedImages; i < UpgradeImages.Count; i++)
        {
            UpgradeImages[i].color = InactiveColor;
            
        }
        CostText.text = Costs[unlockedImages].ToString();
    }
    public void CheckIfCoins()
    {
        if (unlockedImages >= UpgradeImages.Count)
            return;
        if (PlayerDataStorer.totalcoins >= Costs[unlockedImages] && unlockedImages < 5)
        {
            PlayerDataStorer.totalcoins -= Costs[unlockedImages];
            UpdateImages();
        }
        else
        {
            UpgradeButtonAnim.SetTrigger("NoMoney");
            MoreCoinsButton.SetActive(true);
        }
    }
    public void UpdateImages()
    {
        unlockedImages++;
        for (int i = 0; i < unlockedImages; i++)
        {
            UpgradeImages[i].color = ActiveColor;
        }
        CostText.text = Costs[unlockedImages].ToString();
        if (itemType==ItemType.Jetpack)
        PlayerDataStorer.JetpackUpgradeLevel = unlockedImages;
        if(itemType==ItemType.Grapple)
        {
            PlayerDataStorer.GrappleUpgradeLevel = unlockedImages;
        }
        PlayerDataStorer.SaveProgress();
        
    }
}
