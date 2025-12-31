// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

// define the ip addresses/hostnames
string[] addresses = {"www.google.com", "www.facebook.com", "www.instagram.com", "www.twitter.com", "www.linkedin.com", "www.snapchat.com", "www.x.com"};
Console.WriteLine($"\n\n addresses: {String.Join(", ", addresses)}\n\n");

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

// use a for loop
List<Dictionary<string, string>> invalidData = new List<Dictionary<string, string>>();
foreach (string address in addresses)
{
    
    // get the ipv4 ip addresses for the address
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
        invalidData.Add(new Dictionary<string,string>
        {
            {address, "no IPv4 address found for hostname."},
        });
        continue;
    }
    
    // send request and get pinger response
    PingReply pingResponse = pinger.Send(address: ipv4Address, timeout: pingTimeout, buffer: buffer, options: pingOptions)
    switch (pingResponse.Status)
    {
        case IPStatus.Success:
            Console.WriteLine(
                $"\n\n address: {address} \n\n ipv4Address: {pingResponse.Address.MapToIPv4()}\n\n status: {pingResponse.Status}\n\n"
            );
            break;

        case IPStatus.TimedOut:
        case IPStatus.DestinationHostUnreachable:
            invalidData.Add(new Dictionary<string, string>
            {
                { "address", address },
                { "error", $"no connection to {ipv4Address}" }
            });
            break;

        default:
            invalidData.Add(new Dictionary<string, string>
            {
                { "address", address },
                { "error", "overfail dey worry this guy." }
            });
            break;
    }

}

if (invalidData.Count > 0)
{
    foreach (var entry in invalidData)
    {
        Console.WriteLine($"Address: {entry["address"]}");
        Console.WriteLine($"Error: {entry["error"]}");
    }
} 
