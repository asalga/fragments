precision mediump float;
uniform vec2 u_res;
uniform float u_time;

#define _(n)(f0[n]=vec3(1.,.6,.33));
#define O(n)(f0[n]=vec3(1.));
#define W(n)(f0[n]=vec3(1.,.2,.01));

#define W1(n)(f1[n]=vec3(1.));
#define B1(n)(f1[n]=vec3(0.));
#define R1(n)(f1[n]=vec3(1., 0., 0.));

#define W2(n)(f2[n]=vec3(1.));
#define B2(n)(f2[n]=vec3(0.));
#define Y2(n)(f2[n]=vec3(1.,.6,.33));

#define B3(n)(f3[n]=vec3(0.));
#define O3(n)(f3[n]=vec3(1.,.2,.01));
#define Y3(n)(f3[n]=vec3(1.,.6,.33));

void main(){
  vec2 p = (gl_FragCoord.xy/u_res*vec2(10., 14.0));
  vec3 f0[8*14];
  vec3 f1[8*14];
  vec3 f2[8*14];
  vec3 f3[8*14];

                    _(003)_(004)    
              _(0xA)_(0xB)_(0xC)_(0xD)
        _( 17)_( 18)_( 19)_( 20)_( 21)_( 22)
        _( 25)_( 26)O( 27)W( 28)_( 29)_( 30)
  _( 32)_( 33)O( 34)_( 35)_( 36)W( 37)_( 38)_( 39)
  _( 40)_( 41)O( 42)_( 43)_( 44)W( 45)_( 46)_( 47)
  _( 48)_( 49)O( 50)_( 51)_( 52)W( 53)_( 54)_( 55)
  _( 56)_( 57)O( 58)_( 59)_( 60)W( 61)_( 62)_( 63)
  _( 64)_( 65)O( 66)_( 67)_( 68)W( 69)_( 70)_( 71)
  _( 72)_( 73)O( 74)_( 75)_( 76)W( 77)_( 78)_( 79)
        _( 81)_( 82)O( 83)W( 84)_( 85)_( 86)
        _( 89)_( 90)_( 91)_( 92)_( 93)_( 94)
              _( 98)_( 99)_(100)_(101)
                    _(107)_(108)
  
B1(  0)B1(  1)B1(  2)          W1(  3)R1(  4)        B1(  5)B1(  6)B1(  7)
B1(  8)B1(  9)B1(10)           W1( 11)R1( 12)        B1( 13)B1( 14)B1( 15)
B1( 16)B1( 17)         W1( 18)W1( 19)W1( 20)R1( 21)         B1( 22)B1( 23)
B1( 24)B1( 25)         W1( 26)W1( 27)W1( 28)R1( 29)         B1( 30)B1( 31)
B1( 32)B1( 33)         W1( 34)W1( 35)W1( 36)R1( 37)         B1( 38)B1( 39)
B1( 40)B1( 41)         W1( 42)W1( 43)W1( 44)R1( 45)         B1( 46)B1( 47)
B1( 48)B1( 49)         W1( 50)W1( 51)W1( 52)R1( 53)         B1( 54)B1( 55)
B1( 56)B1( 57)         W1( 58)W1( 59)W1( 60)R1( 61)         B1( 62)B1( 63)
B1( 64)B1( 65)         W1( 66)W1( 67)W1( 68)R1( 69)         B1( 70)B1( 71)
B1( 72)B1( 73)         W1( 74)W1( 75)W1( 76)R1( 77)         B1( 78)B1( 79)
B1( 80)B1( 81)         W1( 82)W1( 83)W1( 84)R1( 85)         B1( 86)B1( 87)
B1( 88)B1( 89)         W1( 90)W1( 91)W1( 92)R1( 93)         B1( 94)B1( 95)
B1( 96)B1( 97)B1( 98)         W1( 99)R1(100)         B1(101)B1(102)B1(103)
B1(104)B1(105)B1(106)         W1(107)R1(108)         B1(109)B1(110)B1(111)

B2(  0)B2(  1)B2(  2)     Y2(  3)    B2(  4)B2(  5)B2(  6)B2(  7)
B2(  8)B2(  9)B2(10)      Y2( 11)    B2( 12)B2( 13)B2( 14)B2( 15)
B2( 16)B2( 17)B2( 18)     Y2( 19)    B2( 20)B2( 21)B2( 22)B2( 23)
B2( 24)B2( 25)B2( 26)     Y2( 27)    B2( 28)B2( 29)B2( 30)B2( 31)
B2( 32)B2( 33)B2( 34)     Y2( 35)    B2( 36)B2( 37)B2( 38)B2( 39)
B2( 40)B2( 41)B2( 42)     Y2( 43)    B2( 44)B2( 45)B2( 46)B2( 47)
B2( 48)B2( 49)B2( 50)     W2( 51)    B2( 52)B2( 53)B2( 54)B2( 55)
B2( 56)B2( 57)B2( 58)     W2( 59)    B2( 60)B2( 61)B2( 62)B2( 63)
B2( 64)B2( 65)B2( 66)     Y2( 67)    B2( 68)B2( 69)B2( 70)B2( 71)
B2( 72)B2( 73)B2( 74)     Y2( 75)    B2( 76)B2( 77)B2( 78)B2( 79)
B2( 80)B2( 81)B2( 82)     Y2( 83)    B2( 84)B2( 85)B2( 86)B2( 87)
B2( 88)B2( 89)B2( 90)     Y2( 91)    B2( 92)B2( 93)B2( 94)B2( 95)
B2( 96)B2( 97)B2( 98)     Y2( 99)    B2(100)B2(101)B2(102)B2(103)
B2(104)B2(105)B2(106)     Y2(107)    B2(108)B2(109)B2(110)B2(111)

B3(  0)B3(  1)B3(  2)          Y3(  3)O3(  4)        B3(  5)B3(  6)B3(  7)
B3(  8)B3(  9)B3(10)           Y3( 11)O3( 12)        B3( 13)B3( 14)B3( 15)
B3( 16)B3( 17)         Y3( 18)Y3( 19)Y3( 20)O3( 21)         B3( 22)B3( 23)
B3( 24)B3( 25)         Y3( 26)Y3( 27)Y3( 28)O3( 29)         B3( 30)B3( 31)
B3( 32)B3( 33)         Y3( 34)Y3( 35)Y3( 36)O3( 37)         B3( 38)B3( 39)
B3( 40)B3( 41)         Y3( 42)Y3( 43)Y3( 44)O3( 45)         B3( 46)B3( 47)
B3( 48)B3( 49)         Y3( 50)Y3( 51)Y3( 52)O3( 53)         B3( 54)B3( 55)
B3( 56)B3( 57)         Y3( 58)Y3( 59)Y3( 60)O3( 61)         B3( 62)B3( 63)
B3( 64)B3( 65)         Y3( 66)Y3( 67)Y3( 68)O3( 69)         B3( 70)B3( 71)
B3( 72)B3( 73)         Y3( 74)Y3( 75)Y3( 76)O3( 77)         B3( 78)B3( 79)
B3( 80)B3( 81)         Y3( 82)Y3( 83)Y3( 84)O3( 85)         B3( 86)B3( 87)
B3( 88)B3( 89)         Y3( 90)Y3( 91)Y3( 92)O3( 93)         B3( 94)B3( 95)
B3( 96)B3( 97)B3( 98)         Y3( 99)O3(100)         B3(101)B3(102)B3(103)
B3(104)B3(105)B3(106)         Y3(107)O3(108)         B3(109)B3(110)B3(111)



  vec3 col;
  for(int i = 0; i < 8*14; i++){
  	if(p.x < 1. || p.x > 9.)break;
    if(p.y > 14.)break;
    
    float t = mod(u_time, 1.);

    if(t<.25){       col = f0[i]; }
    else if(t<0.5){  col = f1[i]; }
    else if(t<0.75){ col = f2[i]; }
    else if(t<=1.){  col = f3[i]; }

    int y = int(floor(p.y*1.15) -1.0);
    int x = int(floor(p.x-1.));
    if (y*8+x==i)break;
  }
  gl_FragColor = vec4(col, 1.);
}