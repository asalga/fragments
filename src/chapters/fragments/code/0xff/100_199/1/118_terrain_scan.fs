precision mediump float;

uniform vec2 u_res;
uniform float u_time;
#define BANDS 2.

float valueNoise(vec2 p){
  #define Y_SCALE 45343.
  #define X_SCALE 37738.  
  float x = p.x * X_SCALE;
  float y = p.y * Y_SCALE;  
  return fract( sin(x+y) * 23454.);
}

float smoothValueNoise(vec2 p){
  vec2 lv = fract(p);
  lv = smoothstep(0.,1.,lv);
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
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float t = u_time * .5;

  p *= 1. + sin(t/2.)/20.;
  p.y += t*1.;

  // p *= 2. * abs(sin(t));
  // p.x += sin(t);
  p.x += 2.* smoothValueNoise(vec2(t/2., 0));


  float n;
  n += smoothValueNoise(p*2.) * 0.5;
  n += smoothValueNoise(p*4.) * 0.25;
  n += smoothValueNoise(p*6.) * 0.125;
  n += smoothValueNoise(p*8.) * 0.0625;
  n /= .99375;

  i = floor(n*BANDS)/BANDS;
  float j = floor(n*BANDS*30.)/(BANDS*10.);

  i = fract( i * j - t*.8 );

  if(i < 0.3){
     i = 1.-0.3;
  }

 

  vec3 final = vec3(i);

 

  gl_FragColor = vec4(final,1.);
}




















