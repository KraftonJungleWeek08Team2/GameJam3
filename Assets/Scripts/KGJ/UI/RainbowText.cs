using TMPro;
using UnityEngine;

public class RainbowText : MonoBehaviour
{
    TextMeshProUGUI _text;

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _text.ForceMeshUpdate();
        var textInfo = _text.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            float hue = ((Time.time * 1f) + (i * 0.1f)) % 1f;
            Color rainbowColor = Color.HSVToRGB(hue, 1f, 1f);

            int vertexIndex = charInfo.vertexIndex;
            var colors = textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;

            colors[vertexIndex + 0] = rainbowColor;
            colors[vertexIndex + 1] = rainbowColor;
            colors[vertexIndex + 2] = rainbowColor;
            colors[vertexIndex + 3] = rainbowColor;
        }

        _text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
}
