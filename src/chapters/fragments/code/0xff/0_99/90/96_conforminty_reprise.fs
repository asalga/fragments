// 96 - "Conformity Reprise"
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

const int MaxStep = 100;
const int MaxShadowStep = 100;
const float MaxDist = 1000.;
const float Epsilon = 0.0001;

// https://gist.github.com/girish3/11167208
float eob(float t) {
  if ((t/=1.0) < (1./2.75)) {return 1.*(7.5625*t*t);}
  else if (t < (2./2.75)) {return 1.*(7.5625*(t-=(1.5/2.75))*t + .75);}
  else if (t < (2.5/2.75)) {return 1.*(7.5625*(t-=(2.25/2.75))*t + .9375);}   
  return 1.*(7.5625*(t-=(2.625/2.75))*t + .984375);
}

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
  float t = u_time * 2.1;
  float modTime = 1.;
  float sz = .485;

  float c0pos = mod(t, modTime);

  float c0 = cubeSDF(p+vec3(0, 0. + c0pos,0), vec3(sz));
  float c1 = cubeSDF(p+vec3(0, 1. + c0pos,0), vec3(sz));
  float c2 = cubeSDF(p+vec3(0, 2. + c0pos,0), vec3(sz));
  float c3 = cubeSDF(p+vec3(0, 3. + c0pos,0), vec3(sz));

  float dist = 5.;
  float c3pos = mod(dist -t*dist, dist);
  vec3 objPos = vec3(0,-c3pos,0);

  float s = sdSphere(p+vec3(0, eob(c0pos*.5),0), .5);
  float _c4 = sdSphere(p + vec3(0,-eob( (objPos.y/dist) ),0) , sz);

  float i;
  i = mix(s, c0, c0pos);
  i = min(i,c1);
  i = min(i,c2);
  i = min(i,c3);
  i = min(i,_c4);
  return i;
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

  float ambient = .1;
  float diffuse = (nDotL*power) / d;
  return ambient + diffuse;
}

void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float i = 0.;
  float t = u_time*0.04;
  vec3 lightPos = vec3(2,4,2);

  vec3 center = vec3(0,-.25, 0);
  vec3 up = vec3(0,1,0);
  vec3 eye = vec3(cos(t)*3., .5, sin(t)*3.);
  vec3 ray = rayDirection(100.0, u_res, gl_FragCoord.xy);


  float mv = 0.;
  eye.y += mv;
  center.y += mv;

  mat3 viewWorld = viewMatrix(eye, center, up);
  vec3 worldDir = viewWorld * ray;
  float d = rayMarch(eye, worldDir);
  
  if(d < MaxDist){
    float visibleToLight = shadowMarch(eye+worldDir*(d-0.001), lightPos);

    vec3 point = eye+worldDir*d;  
    vec3 n = estNormal(point);
    float lambert = phong(point, n, lightPos);
    float lambert2 = phong(point, n, vec3(-lightPos.x, lightPos.y, -lightPos.z));

    if(visibleToLight == 1.){
      // float ao = ao(point, n);
      i = lambert2 + lambert;// * (1.-ao)*0.8;
    }
    else{  
      i =  .25;
      //lambert + lambert2 - .2;
    }
  }

  gl_FragColor = vec4(vec3(i),1);
}