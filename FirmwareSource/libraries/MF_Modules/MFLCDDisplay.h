// MFSegments.h
//
/// \mainpage MF Button module for MobiFlight Framework
/// \par Revision History
/// \version 1.0 Initial release
/// \author  Sebastian Moebius (mobiflight@moebiuz.de) DO NOT CONTACT THE AUTHOR DIRECTLY: USE THE LISTS
// Copyright (C) 2013-2014 Sebastian Moebius

#ifndef MFLCDDisplay_h
#define MFLCDDisplay_h

#if ARDUINO >= 100
#include <Arduino.h>
#else
#include <WProgram.h>
#include <wiring.h>
#endif

#include "../NewliquidCrystal/LiquidCrystal_I2C.h"

/////////////////////////////////////////////////////////////////////
/// \class MFSegments MFSegments.h <MFSegments.h>
class MFSegments
{
public:
    MFSegments();
    void display(byte module, char *string, byte points, byte mask, bool convertPoints = false);
    void attach(int dataPin, int csPin, int clkPin, int moduleCount, int brightness);
    void detach();
    void test();
    void powerSavingMode(bool state);
    void setBrightness(int module, int value);
    
private:
    LiquidCrystal_I2C  *_lcdDisplay;
    bool        _initialized;
    byte        _moduleCount;
};
#endif 