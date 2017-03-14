using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerModeInput : MonoBehaviour
{
	[SerializeField]
	private PlayerModeController _playerModeController;
	

	private void Start()
	{
		SetPorridgeModeInputSubscription();
		SetSheepModeInputSubscription();
	}

	private System.IDisposable SetPorridgeModeInputSubscription()
	{
		return
		this.UpdateAsObservable()
			.Where(_ => Input.GetAxis("Vertical") < 0)
			.Subscribe(_ => _playerModeController.SetPorridgeMode())
			.AddTo(this);
	}

	private System.IDisposable SetSheepModeInputSubscription()
	{
		return
		this.UpdateAsObservable()
			.Where(_ => Input.GetAxis("Vertical") > 0)
			.Subscribe(_ => _playerModeController.SetSheepMode())
			.AddTo(this);
	}

}
