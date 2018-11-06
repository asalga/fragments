// 216
precision mediump float;

uniform vec2 u_res;
uniform float u_time;
const float NumSections = 400.;

float valueNoise(vec2 p){
  return fract(sin(p.x * 7384. + p.y * 99331.)* 303412.);
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

vec2 getSection(float x, float multiplier){
  return vec2(floor((x*multiplier)*NumSections)/NumSections, 0.);
}

float remap(float v, float low1, float high1, float low2, float high2){
  return low2 + (v - low1) * (high2 - low2) / (high1 - low1);
}

float sampleChecker(vec2 c) {
  float col;
  float sz = 0.125;

  float x = step(mod(c.x,sz), sz/2.);
  float y = step(mod(c.y,sz), sz/2.);

  if(x == y){return 1.;}
  return x*y;
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res;
  float i;
  float turnTime = 2.5;
  float t = u_time * 1. * (1./turnTime);
  float turn = floor(t) * 20.;
  t = fract(t);

  float porabolaInfluence = (1.5 - pow(p.x*2. -1. , 2.))*2.;

  vec2 accSection = getSection(p.x + turn, 1.);
  float a = smoothValueNoise(accSection);
  a = remap(a, 0., 1., .8, 1.) * 4.;
  float deltaV = a * porabolaInfluence * t;

  vec2 velSection = getSection(p.x + turn, 10.);
  velSection.x = remap(velSection.x, 0., 1., 0.2, 1.);
  float v = smoothValueNoise(velSection) * 1. + deltaV;

  float d = v*t;
  turn = step(turnTime, mod(u_time, turnTime*2.));


  float c = step(p.y, (1. - d));

  // i = c;

  if(turn == 0.){
    // i = 1.-c;
  }

  p.y -= t*0.25;

  p.y = 1.-p.y;

  i = fract(t) * sampleChecker(p);


  if(d-p.y < 0.){
    i = sampleChecker(vec2(p.x, p.y-d));
  }

  // i = step(p.y, 1.-d);

  // i = sampleChecker(p);
  // i = sampleChecker(vec2(c, p.x));

  gl_FragColor = vec4(vec3(i),1);
}