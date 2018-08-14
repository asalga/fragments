// 133 - Menger Walk
precision highp float;

uniform vec2 u_res;
uniform vec3 u_mouse;
uniform  float u_time;

const float Epsilon = 0.0001;
const float E = Epsilon;
const float MaxDist = 100.;
const int MaxSteps = 228;

const float PI = 3.141592658;
const float TAU = PI*2.;
const float HALF_PI = PI*0.5;

float sdBox(vec3 p, vec3 sz) {
  vec3 d = abs(p) - sz;
  float _in = min(max(d.x, max(d.y, d.z)), 0.);
  float _out = length(max(d, 0.));
  return _in + _out;
}

mat3 viewMatrix(vec3 eye, vec3 center, vec3 up) {
  vec3 f = normalize(center - eye);
  vec3 s = normalize(cross(f, up));
  vec3 u = cross(s, f);
  return mat3(s, u, -f);
}

float sdCross( in vec3 p )
{
  float inf = 100.;
  float da = sdBox(p.xyz,vec3(inf,1.0,1.0));
  float db = sdBox(p.yzx,vec3(1.0,inf,1.0));
  float dc = sdBox(p.zxy,vec3(1.0,1.0,inf));
  return min(da,min(db,dc));
}


float sdScene( in vec3 p ){
  float res = sdBox(p, vec3(1.0));

  // mostly from https://iquilezles.org/www/articles/menger/menger.htm
  float s = 1.0;
  for(int it = 0; it < 3; ++it){
    vec3 a = mod(p*s, 2.) - 1.0;
    s *= 3.0;
    vec3 r = abs(1.0 - 3.0*abs(a));

    float da = max(r.x,r.y);
    float db = max(r.y,r.z);
    float dc = max(r.z,r.x);

    float c = (min(da,min(db,dc))-1.0)/s;

    if( c > res ){
      res = c;
    }
  }

  return res;
}

float rayMarch(vec3 ro, vec3 rd){
  float s = 0.;
  for(int i = 0; i < MaxSteps; i++){
    vec3 p = ro + rd * s;

    float d = sdScene(p);

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

float lighting(vec3 p, vec3 n, vec3 lightPos, vec3 eye){
  float ambient = 0.10;
  // ---
  vec3 pToLight = vec3(lightPos - p);
  float power = 10.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  float nDotL = max(dot(n,lightRayDir), 0.);
  float diffuse = (nDotL*power) / d;
  float kd = 1.;

  float gloss = 100.;
  vec3 V = normalize(eye - p);
  vec3 R = normalize(reflect(-lightPos, n));
  float dotRV = dot(R, V);
  float spec = pow(dotRV, gloss);

  return ambient +
          diffuse * 0.5 +
          spec * 0.4;
}

vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
  vec2 xy = fragCoord - size / 2.0;
  float z = size.y / tan(radians(fieldOfView) / 2.0);
  return normalize(vec3(xy, -z));
}

vec3 estimateNormal(vec3 v){
  vec3 n;
  n.x = sdScene(vec3(v.x + Epsilon, v.y, v.z)) - sdScene( vec3(v.x - Epsilon, v.y, v.z));
  n.y = sdScene(vec3(v.x, v.y + Epsilon, v.z)) - sdScene( vec3(v.x, v.y - Epsilon, v.z));
  n.z = sdScene(vec3(v.x, v.y, v.z + Epsilon)) - sdScene( vec3(v.x, v.y, v.z - Epsilon));
  return normalize(n);
}

void main(){
    vec2 p = -1.0 + 2.0 * gl_FragCoord.xy / u_res.xy;
    p.x *= u_res.x/u_res.y;
    vec3 i3;

    vec3 eye = vec3(3);
    vec3 center = vec3(0);
    vec3 lightPos = vec3(0) + eye;
    vec3 up = vec3(0,1,0);
    vec3 ray = rayDirection(80.0, u_res, gl_FragCoord.xy);

    vec3 worldDir = viewMatrix(eye, center, up) * ray;

    float d = rayMarch(eye, worldDir);

    if(d < MaxDist){
      vec3 v = eye + worldDir*d;
      vec3 n = estimateNormal(v);
      float lights = lighting(v, n, lightPos, eye);
      i3 = vec3(d * lights);
    }

    gl_FragColor = vec4(i3,1);
}












