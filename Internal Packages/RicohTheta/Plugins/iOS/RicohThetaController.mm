//
//  * Copyright Ricoh Company, Ltd. All rights reserved.
//  * Modiffied bt Ankur Ranapariya and Umang Patel (GameAnax Studio Pvt. Ltd.)
//
#import "HttpConnection.h"
#import "RicohThetaController.h"
#import "HttpImageInfo.h"

HttpConnection* _httpConnection;
bool _isConnected;
bool _isLiveFeedOn;
bool _isCapturing;
UIImage * _capturedImage;

// Converts C style string to NSString
NSString* CreateThetaNSString (const char* string) {
    if (string)
        return [NSString stringWithUTF8String: string];
    else
        return [NSString stringWithUTF8String: ""];
}

inline static void dispatch_async_main(dispatch_block_t block) {
    dispatch_async(dispatch_get_main_queue(), block);
}

inline static void dispatch_async_default(dispatch_block_t block) {
    dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_DEFAULT, 0), block);
}


RicohThetaController *ricohThetaController;
NSString* logData = @"";
NSString* eventRece = @"RicohThetaUnity";

//
@interface RicohThetaController (){
    HttpConnection* _httpConnection;
}
@end


@implementation RicohThetaController
- (void)appendLog:(NSString*)text {
    //logData =[NSString stringWithFormat:@"%@%@%@",logData,  text, @"\n"];
    NSLog(@"%@", text);
}


#pragma mark - HTTP Operations.
- (void) initRicohThetaNative{
    if(_httpConnection==nil){
        _httpConnection = [[HttpConnection alloc] init];
    }
}
- (void)connectRicohThetaNative:(NSString*)ip {
    [self initRicohThetaNative];
    [self appendLog:@"Connection process Start"];
    NSLog(@"Connecting to %@", ip);
    [_httpConnection setTargetIp:ip];
    [_httpConnection connect:^(BOOL connected) {
        // "Connect" and "OpenSession" completion callback.
        if (connected) {
            // "Connect" is succeeded.
            dispatch_async_main(^{
                [self appendLog:@"connected."];
                UnitySendMessage([eventRece UTF8String], "RicohThetaConnectionSuccessful", [ip UTF8String]);
            });
        } else {
            // "Connect" is failed.
            dispatch_async_main(^{
                [self appendLog:@"connect failed."];
                UnitySendMessage([eventRece UTF8String], "RicohThetaConnectionFailed","");
            });
        }
    }];
    
    [self appendLog:@"Connection process end"];
}

- (void)disconnectRicohThetaNative {
    [self initRicohThetaNative];
    [self appendLog:@"disconnecting..."];
    [_httpConnection close:^{
        // "CloseSession" and "Close" completion callback.
        dispatch_async_main(^{
            [self appendLog:@"disconnected."];
            UnitySendMessage([eventRece UTF8String], "RicohThetaCameraDisconnected", "");
        });
    }];
}
- (BOOL) isRicohThetaConnectedNative{
    [self initRicohThetaNative];
    return [_httpConnection connected];
}

