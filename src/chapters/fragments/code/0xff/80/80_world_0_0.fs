// 81 - "World 0-1"
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

// morter is at every 7/8 of a pixel across
#define SEVEN_EIGHTS (7./8.)

float valueNoise(vec2 p){
  #define Y_SCALE 45343.
  #define X_SCALE 37738.  
  float x = p.x * X_SCALE;
  float y = p.y * Y_SCALE;
  
  return fract( sin(x+y) * 23454.);
}

float smoothValueNoise(vec2 p){
  vec2 lv = fract(p);
  lv = lv*lv*(3.-2.*lv);
  vec2 id = floor(p);
  float bl = valueNoise(vec2(id));
  float br = valueNoise(vec2(id)+vec2(1,0));
  float b = mix(bl,br,lv.x);

  float tl = valueNoise(vec2(id)+vec2(0,1));
  float tr = valueNoise(vec2(id)+vec2(1,1));
  float t = mix(tl,tr,lv.x);

  return mix(b,t,lv.y);
}
 
void main(){
  vec2 p = (gl_FragCoord.xy/u_res);
  float t = u_time * 1.1;
  float i;
  vec3 col;
  float ROWS = u_res.y/16.;

  float NUM_BRICK_LINES = 4.;
  float noiseScale = 0.08;
  
  p.x += t;

  float idx = floor(p.x*ROWS) * noiseScale;
  float n;
  n += smoothValueNoise(vec2(idx,0.)*2.)*0.5;
  n += smoothValueNoise(vec2(idx,0.)*4.)*0.25;
  n += smoothValueNoise(vec2(idx,0.)*8.)*0.125;
  n /= 1.5;
  n = clamp(n,0.,1.);

  // Each column will get a different noise value
  // based on the 'index'
  float columnHeight = n;

  n = floor(columnHeight*ROWS)/ROWS;
  n = step(p.y,n);

  // add brick lines
  // "16 tiles rows" high * lines per tile
  // morter is at every 3/4 of a pixel
  float horizLines = step(fract(p.y * ROWS * NUM_BRICK_LINES), (3./4.));
  n *= horizLines;

  // shift x every other row inside the tile
  float shift = SEVEN_EIGHTS *2.* (step(fract(p.y*ROWS*2.), SEVEN_EIGHTS/2.));
  float vertLines = step(fract( (p.x+shift/ROWS) * ROWS*2.), SEVEN_EIGHTS);
  n *= vertLines;

  col = vec3(n);
  gl_FragColor = vec4(col,1);
}