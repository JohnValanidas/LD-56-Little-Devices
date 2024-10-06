
using TMPro;
using UnityEngine;

public class Version : MonoBehaviour {
    private string _versionCode;
    private TMP_Text _text;
    // Start is called before the first frame update
    void Start() {
        _versionCode = Application.version;
        _text = GetComponentInParent<TMP_Text>();
        _text.SetText(_versionCode);
    }
}
