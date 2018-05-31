//
//  NativeFunction.mm
//
//  Created by Kunal Patel (IndiaNIC Infotech Pvt. Ltd.) on 5/10/13.
//  Modified by Ankur Ranpariya (GameAnax Studio Pvt. Ltd.)
//


#define DEGREES_RADIANS(angle) ((angle) / 180.0 * M_PI)
#import "NativeFunction.h"
#import <MessageUI/MFMailComposeViewController.h>
#import <UIKit/UIKit.h>

// This is just a helper method
// Converts C style string to NSString
NSString* CreateNativeFunctionNSString (const char* string) {
    if (string)
        return [NSString stringWithUTF8String: string];
    else
        return [NSString stringWithUTF8String: ""];
}

NSString* const strEventManager = @"NativeCodeUnity";
NativeFunction *native;
UIActivityIndicatorView *activityView;
NSArray *arrImageSequence;
UIImageView *imgView;
float animationSpeed;


@implementation NativeFunction
@synthesize isAnimationRunning;
-(void) rotateImage:(UIImageView *)imgView atSpeed:(NSTimeInterval) speed {
    
    [UIView beginAnimations:nil context:nil];
    [UIView setAnimationDuration:speed];
    imgView.transform = CGAffineTransformMakeRotation(DEGREES_RADIANS(angle));
    [UIView commitAnimations];
    
    [NSTimer scheduledTimerWithTimeInterval:0.01 target:self selector:@selector(recallRotation) userInfo:nil repeats:NO];
}

-(void) recallRotation{
    if (isAnimationRunning) {
        angle += 1;
        angle = (int)angle % 360;
        [native rotateImage:imgView atSpeed:animationSpeed];
    } else{
        angle = 0;
    }
}

-(void)setImageSequenceIn:(UIImageView *)imgView forFiles:(NSString *)fileNames { }

-(NSArray *) imageArrayFromFiles:(NSString*) fileName{
    NSArray *fileNames = [fileName componentsSeparatedByString:@","];
    NSMutableArray *arrImage = [[NSMutableArray alloc] init];
    for (int i=0; i < [fileNames count]; i++) {
        NSString *strFile = [fileNames objectAtIndex:i];
        UIImage *img = [UIImage imageNamed:strFile];
        [arrImage addObject:img];
    }
    return arrImage;
}

- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex{
    NSString *strButton = [alertView buttonTitleAtIndex:buttonIndex];
    UnitySendMessage([strEventManager UTF8String] , "alertClicked", [strButton UTF8String]);
}



-(NSString*)checkMode {
    NSArray *arrOf2GStatus = [NSArray arrayWithObjects:[NSString stringWithFormat:@"%@",@"CTRadioAccessTechnologyGPRS"],[NSString stringWithFormat:@"%s","CTRadioAccessTechnologyEdge"],[NSString stringWithFormat:@"%s","CTRadioAccessTechnologyCDMA1x"], nil];
    
    NSString *aStrNetworkMode;
    CTTelephonyNetworkInfo *telephonyInfo = [CTTelephonyNetworkInfo new];
    NSLog(@"Current Radio Access Technology: %@", telephonyInfo.currentRadioAccessTechnology);
    NSPredicate *predicate = [NSPredicate predicateWithFormat:@"self contains %@",telephonyInfo.currentRadioAccessTechnology];
    NSArray *arrOfFilterStatus = [arrOf2GStatus filteredArrayUsingPredicate:predicate];
    if(arrOfFilterStatus.count > 0)
    {
        aStrNetworkMode = @"2G";
    }
    else if ([telephonyInfo.currentRadioAccessTechnology isEqualToString:@"CTRadioAccessTechnologyLTE"]) {
        aStrNetworkMode = @"4G";
    }
    else
    {
        aStrNetworkMode = @"3G";
    }
    
    //    [NSNotificationCenter.defaultCenter addObserverForName:CTRadioAccessTechnologyDidChangeNotification
    //                                                    object:nil
    //                                                     queue:nil
    //                                                usingBlock:^(NSNotification *note)
    //     {
    //         NSLog(@"New Radio Access Technology: %@", telephonyInfo.currentRadioAccessTechnology);
    //     }];
    
    return aStrNetworkMode;
}
@end

