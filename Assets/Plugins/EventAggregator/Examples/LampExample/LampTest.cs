using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LampTest : MonoBehaviour, IPointerClickHandler {
    [SerializeField] private Light lampLight;
    [SerializeField] private Button lampButton;
    [SerializeField, Header("OffColors")]
    private ColorBlock offColors;
    [SerializeField, Header("OnColors")]
    private ColorBlock onColors;

    private Text lampButtonText;

    [ContextMenu("Toggle Light")]
    private void TestToggleLight() {
        // Can publish event from anywhere, but do it here for now for simplicity
        this.Publish(new LampToggleEvent());
    }

	// Use this for initialization
	void Start () {
        // Subscribe to the event
	    this.OnEvent<LampToggleEvent>().TakeUntilDestroy(this)
	        .Subscribe(OnLampToggled);

        // Extra: Push out a LampToggleEvent when lampButton is pressed
	    lampButton.OnClickAsObservable().TakeUntilDestroy(this)
            .Subscribe(_ => this.Publish(new LampToggleEvent()))
	        .AddTo(lampButton); // Dispose if lampButton gets destroyed

        // Grab the Text from lamp button and set text
        lampButtonText = lampButton.GetComponentInChildren<Text>();
        if(lampLight != null && lampButtonText != null)
            lampButtonText.text = string.Format("Lamp is {0}", lampLight.enabled ? "on!" : "off.");
        // Set lamp button colors
        if (lampLight != null && lampButton != null)
            lampButton.colors = lampLight.enabled ? onColors : offColors;
            
	}

    private void OnLampToggled(LampToggleEvent evt) {
        // Toggle the lamp light :)
        if (lampLight != null)
            lampLight.enabled = !lampLight.enabled;
        if (lampButton != null)
            lampButton.colors = lampLight.enabled ? onColors : offColors;
        if (lampButtonText != null)
            lampButtonText.text = string.Format("Lamp is {0}", lampLight.enabled ? "on!" : "off.");
    }

    // Turn lamp on/off with a click, going through event aggregator
    public void OnPointerClick(PointerEventData eventData) {
        this.Publish(new LampToggleEvent());
    }
}
public class LampToggleEvent { }
