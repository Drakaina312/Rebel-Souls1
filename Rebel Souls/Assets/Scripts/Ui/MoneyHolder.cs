using TMPro;
using UnityEngine;
using Zenject;

public class MoneyHolder : MonoBehaviour
{
    private MasterSave _masterSave;

    [SerializeField] private TextMeshProUGUI _moneyText;

    [Inject]
    public void Construct(MasterSave masterSave)
    {
        _masterSave = masterSave;
        _masterSave.OnProfileChoosed += FindMoneyOnProfileChoose;
    }

    public void FindMoneyOnProfileChoose()
    {
        _moneyText.text = _masterSave.CurrentProfile.Money.ToString();
        _masterSave.CurrentProfile.OnMoneyChange += x => _moneyText.text = x.ToString();
    }



}
