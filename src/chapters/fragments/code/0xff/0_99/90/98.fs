// 91 - rings
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

#define PI 3.141592658
#define TAU (PI*2.)

const int MaxStep = 170;
const int MaxShadowStep = 100;
const float MaxDist = 100.;
const float Epsilon = 0.001;

float sdTorus( vec3 p, vec2 t){
  vec2 q = vec2(length(p.xz)-t.x,p.y);
  return length(q)-t.y;
}

vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
  vec2 xy = fragCoord - size / 2.;
  float z = size.y/tan(radians(fieldOfView)/2.);
  return normalize(vec3(xy, -z));
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


float m(float t, float md, float s){
  float ti = t;
  return step( mod(t, md) , s) * (1./s)*2. * PI * smoothstep(0.,1.,fract(ti));
}

float sdScene(vec3 p){
  float w = 0.2;
  float s = 5.0;
  float t = u_time * 5.;

  float sz0 = .6 + sin(PI/2. + t) * 0.3;
  float sz1 = .6 + sin(PI/2. + t + PI) * .3;

  vec3 np0 = p + vec3(0, sin(t), 0);
  vec3 np1 = p + vec3(0, sin(t - PI), 0);
  
  float torus0 = sdTorus(np0, vec2(.25 * s + sz0, w));
  float torus1 = sdTorus(np1, vec2(.25 * s + sz1, w));

  float i = min(torus0, torus1);
  return i;
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

  float ambient = .1;
  float diffuse = (nDotL*power) / d;
  
  return ambient + diffuse;
}

void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float i = 0.;
  float t = u_time;
  vec3 ray = rayDirection(100.0, u_res, gl_FragCoord.xy);

  vec3 center = vec3(0,0,0);
  vec3 up = vec3(0,1,0);
  vec3 eye = vec3(-5, 4, 5.);
  mat3 viewWorld = viewMatrix(eye, center, up);
  vec3 worldDir = viewWorld * ray;

  float d = rayMarch(eye, worldDir);
  vec3 lightPos = vec3(4, 2, 0);
  
  vec3 point = eye+worldDir*d;  

  float ambient = 0.1;

  if(d < MaxDist){
    float visibleToLight = shadowMarch(eye+worldDir*(d-0.001), lightPos);
    vec3 n = estimateNormal(point);
    float lambert = phong(point, n, lightPos);

    if(visibleToLight > .8){
      i += lambert;
    }
    else {
      i = 0.1; 
    }
  }

  gl_FragColor = vec4(vec3(i),1);
}