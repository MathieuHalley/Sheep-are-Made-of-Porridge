using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class CoinCounter : MonoBehaviour {
    private IntReactiveProperty coinAmount = new IntReactiveProperty(0);
    [SerializeField] private Text coinAmountText;
    [SerializeField] private List<Text> coinNotificationPool;
    private int poolIndex = 0;

    // Use this for initialization
	void Start () {
	    this.OnEvent<CoinPickedUpEvent>().TakeUntilDestroy(this)
            .Subscribe(OnCoinPickup);

        // Bind coinAmount to coinAmountText.text
	    if (coinAmountText != null)
	        coinAmount.TakeUntilDestroy(this)
	            .Subscribe(i => coinAmountText.text = i.ToString());
	}

    private void OnCoinPickup(CoinPickedUpEvent evt) {
        switch (evt.CoinType) {
            case CoinType.Copper:
                coinAmount.Value += 10;
                DoCoinNotification(evt.CoinType, 10);
                break;
            case CoinType.Silver:
                coinAmount.Value += 25;
                DoCoinNotification(evt.CoinType, 25);
                break;
            case CoinType.Gold:
                coinAmount.Value += 50;
                DoCoinNotification(evt.CoinType, 50);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void DoCoinNotification(CoinType type, int amount) {
        var notifier = coinNotificationPool[poolIndex];

        notifier.text = string.Format("{0} +{1}", type.ToString(), amount);
        notifier.GetComponent<Animator>().SetTrigger("notify");

        if (++poolIndex >= coinNotificationPool.Count)
            poolIndex = 0;
    }

	/* Who needs Update()? :)
	void Update () {
	
	}
    */
}
