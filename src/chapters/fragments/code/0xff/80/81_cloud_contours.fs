// 80 - "Cloud Contours"

precision mediump float;

uniform vec2 u_res;
uniform float u_time;

#define Y_SCALE 45343.
#define X_SCALE 37738.
#define PI 3.141592658
float circle(vec2 p, float r){
  return smoothstep(0.01, 0.001, length(p)-r);
}
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

  return mix(b,t,lv.y);
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float t = u_time*1.;

  float n;
  vec2 np = p+(t/10.);
  n += smoothValueNoise(np*8.)*.5;
  n += smoothValueNoise(np*16.)*.25;
  n += smoothValueNoise(np*32.)*.125;
  n += smoothValueNoise(np*64.)*.0625;
  n/=3.;

  vec2 np2 = p+((t+4.)/10.);
  float n2;
  n2 += smoothValueNoise(np2*8.)*.5;
  n2 += smoothValueNoise(np2*16.)*.25;
  n2 += smoothValueNoise(np2*32.)*.125;
  // n2 += smoothValueNoise(np2*64.)*.0625;
  n2 /=4.;
  // n = n2 = 0.;

  // p += 0.4;
  p*= 0.58;

  vec3 l = normalize(vec3(.7, .7, 0.9));
  float z = sqrt(1. - p.x*p.x - p.y*p.y);

  vec3 v = normalize(vec3(p.x+n, p.y+n2/4., z+n));
  vec3 v2 = normalize(vec3(p.x-n, p.y+n2/4., -z-n));

  float c = smoothstep(0.1, 0.95, sin(t*PI + dot(v,l)*30.));
  float c2 = smoothstep(0.1, 0.95, sin(t*PI + dot(v2,l)*30.) )/2.;

  c += c2;

  // c += circle(p-=1.5, 0.7);
  // gl_FragColor = vec4(vec3(c), 1.);







  i = c;
  gl_FragColor = vec4(vec3(i),1.);
}