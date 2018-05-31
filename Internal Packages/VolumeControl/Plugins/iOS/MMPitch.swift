//
//  MMPitch.swift
//  PitchDetectionDemoObj
//
//  Created by indianic on 07/04/17.
//  Copyright © 2017 indianic. All rights reserved.
//

import UIKit

@objc protocol MMPitchDelegate{
    func voiceSampleRecevied(frequency: [Float])
    func voiceSampleError(er: String)
}

class MMPitch: UIViewController, PitchEngineDelegate {
    
    var delegate: MMPitchDelegate! = nil
    
    lazy var pitchEngine: PitchEngine = { [weak self] in
        var config = Config(estimationStrategy: .yin)
        let pitchEngine = PitchEngine(config: config, delegate: self!)
        pitchEngine.levelThreshold = -30.0
        
        return pitchEngine
        }()
    
    override func viewDidLoad() {
    }
    
    func start() {
        // Do any additional setup after loading the view, typically from a nib.
        pitchEngine.start()
    }
    func stop(){
        pitchEngine.stop()
    }
    
    func pitchEngineDidReceivePitch(_ pitchEngine: PitchEngine, pitch: Pitch?, frequencyArray: [Float]) {
        delegate.voiceSampleRecevied(frequency: frequencyArray)
    }
    
    func pitchEngineDidReceiveError(_ pitchEngine: PitchEngine, error: Error) {
        //print(error)
        delegate.voiceSampleError(er: error.localizedDescription)
    }
    
    public func pitchEngineWentBelowLevelThreshold(_ pitchEngine: PitchEngine) {
        print("Below level threshold")
    }
}
