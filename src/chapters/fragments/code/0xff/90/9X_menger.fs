// 92 - Menger
precision mediump float;

uniform vec2 u_res;
uniform float u_time;
uniform vec3 u_mouse;

#define PI 3.141592658

const int MaxStep = 128;
const int MaxShadowStep = 200;
const float MaxDist = 200.;
const float Epsilon = 0.0001;

mat4 rotateY(float a){
  float c = cos(a);
  float s = sin(a);
  return mat4(c,0,-s,0,0,1,0,0,s,0,c,0,0,0,0,1);
}

vec3 rep(vec3 p, float r){
  return mod(p,vec3(r)) - 0.5 * r;
}
vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
  vec2 xy = fragCoord - size / 2.;
  float z = size.y/tan(radians(fieldOfView)/2.);
  return normalize(vec3(xy, -z));
}

float cubeSDF(vec3 p, vec3 sz) {
  vec3 d = abs(p) - sz;
  float insideDistance = min(max(d.x, max(d.y, d.z)), 0.);
  float outsideDistance = length(max(d, 0.));
  return insideDistance + outsideDistance;
}
float sdSphere(vec3 p, float r){
  return length(p)-r;
}


mat3 viewMatrix(vec3 eye, vec3 center, vec3 up) {
  vec3 f = normalize(center - eye);
  vec3 s = normalize(cross(f, up));
  vec3 u = cross(s, f);
  return mat3(s, u, -f);
}


float cross(vec3 p){
  // float cX = cubeSDF(p, vec3(100., 1., 1.));
  // float cY = cubeSDF(p, vec3(1.,   100., 1.));
  // float cZ = cubeSDF(p, vec3(1.,   1., 100.));
  float sz = 1.;
  float sc = 0.1;

  float cX = cubeSDF(p, vec3(sz, sc, sc));
  float cY = cubeSDF(p, vec3(sc, sz, sc));
  float cZ = cubeSDF(p, vec3(sc, sc, sz));
  return min(min(cX, cY), cZ);
}

float sdScene(vec3 p){
  float sz = 0.25;
  float thrd = 1./3.;

  float s = 1.;

  float c0 = cross(p);  //, 1., sz * thrd);

  s = 3.;

  // float c1 = cross( mod(p*4.,2.)-1.,  sz, 0.1);

  float c1 = cross( mod(p*9., 2.)-1. )/9.;
  // -1., sz*10., .125*sz);

  // float c2 = cross(rep(p, vec3(1./24.)), sz, 0.009);
  // float c1 = cross(np, 0.33,        0.0);
  float mainCube = cubeSDF(p , vec3( sz-0.11));//.5 = full
  
  return c1;
  return mainCube;
  // return max(max(mainCube, -c0), -c1);
  // return max(mainCube, -c1);
  // return max(mainCube, -c1);
  // return max(mainCube, -min(c0,c1));
}

vec3 estimateNormal(vec3 v){
  vec3 n;
  n.x = sdScene(vec3(v.x + Epsilon, v.y, v.z)) - sdScene( vec3(v.x - Epsilon, v.y, v.z));
  n.y = sdScene(vec3(v.x, v.y + Epsilon, v.z)) - sdScene( vec3(v.x, v.y - Epsilon, v.z));
  n.z = sdScene(vec3(v.x, v.y, v.z + Epsilon)) - sdScene( vec3(v.x, v.y, v.z - Epsilon));
  return normalize(n);
}

float rayMarch(vec3 ro, vec3 rd){
  float s = 0.;//14.;

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
  float power = 75.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  
  float nDotL = max(dot(n,lightRayDir), 0.);

  float ambient = 0.2;
  float diffuse = (nDotL*power) /d;
  
  return ambient + diffuse;
}

void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float i = 0.;
  float t = u_time*0.;

  float mx = (u_mouse.x/u_res.x)*10.;
  float my = (u_mouse.y/u_res.y)*10.;
  
  vec3 ray = rayDirection(70.0, u_res, gl_FragCoord.xy);

  
  vec3 up = vec3(0,1,0);
  // vec3 eye = vec3(mx+0.5, my,  -t-2.);
  vec3 eye = vec3(0., my, 1.-t);
  vec3 center = vec3(0., 0., -1.  -t);

  mat3 viewWorld = viewMatrix(eye, center, up);
  vec3 worldDir = viewWorld * ray;
  float dist = rayMarch(eye, worldDir); 

  // vec3 lightPos = eye + vec3(2.,10.,5.);
  vec3 lightPos = eye + vec3(2.,10.,5.);

  // float dist = rayMarch(eye, ray);
  vec3 point = eye+worldDir * dist;
  vec3 n = estimateNormal(point);
  
  i += phong(point, n, lightPos);

  // float fog = 1./pow( dist, .941);
  // i *= fog;

  gl_FragColor = vec4(vec3(i),1);
}