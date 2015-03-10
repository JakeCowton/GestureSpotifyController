# Leap possibilities
## In-built gesture recognition
 - Not very extensive
 - Not extendable
 - **Could be used for basic gestures e.g. swipe hand over to pause/resume**

## Motions API
 - Avoids tracking individual hands and fingers over multiple frames

#### Three types of motion
 - **Translation**
     + *Tracks linear movement in 3 dimension*
 - **Scale**
     + *Tracks the relative expansion or contraction*
 - **Rotation**
     + *Tracks the angular change in 3 dimensions*

#### Properties of motion
 - **Rotation axis**
     + *A direction vector expressing the acis of rotation*
 - **Rotation angle**
     + *The angle of rotation clockwise around the rotation axis*
 - **Rotation matrix**
     + *A transform matrix representing the rotation*
 - **Scale factor**
     + *A factor expressing expansion or contraction*
 - **Translation**
     + *A vector expressing the linear movement*

## Implementation
#### In-built gesture recognition
 - Swipe left or right to pause playback
 - Swipe left or right to resume playback

#### Motions API
 - Mimic turning a volume knob clockwise for volume up
 - Mimic turning a volume knob anti-clockwise for volume down

###### Possible implementation
 1. Recognise the start of the gesture using any classification system (ANN, ID3 etc..)
 2. Use motions to track the degree and direction of the rotation
 3. Adjust the volume accordingly
 4. Detect the end of the gesture (Could use motions [translation property] to detect linear movement (signalling the user has completed the gesture))