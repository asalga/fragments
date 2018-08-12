// 131 - Voronoi
precision highp float;

uniform vec2 u_res;
uniform vec3 u_mouse;
uniform  float u_time;

const int CNT = 15;

float sdCircle(vec2 p, float r){
  return length(p)-r;
}

float random (vec2 st) {
  return fract(sin(dot(st.xy,vec2(22.9898,78.233)))*43758.5453123);
}

float rand(float i, float seed){
  return fract(sin(i)*seed)*2.-1.;
}

vec2 voronoi(vec2 p, in vec2 pts[CNT]){
  float dist = 1.; // MAX DISTANCE
  float col = 0.;

  vec2 res = vec2(1.);

  for(int it = 0; it < CNT; ++it){
    vec2 diff = p - pts[it];
    float testLength = dot(diff,diff);

    if(testLength < res.x){
      res.y = res.x;
      res.x = testLength;
    }
    else if( testLength < res.y){
      res.y = testLength;
    }

  }

  return sqrt(res);
}



void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  vec2 pos[CNT];
  vec2 vel[CNT];
  float t = u_time;

  // populate with site pos
  for(int i = 0; i < CNT; i++){
    float fi = float(i);
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

  vec2 res = voronoi(p, pos);
  float col = res.x;

  float dis = res.y-res.x;
  dis = 1.-smoothstep(0., 0.05, dis);
  //col = step(0., res.x);

  if(col < 0.025){
    col = 0.;
  }
  col = mod(col - t/6., 0.125);
  //col += res.x;//dis;

  // for(int i = 0; i < CNT; i+ +){
     // col += 1.-smoothstep(0.0, .0001, sdCircle(p-pos[i], 0.01));
  // }


  col *= 2.5;

  gl_FragColor = vec4(vec3(col),1);
}





























