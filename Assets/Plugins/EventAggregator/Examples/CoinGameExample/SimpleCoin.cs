using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UniRx;

public class SimpleCoin : MonoBehaviour, IPointerClickHandler {
    [SerializeField]
    private CoinType coinType;

    // Update is called once per frame
    void Update() {
        this.transform.Rotate(Vector3.forward, Time.deltaTime * 100);
    }

    public void OnPointerClick(PointerEventData eventData) {
        this.Publish(new CoinPickedUpEvent() { CoinType = coinType });
    }
}


public class CoinPickedUpEvent {
    public CoinType CoinType { get; set; }
}

public enum CoinType {
    Copper,
    Silver,
    Gold
}