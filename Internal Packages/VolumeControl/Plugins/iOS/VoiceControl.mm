#import "VoiceControl.h"
#import "soundbasecontrol-Swift.h"

#import <AVFoundation/AVFoundation.h>


// This is just a helper method
// Converts C style string to NSString
NSString* CreateNSString (const char* string)
{
    if (string)
        return [NSString stringWithUTF8String: string];
    else
        return [NSString stringWithUTF8String: ""];
}
// this is used as receiver object in Unity3D
NSString* const strEventManager = @"VoiceControl";

@interface VoiceControl () <MMPitchDelegate> {
    MMPitch *objMMPitch;
    bool isActivated;
    bool isInitlized;
}
@end

@implementation VoiceControl

-(void) voiceSampleReceviedWithFrequency:(NSArray<NSNumber *> *)frequency {
    //NSLog(@"frequency: %@", frequency);
    NSLog(@"voice sample received from Swift Code");
    
    NSMutableDictionary *postDict = [[NSMutableDictionary alloc]init];
    [postDict setValue:frequency forKey:@"samples"];
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:postDict options:0 error:nil];
    
    NSString *str;
    str = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    
    UnitySendMessage([strEventManager UTF8String], "PitchyVoiceSampleReceived", [str UTF8String]);
}

- (void)voiceSampleErrorWithEr:(NSString *)er {
    NSLog(@"ERROR: %@", er);
}

- (void)didReceiveMemoryWarning {
    // Dispose of any resources that can be recreated.
}

- (void) initPitchyNative {
    if(isInitlized ){
        NSLog(@"Pitchy is Already initialized, no need to initialized again");
        return;
    }
    
    objMMPitch = [MMPitch new];
    [objMMPitch setDelegate:self];
    isInitlized = true;
    UnitySendMessage([strEventManager UTF8String], "PitchyInitialized", [ @"" UTF8String]);
}
- (void) startPitchyNative {
    if(objMMPitch == nil){
        NSLog(@"Pitchy is is not initialized yet, please init it first");
        return;
    }
    if(isActivated && objMMPitch != nil){
        NSLog(@"Pitchy is Already Started, no need to cretae another setup");
        //return;
    }
    [objMMPitch start];
    isActivated = true;
}

- (bool) isPitchyStartedNative {
    return isActivated;
}
- (int) isPitchyHasAudioPermssion {
    int permissionStatus = -1;
    //    AVAudioSessionRecordPermission  * avPermission = AVAudioSession.sharedInstance().recordPermission();
    //    switch (avPermission) {
    //    case AVAudioSessionRecordPermission.granted:
    //        permissionStatus = 1;
    //            print("Permission granted");
    //    case AVAudioSessionRecordPermission.denied:
    //        permissionStatus = 0;
    //            print("Pemission denied");
    //    case AVAudioSessionRecordPermission.undetermined:
    //        permissionStatus = -1;
    //            print("Request permission here");
    //    default:
    //        break
    //    }
    return permissionStatus;
}

- (void) stopPitchyNative {
    if (objMMPitch != nil){
        [objMMPitch stop];
    }
    isActivated = false;
}
@end

// When native code plugin is implemented in .mm / .cpp file, then functions
// should be surrounded with extern "C" block to conform C function naming rules

// Need a instance of Voice controller to access method from Class
VoiceControl *voiceHandler;
extern "C" {
    
    
    int isHasMicPermssion(){
        //initVoiceHandler();
        //return [voiceHandler isPitchyHasAudioPermssion];
        
        int permissionStatus = -1;
        switch ([[AVAudioSession sharedInstance] recordPermission]) {
            case AVAudioSessionRecordPermissionGranted:
                permissionStatus = 1;
                NSLog(@"Permission grantd by user");
                break;
            case AVAudioSessionRecordPermissionDenied:
                NSLog(@"Permission has been declined by user");
                permissionStatus = 0;
                break;
                
            case AVAudioSessionRecordPermissionUndetermined:
                NSLog(@"user has not decied yet about grant permission or not");
                permissionStatus = -1;
                // This is the initial state before a user has made any choice
                // You can use this spot to request permission here if you want
                break;
                
            default:
                break;
        }
        return  permissionStatus;
    }
    
    bool askForMicPermission() {
        __block BOOL isGranted = false;
        [[AVAudioSession sharedInstance] requestRecordPermission:^(BOOL granted) {
            isGranted = granted;
            if (granted) {
                NSLog(@" Permission granted");
            }
            else {
                NSLog(@"Permission denied");
            }
        }];
        return isGranted;
    }
    
    
    /*
     Ptichy Code Wreapper
     */
    void initVoiceHandler() {
        if(voiceHandler == nil){
            voiceHandler = [[VoiceControl alloc] init];
        }
    }
    
    void initPitchy(){
        initVoiceHandler();
        [voiceHandler initPitchyNative];
    }
    void startPitchy() {
        NSLog(@"Has Permission: %d", isHasMicPermssion());
        initVoiceHandler();
        [voiceHandler startPitchyNative];
    }
    
    bool isPitchyStarted(){
        return [voiceHandler isPitchyStartedNative];
    }
    
    void stopPitchy() {
        initVoiceHandler();
        [voiceHandler stopPitchyNative];
    }
}
