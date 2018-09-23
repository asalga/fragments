// 168 - "Saturn's Shadow"
// add more octaves
// move light
//

precision highp float;

uniform vec2 u_res;
uniform float u_time;

const float Epsilon = 0.0001;
const float MaxDist = 400.;
const int MaxSteps = 128;
const float PI = 3.141592658;
const float TAU = PI*2.;
const float HALF_PI = PI*0.5;

const vec3 lightGrey = vec3(1.);
const vec3 darkGrey = vec3(0.4);

const int MaxShadowStep = 200;


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


float sdBox(vec3 p, vec3 sz) {
  vec3 d = abs(p) - sz;
  float insideDistance = min(max(d.x, max(d.y, d.z)), 0.0);
  float outsideDistance = length(max(d, 0.0));
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

float lighting(vec3 p, vec3 n, vec3 lightPos){
  // vec3 pToLight = vec3(lightPos - p);
  // vec3 lightRayDir = normalize(pToLight);


  float nDotL = max(dot(n,lightPos), 0.);
  return nDotL/2.;
}

vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
  vec2 xy = fragCoord - size / 2.0;
  float z = size.y / tan(radians(fieldOfView) / 2.0);
  return normalize(vec3(xy, -z));
}

mat4 r3dY(float a){
  float c = cos(a);
  float s = sin(a);

  return mat4(-c, 0, s,  0,
              0,  1, 0,  0,
              s,  0, c,  0,
              0,  0, 0,  1);
}
mat4 r3dX(float a){
  float c = cos(a);
  float s = sin(a);

  return mat4(1,  0,  0,  0,
              0,  -c, s,  0,
              0,  s,  c,  0,
              0,  0,  0,  1);
}
mat4 r3dZ(float a){
  float c = cos(a);
  float s = sin(a);

  return mat4(c, -s, 0,  0,
              s,  c, 0,  0,
              0,  0, 1,  0,
              0,  0, 0,  1);
}


float sdScene(vec3 p, out float col, out float obj){
  float t = u_time;
  col = 1.;

  float sphere = sdSphere(p + vec3(0, 0.1, 0), .8);
  float sphere1 = sdSphere(p + vec3(.13, -.30, 3.), .15);

  float rings = sdBox(p, vec3(3., 0.01, 3.));
  float ringSub = sdSphere(p, 1.12);
  rings = max(rings, -ringSub);

  obj = 0.;
  if(rings < 0.001){
    obj = 1.;
    float len = length(p.xz*30.);
    float n = smoothValueNoise(vec2(len));
    col = n;
    if(length(p) > 2.0){
      return MaxDist;
    }
  }

  float d = min(sphere, rings);
  return min(d, sphere1);
}


float shadowMarch(vec3 point, vec3 lightPos){

  vec3 pToLight = lightPos-point;
  vec3 rd = normalize(pToLight);
  vec3 ro = point;

  float t = 0.;
  float res = 1.;
  float k = 120.;

  float dum;
  float dum2;
  for(int i = 0; i < MaxShadowStep; ++i){
    float h = sdScene(ro + (rd*t), dum, dum2);

    if(h < Epsilon){
      return 0.;
    }

    res = min(res, k * h/(float(i)*2.) );
    t += h;

    // if(t >= MaxDist){
      // return res;
    // }
  }

  return res;
}

vec3 estimateNormal(vec3 v){
  vec3 n;
  float dum;
  float dum2;
  n.x = sdScene(vec3(v.x + Epsilon, v.y, v.z),dum, dum2) - sdScene( vec3(v.x - Epsilon, v.y, v.z),dum, dum2);
  n.y = sdScene(vec3(v.x, v.y + Epsilon, v.z),dum, dum2) - sdScene( vec3(v.x, v.y - Epsilon, v.z),dum, dum2);
  n.z = sdScene(vec3(v.x, v.y, v.z + Epsilon),dum, dum2) - sdScene( vec3(v.x, v.y, v.z - Epsilon),dum, dum2);
  return normalize(n);
}

float rayMarch(vec3 ro, vec3 rd, out vec3 col, out float obj){
  float s = 0.;
  for(int i = 0; i < MaxSteps; i++){
    vec3 p = ro + rd * s;

    float intensity;
    // float obj;
    float d = sdScene(p, intensity, obj);
    col = vec3(intensity);

    if(d < Epsilon){
      return s;
    }
    s += d;

    if(d > MaxDist){
      return MaxDist;
    }
  }
  return MaxDist;
}

void main(){
  vec2 fc = gl_FragCoord.xy;
  float t = u_time;
  float i;

  // float dist = 3.;
  // vec3 eye = vec3(3.5, 2., 3.5);

  vec3 eye = vec3(3.5, 2., 3.5);
  vec3 center = vec3(0, 0, 0.);

  // t = 0.;
  float cs = cos(t);
  float sn = sin(t);
  vec3 lightPos =  vec3(cs, 0.8, sn);
  // vec3 lightPos = vec3(20., 20. , 0.);
  vec3(cs, 2., sn);
  vec3 up = vec3(0, 1, 0.5);

  mat3 viewWorld = viewMatrix(eye, center, up);
  vec3 ray = rayDirection(90., u_res, fc);

  vec3 worldDir = viewWorld * ray;
  vec3 col;
  float obj;
  float d = rayMarch(eye, worldDir, col, obj);
  vec3 v = eye + worldDir*d;

  if(d < MaxDist){
    vec3 n = estimateNormal(v);
    float lights = lighting(v, n, lightPos);
    i = lights * col.x;

    if(obj == 1.){
      i = col.x;
    }

    // float visibleToLight = shadowMarch(eye+ray*(d-0.0001), lightPos);
    // if(visibleToLight == 0.){
    //   i = 0.;
    // }
    // vec3 n = estimateNormal(point);
    // float l = phong(point, n, lightPos);
    // i = l + visibleToLight + ambient;
    // i = lights * visibleToLight * col.x;
  }

  gl_FragColor = vec4(vec3(i), 1);
}