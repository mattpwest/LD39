using UnityEngine;
using UnityEngine.UI;

public class Skins : MonoBehaviour
{
    public Text referenceText;

    public Image referenceNewImage;
    public Image referenceButtonImage;

    public void Skin(GameObject gameObject)
    {
        foreach (var text in gameObject.GetComponentsInChildren<Text>())
        {
            text.font = referenceText.font;
            text.color = referenceText.color;
            text.fontStyle = FontStyle.Normal;
        }

        foreach (var image in gameObject.GetComponentsInChildren<Image>())
        {
            if (image.gameObject.name.Contains("Button") || image.gameObject.name.Equals("Handle"))
            {
                image.sprite = referenceButtonImage.sprite = referenceButtonImage.sprite;
                image.type = Image.Type.Sliced;
            }
            else
            {
                image.sprite = referenceNewImage.sprite;
                image.type = Image.Type.Sliced;
            }
        }
    }
}
