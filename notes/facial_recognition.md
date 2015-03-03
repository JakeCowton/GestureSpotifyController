# Facial Recognition Plan

[TOC]

## Feature Selection

An outline on how to choose which features will be used to classify a face.

### Available features

- *(x, y, z)* values of a feature point are not very helpful since a face in the bottom right of the screen would be totally different to the exact same face in the top left of the screen
- Therefore we will need to use the distance between two (or more) points
- Given that there are already a lot of points, if we were to pass the difference between every point and every point we would have *n^2* points which is far to many to handle

### Selecting features to use

We can use GAs to determine which point distances give the most information about a persons face

- Lets say (for example) there are 30 feature differences.
- These feature differences can be stored in an array (of len 30^2 I think)
- Whether we use a point or not will be denoted by a 1 or a 0 respectively (e.g. if we were only to use the first point difference, the chromosome would be 100000000000000000000000000000)
- Chromosomes are evaluated by seeing how well their respective values can be used to classify a face using a classificiation method (e.g. k-nearest-neighbors)

### Reference

See section 2.1.2 for a very, very quick example:
http://www.dtic.mil/dtic/tr/fulltext/u2/a235165.pdf

## Classification

TBC