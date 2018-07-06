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
  vec4 np = vec4(p,1) * rotateY(3.5 + u_time*0.);

  float x = .25;
  float y = 0.5;
  vec3 mp = mod(np.xyz, vec3(x, y, 1.));
    //vec3(.5, 0.25, 1.));
  mp -= vec3(x/2., y/2., 0.5);



  float t = u_time;
  float jut = .1 + ((sin(t*3.+p.y*.7)+1.)/2.)*.3;
  float windows = cubeSDF(mp, vec3(.1, 0.1, jut));

  float building = cubeSDF(np.xyz, vec3(.5, 100., .45 ));

  return max(building, -windows);
}


float ao(vec3 p, vec3 n)
{
  float stepSize = .02;
  float t = stepSize;
  float oc = 0.;
  for(int i = 0; i < 5; ++i)
  {
    float d = sdScene(p+n*t);
    oc += t - d;
    t += stepSize;
  }

  return clamp(oc, 0., 1.);
}

float ambientOcclusion(vec3 p, float dist, vec3 n){
  // March along the normal, get new sample point
  vec3 newPoint = p + n*.001;
  float testpoint = sdScene(newPoint);

  // if newDistance > d were at open convex
  // if( testpoint > dist ){
    // return 0.;//testpoint - dist;
  // }
  return 1.-((testpoint+dist)*.025);
  // return 1.;
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
  float power = 15.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  
  float nDotL = max(dot(n,lightRayDir), 0.);

  float ambient = .15;
  float diffuse = (nDotL*power) / d;
  
  return ambient + diffuse;
}

void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float i = 0.;
  float t = u_time;
  vec3 eye = vec3(.5, -u_time*0., 2.);
  vec3 ray = rayDirection(100.0, u_res, gl_FragCoord.xy);
  float d = rayMarch(eye, ray);

  vec3 lightPos = vec3(2., 0. + abs(sin(u_time*1.)*0.), 4.);
  
  vec3 point = eye+ray*d;  

  if(d < MaxDist){
    float visibleToLight = shadowMarch(eye+ray*(d-0.001), lightPos);

    vec3 n = estimateNormal(point);
    float lambert = phong(point, n, lightPos);

    float ao = ao(point, n);

    i += lambert * (1.-ao)*0.8;

    if(visibleToLight == 0.){
      i = .3;
    }
  }

  gl_FragColor = vec4(vec3(i),1);
}