using UnityEngine;
using UniRx;

public class PlayerModeController : ReactiveController<PlayerModeControllerData>
{
	private void Start()
	{
		SetPorridgeModeSubscription();
	}

	public void SetPorridgeMode()
	{
		Data.IsPorridge = true;
	}

	public void SetSheepMode()
	{
		Data.IsPorridge = false;
	}

	private System.IDisposable SetPorridgeModeSubscription()
	{
		return
		Data.IsPorridgeProperty
			.AsObservable()
			.Subscribe(SetPorridgeMode)
			.AddTo(this);
	}

	private void SetPorridgeMode(bool enabled)
	{
		Data.PorridgeBody.SetActive(enabled);
		Data.PorridgeCollider.enabled = enabled;
		Data.SheepBody.SetActive(!enabled);
		Data.SheepCollider.enabled = !enabled;
	}
}
