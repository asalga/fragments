// 89 - AO Test
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

#define PI 3.141592658
#define TAU (PI*2.)

#define OFFSET PI/4.



float valueNoise(vec2 p){
  #define Y_SCALE 45343.
  #define X_SCALE 37738.  
  float x = p.x * X_SCALE;
  float y = p.y * Y_SCALE;  
  return fract( sin(x+y) * 23454.);
}

float smoothValueNoise(vec2 p){
  vec2 lv = fract(p);
  lv = smoothstep(0.,1.,lv);
  vec2 id = floor(p);
  float bl = valueNoise(vec2(id));
  float br = valueNoise(vec2(id)+vec2(1,0));
  float b = mix(bl,br,lv.x);

  float tl = valueNoise(vec2(id)+vec2(0,1));
  float tr = valueNoise(vec2(id)+vec2(1,1));
  float t = mix(tl,tr,lv.x);

  return mix(b,t,lv.y);
}


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
  vec4 np = vec4(p,1) * rotateY(0.14 + u_time*1.);
  // vec4 np = vec4(p,1);

  vec3 mp = np.xyz;
  if( fract(np.z) <= .27){
    // mp  = mod(np.xyz, vec3(1,0,1));
  }




float building = smoothValueNoise(np.xy*10.)/10. + cubeSDF(np.xyz, vec3(.80, 1,.81));
return building;



  

  // float c = cubeSDF(mp+ vec3(0,0,0.), vec3(.51, .151, .5));

  // float building = cubeSDF(np.xyz, vec3(.80, 1,.81));

  // float res = max(building,-c);

  // return res;
}

vec3 estimateNormal(vec3 v){
  vec3 n;
  n.x = sdScene(vec3(v.x + Epsilon, v.y, v.z)) - sdScene( vec3(v.x - Epsilon, v.y, v.z));
  n.y = sdScene(vec3(v.x, v.y + Epsilon, v.z)) - sdScene( vec3(v.x, v.y - Epsilon, v.z));
  n.z = sdScene(vec3(v.x, v.y, v.z + Epsilon)) - sdScene( vec3(v.x, v.y, v.z - Epsilon));
  return normalize(n);
}


// vec3 estimateNormal(vec3 p) {
//   return normalize(vec3(
//     sdScene(vec3(p.x + Epsilon, p.y, p.z)) - sdScene(vec3(p.x - Epsilon, p.y, p.z)),
//     sdScene(vec3(p.x, p.y + Epsilon, p.z)) - sdScene(vec3(p.x, p.y - Epsilon, p.z)),
//     sdScene(vec3(p.x, p.y, p.z  + Epsilon)) - sdScene(vec3(p.x, p.y, p.z - Epsilon))
//   ));
// }

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

  float ambient = .9;
  float diffuse = (nDotL*power) / d;
  
  return 0. + diffuse;
}

void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float i = 0.;
  float t = u_time*1. + 31.;
  vec3 eye = vec3(0, 0, 3.);

  vec3 ray = rayDirection(105.0, u_res, gl_FragCoord.xy);
  vec3 dirLight = normalize(vec3(cos(t*TAU),0,sin(t*TAU) ));
  float ambient = 0.3;
  float s = 10.;
  float d = rayMarch(eye, ray);

  vec3 lightPos = vec3(0, 7., 3.5);
  
  vec3 point = eye+ray*d;

  if(d < MaxDist){
    // float visibleToLight = shadowMarch(eye+ray*(d-0.001), lightPos);
  
    vec3 n = estimateNormal(point);
    // if(visibleToLight == 1.){
      i += phong(point, n, lightPos)+ .3;
    // }
    // else{
      // i += .3;
    // }
  }

  gl_FragColor = vec4(vec3(i),1);
}