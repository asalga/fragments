// 81 - "World 0-1"
precision mediump float;

uniform vec2 u_res;
uniform float u_time;


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
  float t = u_time * 14.;
  float i;
  vec3 col;
  float DEBUG = 1.;
  float ROWS = u_res.y/16.;

  float NUM_BRICK_LINES = 4.;
  float noiseScale = 0.1;
  
  float idx = floor(p.x*ROWS+t) * noiseScale;
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




  //floor(n*ROWS* NUM_BRICK_LINES) * .96 ;
  // n = floor(n*ROWS* NUM_BRICK_LINES) * .96 ;
  // n = 1.-step(n, p.y);
  // n = n*step(fract(p.y*ROWS* NUM_BRICK_LINES), 0.5);

  

  // col.rg = (fract(p*ROWS)) * DEBUG;
  col = vec3(n);
  gl_FragColor = vec4(col,1.);
}





