// When native code plugin is implemented in .mm / .cpp file, then functions
// should be surrounded with extern "C" block to conform C function naming rules
extern "C" {
    void initNativeFunction() {
        if(native == nil){
            native = [[NativeFunction alloc] init];
        }
    }
    
    int _canSendMail() {
        if ([MFMailComposeViewController canSendMail]){
            return 1;
        }
        else{
            return 0;
        }
    }
    
    void animateImage(float angle) {
        [UIView beginAnimations:nil context:nil];
        [UIView setAnimationBeginsFromCurrentState:YES];
        imgView.transform = CGAffineTransformMakeRotation(DEGREES_RADIANS(angle));
        [UIView commitAnimations];
    }
    
    void _showActivityView() {
        if (activityView == nil) {
            activityView = [[UIActivityIndicatorView alloc] initWithFrame:CGRectMake(0, 0, 60, 60)];
        }
        
        UIWindow* keywindow = [[UIApplication sharedApplication] keyWindow];
        activityView.center = CGPointMake(keywindow.frame.size.width/2, keywindow.frame.size.height/2);
        [keywindow.rootViewController.view addSubview:activityView];
        activityView.activityIndicatorViewStyle = UIActivityIndicatorViewStyleWhiteLarge;
        [activityView startAnimating];
    }
    
    void _hideActivityView() {
        [activityView removeFromSuperview];
        [imgView removeFromSuperview];
        native.isAnimationRunning = FALSE;
    }
    
    void _showSingleImage(const char *fileName,float rotationSpeed) {
        
        initNativeFunction();
        
        animationSpeed = rotationSpeed;
        NSString *imageName = CreateNativeFunctionNSString(fileName);
        
        if(imgView == nil){
            imgView = [[UIImageView alloc] initWithFrame:CGRectMake(0, 0, 100, 100)];
        }
        
        [imgView stopAnimating];
        UIWindow* keywindow = [[UIApplication sharedApplication] keyWindow];
        [keywindow.rootViewController.view addSubview:imgView];
        UIImage *img = [UIImage imageNamed:imageName];
        
        imgView.center = CGPointMake(keywindow.frame.size.width/2, keywindow.frame.size.height/2);
        [keywindow.rootViewController.view addSubview:imgView];
        imgView.contentMode = UIViewContentModeScaleAspectFit;
        imgView.backgroundColor= [UIColor clearColor];
        imgView.image = img;
        
        
        CGSize size = img.size;
        
        imgView.frame = CGRectMake(0, 0, size.width/2, size.height/2);
        imgView.center = CGPointMake(keywindow.frame.size.width/2, keywindow.frame.size.height/2);
        
        
        native.isAnimationRunning = TRUE;
        
        [native rotateImage:imgView atSpeed:1];
    }
    
    void _changeImageSequence(const char *fileNames) {
        initNativeFunction();
        arrImageSequence = [native imageArrayFromFiles:CreateNativeFunctionNSString(fileNames)];
        
    }
    
    void _showImageSequence(const char *fileNames,float animSpeed) {
        
        initNativeFunction();
        
        [UIView beginAnimations:nil context:nil];
        [UIView setAnimationDuration:0];
        imgView.transform = CGAffineTransformMakeRotation(DEGREES_RADIANS(0));
        [UIView commitAnimations];
        
        animationSpeed = animSpeed;
        NSString *imageNames = CreateNativeFunctionNSString(fileNames);
        
        if(imgView == nil){
            imgView = [[UIImageView alloc] initWithFrame:CGRectMake(0, 0, 100, 100)];
        }
        native.isAnimationRunning = FALSE;
        
        UIWindow* keywindow = [[UIApplication sharedApplication] keyWindow];
        [keywindow.rootViewController.view addSubview:imgView];
        
        
        [keywindow.rootViewController.view addSubview:imgView];
        imgView.contentMode = UIViewContentModeScaleAspectFit;
        imgView.backgroundColor= [UIColor clearColor];
        
        if(arrImageSequence == nil){
            _changeImageSequence(fileNames);
        }
        NSArray *nativeArray = [native imageArrayFromFiles:imageNames];
        CGSize size = [[nativeArray objectAtIndex:0] size];
        
        imgView.frame = CGRectMake(0, 0, size.width/2, size.height/2);
        imgView.center = CGPointMake(keywindow.frame.size.width/2, keywindow.frame.size.height/2);
        
        imgView.animationImages = nativeArray;
        imgView.animationDuration = animationSpeed;
        imgView.animationRepeatCount = 0;
        [imgView startAnimating];
    }
    
    void _openURL(const char * url) {
        
        NSString *strURL = CreateNativeFunctionNSString(url);
        NSURL *nsurl = [NSURL URLWithString:strURL];
        [[UIApplication sharedApplication] openURL:nsurl];
    }
    
    void _openReview(const char * appId) {
        
        NSString *strAppID = CreateNativeFunctionNSString(appId);
        NSString *strURL = [NSString stringWithFormat:@"https://userpub.itunes.apple.com/WebObjects/MZUserPublishing.woa/wa/addUserReview?type=Purple+Software&id=%@&mt=8&o=i",strAppID];
        _openURL([strURL UTF8String]);
        
    }
    
    
    void _showTwoButtonAlert(const char * title,const char * message,const char * button1,const char * button2) {
        initNativeFunction();
        NSString *strTitle = CreateNativeFunctionNSString(title);
        NSString *strMessage = CreateNativeFunctionNSString(message);
        NSString *strButton1 = CreateNativeFunctionNSString(button1);
        NSString *strButton2 = CreateNativeFunctionNSString(button2);
        UIAlertView *alert = [[UIAlertView alloc] initWithTitle:strTitle message:strMessage delegate:native cancelButtonTitle:nil otherButtonTitles:strButton1,strButton2, nil];
        [alert show];
    }
    
    void _showSingleButtonAlert(const char * title,const char * message,const char * button){
        initNativeFunction();
        NSString *strTitle = CreateNativeFunctionNSString(title);
        NSString *strMessage = CreateNativeFunctionNSString(message);
        NSString *strButton = CreateNativeFunctionNSString(button);
        UIAlertView *alert = [[UIAlertView alloc] initWithTitle:strTitle message:strMessage delegate:native cancelButtonTitle:nil otherButtonTitles:strButton, nil];
        [alert show];
    }
    
    void CheckNetworkStatus() {
        initNativeFunction();
        Reachability *reachability = [Reachability reachabilityForInternetConnection];
        
        NetworkStatus netStatus = [reachability currentReachabilityStatus];
        
        NSString* statusString ;
        
        switch (netStatus) {
            case NotReachable: {
                statusString = @"No network Access";
                break;
            }
            case ReachableViaWWAN: {
                statusString = [native checkMode];
                break;
            }
            case ReachableViaWiFi: {
                statusString = @"WiFi";
                break;
            }
            default: {
                break;
            }
                
        }
        UnitySendMessage([strEventManager UTF8String] , "didReceiveNetworkMode", [statusString UTF8String]);
    }
    
    void openWifiSettings() {
        if ([[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString:@"prefs:root=WIFI"]]) {
            [[UIApplication sharedApplication] openURL:[NSURL URLWithString:@"prefs:root=WIFI"]];
        } else {
            [[UIApplication sharedApplication] openURL:[NSURL URLWithString:@"App-Prefs:root=WIFI"]];
        }
    }
}
