//
//  DeviceDetail.mm
//
//  Created by Kunal Patel (IndiaNIC Infotech Pvt. Ltd.) on 5/10/13.
//  Modified by Ankur Ranpariya (GameAnax Studio Pvt. Ltd.)
//

#import "DeviceDetail.h"

//////  for mac address
#include <sys/socket.h>
#include <sys/sysctl.h>
#include <net/if.h>
#include <net/if_dl.h>

//////  for ip address
#include <ifaddrs.h>
#include <arpa/inet.h>

// This is just a helper method
// Converts C style string to NSString
NSString* CreateDDNSString (const char* string) {
    if (string)
        return [NSString stringWithUTF8String: string];
    else
        return [NSString stringWithUTF8String: ""];
}
NSString* const strEventManager = @"DeviceDetailUnity";

// When native code plugin is implemented in .mm / .cpp file, then functions
// should be surrounded with extern "C" block to conform C function naming rules
extern "C" {
    void _getCountry() {
        NSLocale *locale = [NSLocale currentLocale];
        NSString *countryCode = [locale objectForKey: NSLocaleCountryCode];
        NSString *str = [NSString stringWithFormat:@"%@",countryCode];
        UnitySendMessage([strEventManager UTF8String], "didRecevieCountry", [str UTF8String]);
    }
    
    void _getLanguageInfo() {
        NSString *strIdentifier = [[NSLocale currentLocale] localeIdentifier];
        NSString *strLanguages = [[NSLocale preferredLanguages] objectAtIndex:0];
        NSString *str = [NSString stringWithFormat:@"%@,%@",strIdentifier,strLanguages];
        UnitySendMessage([strEventManager UTF8String], "didReceiveLanguage", [str UTF8String]);
    }
    
    
    void _getMACAddress() {
        NSString *macAddressString = @"";
        if ([[UIDevice currentDevice] respondsToSelector:@selector(identifierForVendor)]) {
            macAddressString = [[[UIDevice currentDevice] identifierForVendor] UUIDString];
        }
        else {
            int                 mgmtInfoBase[6];
            char                *msgBuffer = NULL;
            size_t              length;
            unsigned char       macAddress[6];
            struct if_msghdr    *interfaceMsgStruct;
            struct sockaddr_dl  *socketStruct;
            NSString            *errorFlag = NULL;
            
            // Setup the management Information Base (mib)
            mgmtInfoBase[0] = CTL_NET;        // Request network subsystem
            mgmtInfoBase[1] = AF_ROUTE;       // Routing table info
            mgmtInfoBase[2] = 0;
            mgmtInfoBase[3] = AF_LINK;        // Request link layer information
            mgmtInfoBase[4] = NET_RT_IFLIST;  // Request all configured interfaces
            
            // With all configured interfaces requested, get handle index
            if ((mgmtInfoBase[5] = if_nametoindex("en0")) == 0)
                errorFlag = @"if_nametoindex failure";
            else {
                // Get the size of the data available (store in len)
                if (sysctl(mgmtInfoBase, 6, NULL, &length, NULL, 0) < 0)
                    errorFlag = @"sysctl mgmtInfoBase failure";
                else {
                    // Alloc memory based on above call
                    if ((msgBuffer = (char *) malloc(length)) == NULL)
                        errorFlag = @"buffer allocation failure";
                    else
                    {
                        // Get system information, store in buffer
                        if (sysctl(mgmtInfoBase, 6, msgBuffer, &length, NULL, 0) < 0)
                            errorFlag = @"sysctl msgBuffer failure";
                    }
                }
            }
            
            // Befor going any further...
            if (errorFlag != NULL) {
                NSLog(@"Error: %@", errorFlag);
                errorFlag = [NSString stringWithFormat:@"Error : %@",errorFlag];
                UnitySendMessage([strEventManager UTF8String], "didReceiveMACError", [errorFlag UTF8String]);
            }
            
            // Map msgbuffer to interface message structure
            interfaceMsgStruct = (struct if_msghdr *) msgBuffer;
            
            // Map to link-level socket structure
            socketStruct = (struct sockaddr_dl *) (interfaceMsgStruct + 1);
            
            // Copy link layer address data in socket structure to an array
            memcpy(&macAddress, socketStruct->sdl_data + socketStruct->sdl_nlen, 6);
            
            // Read from char array into a string object, into traditional Mac address format
            macAddressString = [NSString stringWithFormat:@"%02X:%02X:%02X:%02X:%02X:%02X",
                                macAddress[0], macAddress[1], macAddress[2],
                                macAddress[3], macAddress[4], macAddress[5]];
            //NSLog(@"Mac Address: %@", macAddressString);
            // Release the buffer memory
            free(msgBuffer);
        }
        UnitySendMessage([strEventManager UTF8String], "didReceiveMAC", [macAddressString UTF8String]);
    }
    
    void _getIPAddress() {
        struct ifaddrs *interfaces = NULL;
        struct ifaddrs *temp_addr = NULL;
        NSString *wifiAddress = nil;
        NSString *cellAddress = nil;
        
        // retrieve the current interfaces - returns 0 on success
        if(!getifaddrs(&interfaces)) {
            // Loop through linked list of interfaces
            temp_addr = interfaces;
            while(temp_addr != NULL) {
                sa_family_t sa_type = temp_addr->ifa_addr->sa_family;
                if(sa_type == AF_INET || sa_type == AF_INET6) {
                    NSString *name = [NSString stringWithUTF8String:temp_addr->ifa_name];
                    NSString *addr = [NSString stringWithUTF8String:inet_ntoa(((struct sockaddr_in *)temp_addr->ifa_addr)->sin_addr)]; // pdp_ip0
                    //NSLog(@"NAME: \"%@\" addr: %@", name, addr); // see for yourself
                    
                    if([name isEqualToString:@"en0"]) {
                        // Interface is the wifi connection on the iPhone
                        wifiAddress = addr;
                    } else if([name isEqualToString:@"pdp_ip0"]) {
                        // Interface is the cell connection on the iPhone
                        cellAddress = addr;
                    }
                }
                temp_addr = temp_addr->ifa_next;
            }
            // Free memory
            freeifaddrs(interfaces);
        }
        NSString *addr = wifiAddress ? wifiAddress : cellAddress;
        UnitySendMessage([strEventManager UTF8String] , "didReceiveIP", [addr UTF8String]);
    }
}
