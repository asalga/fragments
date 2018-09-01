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

float sdCircle(vec2 p, float r){
  return length(p)-r;
}

float sdRing(vec2 p, float r, float w){
  return abs(length(p)- (r*.5)) - w;
}


// float sdRing(vec2 p, float r1, float r2){
//   float u = 0.0;
//   float l = 0.01;
//   const float D = 0.005;

//   float c = smoothstep(l, u, sdCircle(p, r1)) -
//             smoothstep(l-D, u-D, sdCircle(p, r2));
//   return c * smoothstep(1., 0., r1*2.);
// }

const float BANDS = 105.;

void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float c;
  float ti = u_time * 0.0;

  float r = length(p) + ti;
  float theta = atan(p.y, p.x);

  vec2 pc = vec2(r,theta);
  float n;
  n += smoothValueNoise(pc*4.          );
  n += smoothValueNoise(pc*8.  + ti*2. ) * .5;
  n += smoothValueNoise(pc*16. + ti*4. ) * .25;
  n += smoothValueNoise(pc*32. + ti*6. ) * .125;
  n += smoothValueNoise(pc*64. + ti*8. ) * .0625;
  n /= 1.5;

  float test = floor(n*BANDS)/BANDS;
  // c = sdCircle(p, 0.5);

  float b = floor(((length(p))* BANDS))/BANDS;

  c = test * step(sdRing(p, 1., 0.5), 0.) * b;

  // c = step(sdRing(p, .5, 1.1), 0.);

  gl_FragColor = vec4(vec3(c),1.);

  if(c > 0.5){
    // gl_FragColor = vec4(vec3(0.0), 1.0);
  }
}