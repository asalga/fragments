// 132 - "Vel Under Fire"
precision highp float;

uniform vec2 u_res;
uniform vec3 u_mouse;
uniform  float u_time;

const int CNT = 20;

float random (vec2 st) {
  return fract(sin(dot(st.xy,vec2(22.9898,78.233)))*43758.5453123);
}

float rand(float i, float seed){
  return fract(sin(i)*seed)*2.-1.;
}

vec3 voronoi(vec2 p, in vec2 pts[CNT], in vec3 cls[CNT]){
  float dist = 1.; // MAX DISTANCE
  float col = 0.;

  vec3 res = vec3(1.);

  for(int it = 0; it < CNT; ++it){
    vec2 diff = p - pts[it];
    float testLength = dot(diff,diff);

    if(testLength < res.x){
      res.y = res.x;
      res.x = testLength;

      float c = cls[it].r * 0.2989 + cls[it].g * 0.5870 + cls[it].b * 0.1140;
      res.z = c;
    }
    else if( testLength < res.y){
      //res.y = pts[it].y;
      res.y = testLength;
    }
  }
  return vec3(sqrt(res.xy), res.z);

  // return vec2(sqrt(res.xy), res.z);
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  vec2 pos[CNT];
  vec2 vel[CNT];
  vec3 col[CNT];
  float t = u_time;

  // populate with site pos
  for(int i = 0; i < CNT; i++){
    float fi = float(i);
    col[i] = vec3(  (rand(fi, 31644.)+1.)/2.,
                    (rand(fi, 45144.)+1.)/2.,
                    (rand(fi, 91244.)+1.)/2.);
    pos[i] = vec2(rand(fi, 64134.), rand(fi, 39598.));
    vel[i] = vec2(rand(fi, 83634.), rand(fi, 39598.));
    vel[i] /= 7.;

    // move pos
    pos[i] += vel[i] * t*2.;
    vec2 screenIdx = floor(mod(pos[i], 2.));// 0..1
    vec2 dir = screenIdx * 2. - 1.;
    vec2 finalPos = vec2((1.-screenIdx) + dir * mod(pos[i], 1.));
    pos[i] = (finalPos-0.5)*2.;
  }

  vec3 res = voronoi(p, pos, col);
  float c = fract(res.z + t/5.);
  gl_FragColor = vec4(vec3(c),1);
}
