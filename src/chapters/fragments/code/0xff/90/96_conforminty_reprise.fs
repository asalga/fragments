// 96 - "Conformity Reprise"
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

const int MaxStep = 100;
const int MaxShadowStep = 100;
const float MaxDist = 1000.;
const float Epsilon = 0.0001;

mat4 rotateY(float a){
  float c = cos(a);
  float s = sin(a);
  return mat4(c,0,-s,0,0,1,0,0,s,0,c,0,0,0,0,1);
}

mat3 viewMatrix(vec3 eye, vec3 center, vec3 up) {
  vec3 f = normalize(center - eye);
  vec3 s = normalize(cross(f, up));
  vec3 u = cross(s, f);
  return mat3(s, u, -f);
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
  float t = u_time * 2.;
  float modTime = 1.;

  float c0pos = mod(t, modTime);

  float c0 = cubeSDF(p+vec3(0,0.00 + c0pos,0), vec3(.5));
  float c1 = cubeSDF(p+vec3(0,1.01 + c0pos,0), vec3(.5));
  float c2 = cubeSDF(p+vec3(0,2.01 + c0pos,0), vec3(.5));

  float c3pos = mod(5. -t*5., 5.);
  float c3 = cubeSDF(p+ vec3(0,-c3pos,0), vec3(.5));

  float i = min(c0,c1);
  i = min(i,c2);
  i = min(i,c3);
  return i ;
}

// float ao(vec3 p, vec3 n)
// {
//   float stepSize = .02;
//   float t = stepSize;
//   float oc = 0.;
//   for(int i = 0; i < 5; ++i)
//   {
//     float d = sdScene(p+n*t);
//     oc += t - d;
//     t += stepSize;
//   }

//   return clamp(oc, 0., 1.);
// }

// float ambientOcclusion(vec3 p, float dist, vec3 n){
//   // March along the normal, get new sample point
//   vec3 newPoint = p + n*.001;
//   float testpoint = sdScene(newPoint);

//   // if newDistance > d were at open convex
//   // if( testpoint > dist ){
//     // return 0.;//testpoint - dist;
//   // }
//   return 1.-((testpoint+dist)*.025);
//   // return 1.;
// }

vec3 estNormal(vec3 p){
  vec3 n;
  vec3 e = vec3(Epsilon,0,0);
  n.x = sdScene(p + e.xzz) - sdScene(p - e.xzz);
  n.y = sdScene(p + e.zxz) - sdScene(p - e.zxz);
  n.z = sdScene(p + e.zzx) - sdScene(p - e.zzx);
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
  float power = 20.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  
  float nDotL = max(dot(n,lightRayDir), 0.);

  float ambient = .4;
  float diffuse = (nDotL*power) / d;
  
  // if(nDotL > .55){return 0.;}

  return ambient + diffuse;
}

void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float i = 0.;
  float t = u_time * 1. + 1.;
  vec3 lightPos = vec3(3);

  vec3 center = vec3(0);
  vec3 up = vec3(0,1,0);
  vec3 eye = vec3(cos(t)*3., .2, sin(t)*3.);
  vec3 ray = rayDirection(100.0, u_res, gl_FragCoord.xy);

  // float mv = u_time*1.1;
  float mv = 0.;

  // mv = mod(mv, 1.);

  eye.y += mv;
  center.y += mv;

  mat3 viewWorld = viewMatrix(eye, center, up);
  vec3 worldDir = viewWorld * ray;
  float d = rayMarch(eye, worldDir);
  
  if(d < MaxDist){
    // float visibleToLight = shadowMarch(eye+ray*(d-0.001), lightPos);

    vec3 point = eye+worldDir*d;  
    vec3 n = estNormal(point);
    float lambert = phong(point, n, lightPos);

    // float ao = ao(point, n);
    i = lambert;// * (1.-ao)*0.8;

    // if(visibleToLight == 0.){
      // i = .3;
    // }
  }

  gl_FragColor = vec4(vec3(i),1);
}