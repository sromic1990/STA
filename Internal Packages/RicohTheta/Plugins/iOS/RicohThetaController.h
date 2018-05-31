/*
 * Copyright Ricoh Company, Ltd. All rights reserved.
 */

#import <UIKit/UIKit.h>

@interface RicohThetaController : NSObject
- (void) initRicohThetaNative;
- (void) connectRicohThetaNative:(NSString*)ip;
- (void) disconnectRicohThetaNative;
- (BOOL) isRicohThetaConnectedNative;
- (void) capture360ImageNavite:(bool)isUniqueFileName;
- (void) startLiveCapture;
- (void) stopLiveCapture;
@end
