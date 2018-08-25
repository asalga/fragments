// 145 "In space"

// asteroid verts:
// github.com/asalga/Asteroids/blob/master/Asteroid.pde

precision highp float;

const int CNT = 26;
const float PI = 3.141592658;
const float TAU = PI*2.;

uniform vec2 u_res;
uniform float u_time;
uniform float u_epochTime;
uniform vec3 u_mouse;


uniform float u_type0[CNT];
uniform float u_type1[CNT];
uniform float u_type2[CNT];


// float valueNoise (float st, float scale) {
  // return fract(sin(st*798.233)*43758.5453123);
// }



float valueNoise(float i, float scale){
  float n = sin(i*scale);
  return fract(n);
}

float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

float sdCircle(vec2 p, float r){
  return length(p)-r;
}

float capsule(vec2 p, vec2 a, vec2 b, float r) {
    vec2 pa = p - a, ba = b - a;
    float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
    return length( pa - ba*h ) - r;
}

mat2 r2d(float a){
  return mat2(cos(a),sin(a),-sin(a),cos(a));
}

float asteroid(vec2 p, float a[CNT]){
  float sz = 80.;// + (sin(u_time)+1.)/2. * 1050.;
  const float thi = 0.008;
  float sc = .125;

  vec2 p1 = floor(p*sz)/sz;
  // p1 *= 2.0;

  float c = capsule(p1, vec2(a[0] * sc, a[1] * sc), vec2(a[2] * sc, a[3] * sc), thi);
  float d = step(c,0.);

  for(int it = 4; it < CNT; it += 2){
    c = capsule(p1,  vec2(a[it-2] * sc, a[it-1] * sc), vec2(a[it+0] * sc, a[it+1] * sc), thi);
    d += step(c, 0.);
  }

  return d;
}

void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float i;
  float t = u_time/2.;
  float sz = 30.;

  const int NUM = 8;
  vec2 vel[NUM];
  vec2 pos[NUM];
  float type[NUM];
  float rot[NUM];

  for(int it=0; it < NUM; ++it){
    float fit = float(it);

    vec2 v = vec2(valueNoise((fit+1.) * (fit+1.), fit*198323.712983),
                  valueNoise((fit+1.) * (fit+1.), fit*128392.234923));
    // v = vec2(1.);
    vel[it] = (v*2.-1.)*1.;

    vec2 p = vec2(valueNoise(fit*fit, fit*198323.523),
                  valueNoise(fit*fit, fit*9692033.623));
    pos[it] = p*2.-1.;

    // pos[it] = vec2(0.5, 0.);//vel[it];
    // float y = u_mouse.y*2.-1.;
    // pos[it] = vec2(-u_mouse.x*2.-1., y );
    type[it] = mod(fit,4.);
    rot[it] = fit*TAU;
  }

  // render
  for(int it = 0; it < NUM; ++it){
    vec2 _pos = p + pos[it] + (vel[it]*t);

    vec2 c = mod(_pos, vec2(2.)) - vec2(1.);

    c *= r2d(rot[it]*t/40.);

    if(type[it] == 0.)
      i += asteroid(c, u_type0);
    else if(type[it] == 1.)
      i += asteroid(c, u_type1);
    else
     i += asteroid(c, u_type2);
  }

  i -= step(mod(gl_FragCoord.y, 4.), 2.) * .5;

  i *= (sin(u_time*100.)+1.)/2.;
  gl_FragColor = vec4(vec3(i),1);
}

// test
// for(int it = 0; it < 210; it++){
//   float fit = float(it)/10.;
//   float f = fract(t + 0.410);
//    d += step(capsule(p - vec2(f,0.720) * fit,
//                         vec2(0.120,0.070),
//                           vec2(-0.160,-0.190), 0.01), 0.0);
//}