using System.Net.Sockets;
using System.Net;
using System;
using System.Reflection;

const string address = "127.0.0.1";
const int port = 80;

var server = new TcpListener(IPAddress.Parse(address), port);

server.Start();
Console.WriteLine("Server has started on 127.0.0.1:80.{0}Waiting for a connection…", Environment.NewLine);

var client = server.AcceptTcpClient();

Console.WriteLine("A client connected.");