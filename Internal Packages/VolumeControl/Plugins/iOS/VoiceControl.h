#import <UIKit/UIKit.h>

@interface VoiceControl : NSObject
- (void) initPitchyNative;
- (void) startPitchyNative;
- (void) stopPitchyNative;
- (int) isPitchyHasAudioPermssion;
@end
