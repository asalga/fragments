precision mediump float;

uniform vec2 u_res;
uniform float u_time;

#define Y_SCALE 45343.
#define X_SCALE 37738.

/*
*/
float valueNoise(float seed, vec2 p){
  
  float x = p.x * X_SCALE;
  float y = p.y * Y_SCALE;
  
  return fract( sin(x+y) * (23454. + seed));
}

float smoothValueNoise(vec2 p){
  vec2 lv = fract(p);
  lv = lv*lv*(3.-2.*lv);
  vec2 id = floor(p);
  float bl = valueNoise(0., vec2(id));
  float br = valueNoise(0., vec2(id)+vec2(1,0));
  // float tr = valueNoise(vec2(id)+vec2(0.,1.));
  float b = mix(bl,br,lv.x);

  float tl = valueNoise(0., vec2(id)+vec2(0,1));
  float tr = valueNoise(0., vec2(id)+vec2(1,1));
  float t = mix(tl,tr,lv.x);

  // c.rg += lv;
  // i += lv;
  return mix(b,t,lv.y);
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res;
  float c;
  float ti = u_time * 1.;

  c += smoothValueNoise(p*4.          );
  c += smoothValueNoise(p*8.  + ti*2. ) * .5;
  c += smoothValueNoise(p*16. + ti*4. ) * .25;
  c += smoothValueNoise(p*32. + ti*6. ) * .125;
  c += smoothValueNoise(p*64. + ti*8. ) * .0625;

  c /= 3.;
  vec3 col = vec3(c);

  gl_FragColor = vec4(col,1);
}