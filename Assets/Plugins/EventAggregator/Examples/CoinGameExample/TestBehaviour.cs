using UniRx;
using UnityEngine;

public class TestBehaviour : MonoBehaviour {

    [ContextMenu("Test Copper CoinPickedUpEvent")]
    private void TestCopper() {
        this.Publish(new CoinPickedUpEvent() { CoinType = CoinType.Copper });
    }

    [ContextMenu("Test Silver CoinPickedUpEvent")]
    private void TestSilver() {
        this.Publish(new CoinPickedUpEvent() { CoinType =  CoinType.Silver});
    }

    [ContextMenu("Test Gold CoinPickedUpEvent")]
    private void TestGold() {
        this.Publish(new CoinPickedUpEvent() { CoinType = CoinType.Gold});
    }

    void Start() {
        // Example 1: Subscribe to all notifications
        this.OnEvent<CoinPickedUpEvent>()
            .TakeUntilDestroy(this)
            .Subscribe(OnCoinPickedUp);

        // Example 2: Subscribe to be notified ONLY when Gold coins are picked up
        this.OnEvent<CoinPickedUpEvent>()
            .TakeUntilDestroy(this)
            .Where(e => e.CoinType == CoinType.Gold)
            .Subscribe(_ => Debug.Log("Picked up gold coin!"));
    }

    private void OnCoinPickedUp(CoinPickedUpEvent evt) {
        Debug.LogFormat("{0} Coin picked up!", evt.CoinType.ToString());
    }
}