precision mediump float;

uniform vec2 u_res;
uniform float u_time;
#define BANDS 8.

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
  float t = u_time;


  //arbitrary value, trying to find a nice
  // spot for the noise
  p.y += 2.;
  p.x += u_time * .2; //move camera
  p *= .2; //zoom in like crazy, so it looks more organic
  float jumpCut = floor(t/2.);

  float n;
  n += smoothValueNoise(p*2.+ jumpCut)*0.5;
  n += smoothValueNoise(p*4.)*0.25;
  n += smoothValueNoise(p*6.)*0.125;
  n += smoothValueNoise(p*8.)*0.0625;
  n /= 1.5;
  n = clamp(n,0.,1.);
  
  i = pow(i,6.);
  float test = floor(n*BANDS);
  if(mod(test,2.)== 0.){
  	n += smoothValueNoise(p*8.+t*3.)*0.0625;
  }

	i = floor(n*BANDS)/BANDS;
  i = 1.-i;
  
  vec3 final = vec3(0);

  if(mod(test,2.) == 0.){
    vec3 rgb = vec3(0.2989, 0.5870, 0.1140);
    final = vec3(dot(rgb,vec3(n,i,i)));
  }

  gl_FragColor = vec4(final,1.);
}