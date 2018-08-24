precision mediump float;

uniform sampler2D u_t0;
uniform vec2 u_res;
uniform float u_time;

const int CNT = 30;
const float PI = 3.141592658;

float random (vec2 st) {
  return fract(sin(dot(st.xy,vec2(22.9898,78.233)))*43758.5453123);
}

float rand(float i, float seed){
  return fract(sin(i)*seed);
}

mat2 r2d(float a){
  return mat2(cos(a),-sin(a),sin(a), cos(a));
}
// Find the closest point in the pts array.
float voronoi(in vec2 p, in vec2 pts[CNT], out vec2 test, in vec2 vel[CNT], out float d, out float border){
  float dist = 1.; // MAX DISTANCE
  float col = 0.;

  vec3 res = vec3(1.);
  float id = 0.;

  for(int it = 0; it < CNT; ++it){
    vec2 diff = p - pts[it];
    float testLength = dot(diff,diff);

    if(testLength < dist){
      dist = testLength;
      d = dist;

      // float c = cls[it].r * 0.2989 + cls[it].g * 0.5870 + cls[it].b * 0.1140;
      id = float(it);
      test = sin(u_time/2.) * (vel[it])/5.;
    }
  }
  return id;
}

void main() {
  vec2 p = (gl_FragCoord.xy / u_res);//*2.-1.;
  p.y = 1.0 - p.y;
  float id;
  vec2 sites[CNT];
  vec2 vel[CNT];
  float t = u_time;

  // populate with site pos
  for(int i = 0; i < CNT; i++){
    float fi = float(i);
    // id[i] = vec2(rand(fi, 64134.), rand(fi, 39598.));

    // sites[i] = vec2(  (random(vec2(fi))+1.)/2.,
                      // (random(vec2(fi))+1.)/2.);

    // sites[i] = vec2(  (rand(fi, 31644.)+1.)/2.,
    //                   (rand(fi, 45144.)+1.)/2.);

    sites[i] = vec2(rand(fi, 3144.), rand(fi, 145144.));
    vel[i]   = vec2(rand(fi, 83634.), rand(fi, 39598.))*2. -1.;
    vel[i] /= 10.;

    sites[i] += vel[i] * t;
    vec2 screenIdx = floor(mod(sites[i], 2.));// 0..1
    vec2 dir = screenIdx * 2. - 1.;

    vec2 finalPos = vec2((1.-screenIdx) + dir * mod(sites[i], 1.));
    // sites[i] = (finalPos-0.5)*2.;
  }

  // Now find which site this fragment is closest to.
  // Every fragment is assigned a 'shared' id for the site
  // that it resides in.
  vec2 test;
  float dist;
  float border= 0.;
  id = voronoi(p, sites, test, vel, dist, border);
  // test = vec2(0.);

  // offset texture
  // p.x += fract(id/float(CNT)/20.) + test.x*2.;
  // p.y += fract(id/float(CNT)/20.) + test.y*2.;

  // p.x +=  test.x*1.;// + dist*110.;
  // p.y +=  test.y*1.;
  // p.y += fract(id/float(CNT)/20.) + test.y*2.;

  // debug
  // gl_FragColor = vec4(vec3(id),1);
  float testcol = 1.+id;//vec4(vec3(id),1);
  // float c = fract(res.z + t/5.);

  p -= vec2(0.5);
  p *= r2d( sin(u_time)/10. * (id/float(CNT))*2.-1.  * PI);
  p += vec2(0.5);
  p.y = 1.-p.y;
  p.x = 1.- p.x;

  vec4 col = texture2D(u_t0, p);

  if(p.x > 1. || p.x < 0. || p.y < 0. || p.x > 1.){
    col = vec4(1.);
  }

  gl_FragColor = col;
}