using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // UI
    [SerializeField]
    private Image receiveImage;

    // Other components
    [SerializeField]
    private NetworkManager networkManager;

    // Functions
    private void Start()
    {
        FileStream fileStream = new(networkManager.Receive(), FileMode.Open, FileAccess.Read);

        byte[] buffer = new byte[fileStream.Length];

        fileStream.Read(buffer, 0, buffer.Length);

        Texture2D texture = new Texture2D(0, 0);

        texture.LoadImage(buffer);

        SetReceiveImage(texture);

        fileStream.Close();
    }

    public void SetReceiveImage(Texture2D texture)
    {
        receiveImage.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.one * 0.5f);
    }
}
