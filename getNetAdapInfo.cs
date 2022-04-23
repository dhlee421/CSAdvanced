using System.Management;

var query =
new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled='TRUE'");

var machineNetworkInfo = from ManagementBaseObject mo in queryCol
                                     let arrIpAddress = (string[])mo["IPAddress"]
                                     where arrIpAddress.Length >= 1
                                     select new MachineNetworkInfo
                                     {
                                          MacAddress = mo["MACAddress"].ToString(),
                                          NetworkIp = arrIpAddress[0],
                                      };
