precision mediump float;

uniform vec2 u_res;
uniform float u_time;
#define BANDS 10.

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
  vec2 p = (gl_FragCoord.xy/u_res*2.-1.);
  vec2 op = p;
  float i;
  float t = u_time;

  // p.x += 51.;
  // p.y += 1.;
  // p *= .83;

  //  :(
  p.x -= 3.25;
  p *= .6;
  p.y += 0.54;
  p.y = 1.-p.y;


  float n;
  n += smoothValueNoise(p*2.)*0.5;
  p.y -= t;
  p.x -= t/10.;
  n += smoothValueNoise(p * 32.)*(0.03125*.5);
  n /= .453125;

  // shitty shaping
  n *= 1./((op.y+1.)/2.) - .1;

  if(n > 0.4){
    n = 0.;
  }

  i = floor(n*BANDS)/BANDS;

  if(n > 0.14){
    i = 1.-i;
  }


  // vec3 rgb = vec3(0.2989, 0.5870, 0.1140);
  // vec3 final = vec3(dot(rgb,vec3(n,i,i)));

  gl_FragColor = vec4(vec3(i),1.);

  // hack
  if(gl_FragCoord.y > 400.){
    gl_FragColor = vec4(0., 0., 0., 1.);
  }
}