#pragma mark - Objective C Opertaion
- (void)capture360ImageNavite:(bool)isUniqueFileName {
    if(_isCapturing) return;
    dispatch_async_default(^{
        _isCapturing = TRUE;
        if(_isLiveFeedOn){
            [self stopLiveCapture];
            _isLiveFeedOn = TRUE;
        }
        
        // Start shooting process
        HttpImageInfo *info = [_httpConnection takePicture];
        
        if (info != nil) {
            NSArray* fileNameParts =[info.file_id componentsSeparatedByString: @"/"];
            long size = [fileNameParts count] - 1;
            NSString* rawFileName = [fileNameParts objectAtIndex:size];
            
            fileNameParts = [rawFileName componentsSeparatedByString: @"."];
            rawFileName = [fileNameParts objectAtIndex:0];
            
            NSString* imageFileName = [rawFileName stringByAppendingString:@"_360.JPG"];
            NSString* thumbFileName = [rawFileName stringByAppendingString:@"_Thumb.JPG"];
            if(isUniqueFileName==false){
                imageFileName = @"image360.JPG";
                thumbFileName = @"imageThumb.JPG";
            }
            
            NSData* thumbData = [_httpConnection getThumb:info.file_id];
            NSData* imageData = [_httpConnection getImage:info.file_id];
            
            UIImage *thumbImage = [UIImage imageWithData:thumbData];
            UIImage *image = [UIImage imageWithData:imageData];
            
            //NSString *stringPath = [[NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, /YES)objectAtIndex:0]stringByAppendingPathComponent:@"ListVR"];
            NSString *stringPath = [[NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES)objectAtIndex:0]stringByAppendingString:@"/"];
            
            
            NSError *error = nil;
            if (![[NSFileManager defaultManager] fileExistsAtPath:stringPath])
                [[NSFileManager defaultManager] createDirectoryAtPath:stringPath withIntermediateDirectories:NO attributes:nil error:&error];
            
            NSString *fileName = [stringPath stringByAppendingString:imageFileName];
            NSData *data1 = UIImageJPEGRepresentation(image, 1.0);
            [data1 writeToFile:fileName atomically:YES];
            
            fileName = [stringPath stringByAppendingString:thumbFileName];
            data1 = UIImageJPEGRepresentation(thumbImage, 1.0);
            [data1 writeToFile:fileName atomically:YES];
            
            NSDictionary *setUser = [NSDictionary
                                     dictionaryWithObjectsAndKeys:
                                     thumbFileName,@"thumbName",
                                     imageFileName,@"imageUrl",nil];
            
            NSData* jsonData = [NSJSONSerialization dataWithJSONObject:setUser
                                                               options:NSJSONWritingPrettyPrinted error:&error];
            NSString* newStr = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
            //NSLog([NSString stringWithFormat:@"new string: %@", newStr]);
            UnitySendMessage([eventRece UTF8String], "RicohThetaImageCaptured",[newStr UTF8String]);
        } else{
            UnitySendMessage([eventRece UTF8String], "RicohThetaImageCaptureFailed","Unknown Error");
        }
        
        if(_isLiveFeedOn == TRUE){
            [NSThread sleepForTimeInterval:5];
            _isLiveFeedOn = FALSE;
            NSLog(@"%s", "trying to start live feed after image capture");
            [self startLiveCapture];
        }
        _isCapturing = FALSE;
        
    });
}
NSString* base64ImageData;

- (void) startLiveCapture{
    // Start live view display
    if(_isLiveFeedOn) return;
    UnitySendMessage([eventRece UTF8String], "RicohThetaStartedLiveStreaming","");
    [_httpConnection startLiveView:^(NSData *frameData) {
        _isLiveFeedOn = TRUE;
        dispatch_async_main(^{
            base64ImageData = [frameData base64EncodedStringWithOptions:0];
            UnitySendMessage([eventRece UTF8String], "RicohThetaLiveStreamingData",[base64ImageData UTF8String]);
        });
    }];
}

- (void) stopLiveCapture{
    // stop live view display
    if(!_isLiveFeedOn) return;
    [_httpConnection stopLiveView];
    _isLiveFeedOn = FALSE;
    UnitySendMessage([eventRece UTF8String], "RicohThetaStopLiveStreaming","");
}

@end

extern "C" {
    void initRecoTheta(){
        if(ricohThetaController == nil){
            ricohThetaController = [[RicohThetaController alloc] init];
            [ricohThetaController initRicohThetaNative];
        }
    }
    
    void _connectCamera(const char * ip) {
        NSLog(@"Started to Connect from Objective C");
        initRecoTheta();
        NSString* ipAddress = CreateThetaNSString(ip);
        [ricohThetaController connectRicohThetaNative: ipAddress];
    }
    
    void _disconnectCamera() {
        NSLog(@"Started to Disconnect from Objective C");
        initRecoTheta();
        [ricohThetaController disconnectRicohThetaNative];
    }
    
    int _isConnectedCamera() {
        initRecoTheta();
        bool state = [ricohThetaController isRicohThetaConnectedNative];
        if (state) return 1;
        else return 0;
    }
    void _captureImage(bool isUniqueFileName){
        initRecoTheta();
        [ricohThetaController capture360ImageNavite:isUniqueFileName];
    }
    void _startLiveStream(){
        initRecoTheta();
        [ricohThetaController startLiveCapture];
    }
    void _stopLiveStream(){
        initRecoTheta();
        [ricohThetaController stopLiveCapture];
    }
}
