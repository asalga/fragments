precision mediump float;
uniform vec2 u_res;
#define YELLOW vec3(1.,.6,.33)
#define ORANGE vec3(1.,.2,.01)
#define X(n) (f0[n] = vec3(0.));
#define _(n) (f0[n] = YELLOW);
#define U(n) (f0[n] = ORANGE);
#define K(n) (f0[n] = vec3(1.));
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = gl_FragCoord.xy/u_res * vec2(8., 16.);
  vec3 f0[8*16];

  X(0x0)X(0x1)X(0x2)        _(  3)_(  4)          X(  5)X(  6)X(  7)
  X(  8)X(  9)        _( 10)_( 11)_( 12)_( 13)          X( 14)X(  15)
  X( 16)        _( 17)_( 18)_( 19)_( 20)_( 21)_( 22)          X(  23)
  X( 24)        _( 25)_( 26)U( 27)_( 28)_( 29)_( 30)          X(  31)
          _( 32)_( 33)U( 34)_( 35)_( 36)_( 37)_( 38)_( 39)
          _( 40)_( 41)U( 42)_( 43)_( 44)_( 45)_( 46)_( 47)
          _( 48)_( 49)U( 52)_( 51)_( 52)_( 53)_( 54)_( 55)
          _( 56)_( 57)U( 58)_( 59)_( 60)_( 61)_( 62)_( 63)
          _( 64)_( 65)U( 66)_( 67)_( 68)_( 69)_( 70)_( 71)
          _( 72)_( 73)U( 74)_( 75)_( 76)_( 77)_( 78)_( 79)
          _( 80)_( 81)U( 82)_( 83)_( 84)_( 85)_( 86)_( 87)
          _( 88)_( 89)U( 90)_( 91)_( 92)_( 93)_( 94)_( 95)
          _( 96)_( 97)U( 98)_( 99)_(100)_(101)_(102)_(103)
  X(104)        _(105)_(106)U(107)_(108)_(109)_(110)          X(111)
  X(112)        _(113)_(114)_(115)_(116)_(117)_(118)          X(119)
  X(120)X(121)        _(122)_(123)_(124)_(125)          X(126)X(127)

  vec3 col = f0[19];

  for(int i = 0; i < 8*16; i++){
    col = f0[i];

    int y = int(floor(p.y));
    int x = int(floor(p.x));

    if ( y + x == i ){
    	break;
    }
  }

  gl_FragColor = vec4(col, 1.);
}