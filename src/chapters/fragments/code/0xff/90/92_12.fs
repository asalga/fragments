// 92 - "12 Apartment floors"
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

#define PI 3.141592658
#define TAU (PI*2.)

#define OFFSET PI/4.

const int MaxStep = 170;
const int MaxShadowStep = 100;
const float MaxDist = 1000.;
const float Epsilon = 0.0001;
const float NormEpsilon = 0.01;

mat4 rotateY(float a){
  float c = cos(a);
  float s = sin(a);
  return mat4(c,0,-s,0,0,1,0,0,s,0,c,0,0,0,0,1);
}


vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
  vec2 xy = fragCoord - size / 2.;
  float z = size.y/tan(radians(fieldOfView)/2.);
  return normalize(vec3(xy, -z));
}

float sdSphere(vec3 p, float r){
  return length(p)-r;
}

float cubeSDF(vec3 p, vec3 sz) {
  vec3 d = abs(p) - sz;
  float insideDistance = min(max(d.x, max(d.y, d.z)), 0.);
  float outsideDistance = length(max(d, 0.));
  return insideDistance + outsideDistance;
}

float sdScene(vec3 p){
  vec4 np = vec4(p,1) * rotateY(4. + u_time*0.);

  float x = .25;
  float y = 0.5;
  vec3 mp = mod(np.xyz*1., vec3(x, y, 1.));
    //vec3(.5, 0.25, 1.));
  mp -= vec3(x/2., y/2., 0.5);

  float windows = cubeSDF(mp, vec3(.1, 0.1, .2));

  float building = cubeSDF(np.xyz, vec3(.5, 1., .45));

  // if(mod(u_time, 4.) < 2.){
    return max(building, -windows);
  // }
  
  return windows;

  // return building;
}

vec3 estimateNormal(vec3 v){
  vec3 n;
  n.x = sdScene(vec3(v.x + Epsilon, v.y, v.z)) - sdScene( vec3(v.x - Epsilon, v.y, v.z));
  n.y = sdScene(vec3(v.x, v.y + Epsilon, v.z)) - sdScene( vec3(v.x, v.y - Epsilon, v.z));
  n.z = sdScene(vec3(v.x, v.y, v.z + Epsilon)) - sdScene( vec3(v.x, v.y, v.z - Epsilon));
  return normalize(n);
}

float rayMarch(vec3 ro, vec3 rd){
  float s = 0.;
  for(int i = 0; i < MaxStep; ++i){
    vec3 v = ro + (rd*s);
    float dist = sdScene(v);

    if(dist < Epsilon){
      return s;
    }
    s += dist;

    if(s >= MaxDist){
      return MaxDist;
    }
  }
  return MaxDist;
}

float shadowMarch(vec3 point, vec3 lightPos){

  vec3 pToLight = lightPos-point;
  vec3 rd = normalize(pToLight);
  vec3 ro = point;

  float s = 0.;
  for(int i = 0; i < MaxShadowStep; ++i){
    vec3 v = ro + (rd*s);
    float dist = sdScene(v);

    if(dist < Epsilon){
      return 0.;
    }
    s += dist;

    if(s >= MaxDist){
      return 1.;
    }
  }
  return 1.;
}

float phong(vec3 p, vec3 n, vec3 lightPos){
  vec3 pToLight = vec3(lightPos - p);
  float power = 25.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  
  float nDotL = max(dot(n,lightRayDir), 0.);

  float ambient = .3;
  float diffuse = (nDotL*power) / d;
  
  return ambient + diffuse;
}

void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float i = 0.;
  float t = u_time;
  vec3 eye = vec3(.5, 0, 2.);

  vec3 ray = rayDirection(90.0, u_res, gl_FragCoord.xy);
  float ambient = 0.3;
  float s = 10.;
  float d = rayMarch(eye, ray);

  vec3 lightPos = vec3(0, 5. + sin(u_time)*1., 4. );
  
  vec3 point = eye+ray*d;

  if(d < MaxDist){
    float visibleToLight = shadowMarch(eye+ray*(d-0.001), lightPos);
  

    vec3 n = estimateNormal(point);
    float lambert = phong(point, n, lightPos);

    if(visibleToLight == 1.){
      i += lambert;
    }
    else{
      i += lambert * 0.4;
    }
  }

if(mod(u_time, 2.) > 1.){
  //i *= 1./pow(d, 1.1);
}

  gl_FragColor = vec4(vec3(i),1);
}