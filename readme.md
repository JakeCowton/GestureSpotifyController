# Gesture Controller for Spotify

This software is a basic proof of concept to demonstrate the use of neural networks in static gesture recognition using the Microsoft Kinect.

## Requirements

- Xbox 360 Kinect
- Kinect SDK 1.8
- MS Speech SDK 11.0
- Leap Motion*
- Leap Motion SDK 2.2.7*

* Optional

## Usage

Run the software in Visual Studio and allow the nueral network to train. Once this has completed you can carry out the gestures infront of the Kinect

## Gestures
### Kinect
#### Play

Both arms up to the side

```
                 |
                 |
----------------------------------------
                 |
                 |
                 |
                 |
                 |
                 |
                 |

```

#### Pause

Both arms up in the air

```
               | | |
               | | |
               |---|
                 |
                 |
                 |
                 |
                 |
                 |
                 |

```

#### Skip

Right arm up by side

```
                  |
                  |
            --------------------------
            |     |
            |     |
            |     |
            |     |
            |     |
            |     |
                  |

```

#### Previous

Left arm up by side

```
                  |
                  |
--------------------------
                  |      |
                  |      |
                  |      |
                  |      |
                  |      |
                  |      |
                  |

```

#### Volume Up

Both arms pointing up, 90 degree bend at elbow

```
            |     |    |
            |     |    |
             -----------
                  |
                  |
                  |
                  |
                  |
                  |
                  |

```

#### Volume Down

Both arms pointing down, 90 degree bend at elbow

```
                  |
                  |
             -----------
             |    |   |
             |    |   |
                  |
                  |
                  |
                  |
                  |

```

#### Mute

Right hand over mouth

```
                  |----
                  |   |
            -----------
            |     |
            |     |
            |     |
            |     |
            |     |
            |     |
                  |

```

### Leap

#### Volume Up

Draw clockwise circle with a pointed finger

#### Volume Down

Draw anti-clockwise circle with a pointed finger