using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private Socket socket;

    private void Awake()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        socket.Connect("xxx.xxx.xxx.xxx", 2139);
    }

    /// <summary>
    /// Receive data to server.
    /// </summary>
    /// <returns>Returns the created file path.</returns>
    public string Receive()
    {
        // Get file size
        byte[] buffer = new byte[4];

        socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);

        int fileLength = BitConverter.ToInt32(buffer, 0);

        // Get file data
        buffer = new byte[1024];

        string directoryName = Application.persistentDataPath;
        string fileName = "Receive.png";
        string path = Path.Combine(directoryName, fileName);

        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }

        FileStream fileStream = !File.Exists(path) ? File.Create(path) : new(path, FileMode.Create, FileAccess.Write);
        BinaryWriter binaryWriter = new(fileStream);

        for (int totalLength = 0, recvBytes = 0; totalLength < fileLength; totalLength += recvBytes)
        {
            recvBytes = socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);

            binaryWriter.Write(buffer, 0, recvBytes);
        }

        binaryWriter.Close();
        fileStream.Close();

        return path;
    }
}
