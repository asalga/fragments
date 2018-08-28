// flow
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

const float PI = 3.141592658;
const float TAU = PI*2.;

float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

mat2 r2d(float a){
  return mat2(cos(a),-sin(a),sin(a),cos(a));
}

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
  float i;
  float t = u_time;
  vec2 trans = vec2(t/4., 0.);

  float c = 0.5;
  vec2 rc = mod(p+trans, vec2(c))- vec2(c/2.);

  // float te = floor( sin(u_time)*100.);
  float te = 195.;// should be 5, but use  to 'tear'
  vec2 fp = floor((p+trans)*te)/te;
  float n = smoothValueNoise(fp+vec2(t,t*1.)) * TAU;

  // rc *= r2d(n/2.);
  float rot =  (fp.y+1.)/20. + t/2.;
  if(u_time < 1.){
    rot = 0.;
  }

  rc *= r2d( n/2.);

  vec2 line = vec2(0.07, 0.013) * 2.25;

  // cool fade effect, but still need to polish it.
  // for(int it = 10; it > 1; --it){
  //   vec2 rc2 = mod(p+trans, vec2(c))- vec2(c/2.);
  //   float fit = float(it);
  //   //(1./pow(9.91, fit))
  //   float n2 = smoothValueNoise(fp-vec2(t -(1./pow(100.91, fit)) , t)) * TAU;
  //   rc2 *= r2d(n2/2.);
  //   // i += step(sdRect(rc2, line ), 0.) * 0.019;
  //   // i += step(sdRect(rc2, line.yx ), 0.) * 0.019;
  // }


  i += 1./(sdRect(rc, line.xy)*500.);
  i += 1./(sdRect(rc, line.yx)*500.);
  i = clamp(i,0.,1.);

  i += step(sdRect(rc, line.xy*.75), 0.);
  i += step(sdRect(rc, line.yx*.75), 0.);

  // white bk makes it look like we have ao.
  // i = 1.- i;

  // i *= 1.5/abs( (mod(p.x*p.y, 0.5)-0.25) *200.)*70.;

  gl_FragColor = vec4(vec3(i),1.);
}