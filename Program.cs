// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

// define the ip addresses/hostnames
string[] addresses = {"www.google.com"};
Console.WriteLine($"\n\n addresses: {String.Join(", ", addresses)}\n\n");

string address = addresses[0];

// get the ipv4 ip addresses for the first address
IPAddress[] ipv4Addresses = Dns.GetHostAddresses(address);
IPAddress? ipv4Address = null;
foreach (var addr in ipv4Addresses)
{
    if (addr.AddressFamily == AddressFamily.InterNetwork)
    {
        ipv4Address = addr;
        break; 
    }
}

if (ipv4Address == null)
{
    Console.WriteLine("No IPv4 address found for hostname.");
    return;
}

Console.WriteLine($"\n\n hostname: {address}\n ip address: {ipv4Address.MapToIPv4()}");

// initialize the pinger
Ping pinger = new();

// set up the pinger
PingOptions pingOptions = new()
{
    DontFragment = true
};

// set up pinger request
string data = "saviganga ti wole ninu c# world";
byte[] buffer = Encoding.ASCII.GetBytes(data);
int pingTimeout = 120;

// send request and get pinger response
PingReply pingResponse = pinger.Send(address: ipv4Address, timeout: pingTimeout, buffer: buffer, options: pingOptions);

// return response
Console.WriteLine($"\n\n ipv4Adress: {pingResponse.Address.MapToIPv4()}\n\n status: {pingResponse.Status.ToString()}\n\n");
