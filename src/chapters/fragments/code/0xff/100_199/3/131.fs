// 131 - Voronoi
// 1) add animation
// 2) add borders
// 3) add colours?
// bonus
precision highp float;

uniform vec2 u_res;
uniform vec3 u_mouse;
uniform  float u_time;

const int CNT = 15;
const float MAX_DIST = 1.;

float random (vec2 st) {
  return fract(sin(dot(st.xy,vec2(22.9898,78.233)))*43758.5453123);
}

float rand(float i, float seed){
  return fract(sin(i)*seed)*2.-1.;
}

float voronoi(vec2 p, in vec2 pts[CNT]){
  float dist = MAX_DIST;
  float col = 0.;
  for(int it = 0; it < CNT; ++it){
    float testLength = length(p - pts[it]);

    if(testLength - dist < 0.1){
      col = 0.;
    }

    if(testLength < dist){
      dist = testLength;
      col = dist;
    }
  }
  return col;
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  vec2 points[CNT];
  vec2 vel[CNT];

  // populate with site points
  for(int i = 0; i < CNT; i++){
    float fi = float(i);
    points[i] = vec2(rand(fi, 13634.), rand(fi, 39598.));
    vel[i] = vec2(rand(fi, 13634.), rand(fi, 39598.));
  }

  float col = voronoi(p, points);

  gl_FragColor = vec4(vec3(col),1);
}