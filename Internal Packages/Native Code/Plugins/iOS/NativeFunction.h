//
//  DeviceDetail.h
//
//
//  Created by IndiaNIC on 5/10/13.
//
//

#import <UIKit/UIKit.h>
#import "Reachability.h"
#import <CoreTelephony/CTTelephonyNetworkInfo.h>

@interface NativeFunction : NSObject{
    float angle;
}

@property (nonatomic,readwrite) BOOL isAnimationRunning;
-(void) rotateImage:(UIImageView *)imgView atSpeed:(NSTimeInterval) speed;
-(void) setImageSequenceIn:(UIImageView *)imgView forFiles:(NSString *)fileNames;
-(NSArray *) imageArrayFromFiles:(NSString*) fileName;
-(NSString*) checkMode;
@end
