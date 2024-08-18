using System.Net;
using System.Net.Sockets;

// Variables
FileStream fileStream = new FileStream("E:\\.png", FileMode.Open, FileAccess.Read);
BinaryReader binaryReader = new BinaryReader(fileStream);
Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
Socket clientSocket;

// Initialize sockets
serverSocket.Bind(new IPEndPoint(IPAddress.Any, 2139));
serverSocket.Listen(1);

// Accept client
clientSocket = serverSocket.Accept();

// Send file length to client
int fileLength = (int)fileStream.Length;
byte[] fileBuffer = BitConverter.GetBytes(fileLength);

clientSocket.Send(fileBuffer, 0, fileBuffer.Length, SocketFlags.None);

// Send file data to client
int count = fileLength / 1024 + 1;

for (int i = 0; i < count; i++)
{
    fileBuffer = binaryReader.ReadBytes(1024);

    clientSocket.Send(fileBuffer, 0, fileBuffer.Length, SocketFlags.None);
}

// Shutdown
clientSocket.Close();
serverSocket.Close();
binaryReader.Close();
fileStream.Close();