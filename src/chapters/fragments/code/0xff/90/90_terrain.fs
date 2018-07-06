// 91 - Terrain
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

#define PI 3.141592658
#define TAU (PI*2.)

const int MaxStep = 400;
const int MaxShadowStep = 100;
const float MaxDist = 1000.;
const float Epsilon = 0.00001;

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

float sdScene(vec3 p){
  vec3 np = p;
  float t = u_time;

  vec2 nLookup = vec2(np.xz + vec2(10, -t));
  float n;
  n += smoothValueNoise(nLookup*2.) * 0.5000;
  n += smoothValueNoise(nLookup*4.) * 0.2500;
  n += smoothValueNoise(nLookup*8.) * 0.1250;
  n/= 1.875;

  vec2 nLookup2 = vec2(np.xz + vec2(2, -t));
  float n2;
  // n2 += smoothValueNoise(nLookup2*2.) * 0.500;
  // n2 += smoothValueNoise(nLookup2*4.) * 0.250;
  n2 += n + smoothValueNoise(nLookup2*8.) * 0.125;
  // // n2/= 2.061;
  n2/= 1.875;

  // n = floor(n*50.)/50.;

  float terrain = -n2 + cubeSDF(np.xyz  + vec3(0, -.851, 0), vec3(10., .01 ,10));
  float terrain2 = -n + cubeSDF(np.xyz + vec3(0, +.851, 2), vec3(20., .01 ,20));

  return min(terrain , terrain2);
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
  float power = 5.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  
  float nDotL = max(dot(n,lightRayDir), 0.);

  float ambient = .2;
  float diffuse = (nDotL*power) / d;
  
  return ambient + diffuse;
}

void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float i = 0.;
  float t = u_time;
  vec3 eye = vec3(0, 0, 0);
  vec3 ray = rayDirection(85.0, u_res, gl_FragCoord.xy);
  vec3 lightPos = vec3(0, 0, -4.);

  float test = rayMarch(eye, ray);
  vec3 point = eye+ray*(test+0.001);
  
    vec3 n = estimateNormal(point);
  
      i += phong(point, n, lightPos);

  float fog = 1./ pow( test, 1.8);
  i *= fog;

  gl_FragColor = vec4(vec3(i),1);
}