//
precision highp float;

uniform vec2 u_res;
uniform float u_time;

const float Epsilon = 0.0001;
const float MaxDist = 100.;
const int MaxSteps = 64;
const float PI = 3.141592658;
const float TAU = PI*2.;
const float HALF_PI = PI*0.5;

const vec3 lightGrey = vec3(1.);
const vec3 darkGrey = vec3(0.4);


float sdBox(vec3 p, vec3 sz) {
  vec3 d = abs(p) - sz;
  float insideDistance = min(max(d.x, max(d.y, d.z)), 0.0);
  float outsideDistance = length(max(d, 0.0));
  return insideDistance + outsideDistance;
}

mat3 viewMatrix(vec3 eye, vec3 center, vec3 up) {
  vec3 f = normalize(center - eye);
  vec3 s = normalize(cross(f, up));
  vec3 u = cross(s, f);
  return mat3(s, u, -f);
}

float lighting(vec3 p, vec3 n, vec3 lightPos){
  float ambient = 0.0;
  // ---
  vec3 pToLight = vec3(lightPos - p);
  float power = 140.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  float nDotL = max(dot(n,lightRayDir), 0.);
  float diffuse = (nDotL*power) / d;
  float kd = 1.;

  // ---
  // float gloss = 100.;
  // vec3 H = normalize(lightRayDir + p);
  // float NdotH = max(dot(n, H), .0);
  // // vec3 r = reflect(lightRayDir, n);
  // // float RdotV = max( 0., dot(r, normalize(p)) );
  // float spec = pow( NdotH , gloss )/d;
  // // float spec = pow(RdotV, gloss);
  // float ks = 0.;
  // ---

  return ambient + diffuse;// + spec;
}

vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
  vec2 xy = fragCoord - size / 2.0;
  float z = size.y / tan(radians(fieldOfView) / 2.0);
  return normalize(vec3(xy, -z));
}

void _r3dY(inout vec3 p, float a){

  float c = cos(a);
  float s = sin(a);

  mat4 m = mat4(-c, 0, s,  0,
                0,  1, 0,  0,
                s,  0, c,  0,
                0,  0, 0,  1);

  vec4 _p = vec4(p, 1.);
  _p *= m;
  p.xyz = _p.xyz;
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

float sdSphere(in vec3 p, float r){
  return length(p)-r;
}

float sdScene(vec3 p, out float col){
  float t = u_time*0.5;
  col = 1.;
  float thick = .01;
  float res = 1.;

  res = sdBox(p, vec3(0.28, 0.28, thick));

  float holeSz = 1.;

  for(float it = 2.; it < 7.; it++){
    vec3 rp = p;
    _r3dY(rp, it/5. + it * t);

    float sz = it * 0.3;
    float h = sdBox(rp, vec3(sz - 0.3, sz - 0.3, 1.));
    float s = sdBox(rp, vec3(sz, sz, thick));
    float squareDonut =  max(s, -h);
    res = min(res, squareDonut);
  }

  return res;
}

vec3 estimateNormal(vec3 v){
  vec3 n;
  float dum;
  n.x = sdScene(vec3(v.x + Epsilon, v.y, v.z),dum) - sdScene( vec3(v.x - Epsilon, v.y, v.z),dum);
  n.y = sdScene(vec3(v.x, v.y + Epsilon, v.z),dum) - sdScene( vec3(v.x, v.y - Epsilon, v.z),dum);
  n.z = sdScene(vec3(v.x, v.y, v.z + Epsilon),dum) - sdScene( vec3(v.x, v.y, v.z - Epsilon),dum);
  return normalize(n);
}

float rayMarch(vec3 ro, vec3 rd, out vec3 col){
  float s = 0.;
  for(int i = 0; i < MaxSteps; i++){
    vec3 p = ro + rd * s;

    float intensity;
    float d = sdScene(p, intensity);
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
  float i;
  float t = u_time;
  vec2 fc = gl_FragCoord.xy;

  float dist = 4.;
  vec3 eye = vec3(dist);
  vec3 center = vec3(0, 0, 0.);
  vec3 lightPos =  vec3(0., 4., 2) + eye;
  vec3 up = vec3(0,1,0);

  mat3 viewWorld = viewMatrix(eye, center, up);
  vec3 ray = rayDirection(90., u_res, fc);

  vec3 worldDir = viewWorld * ray;
  vec3 col;
  float d = rayMarch(eye, worldDir, col);
  vec3 v = eye + worldDir*d;

  if(d < MaxDist){
    vec3 n = estimateNormal(v);
    float lights = lighting(v, n, lightPos);
    i = lights * col.x;
  }

  gl_FragColor = vec4(vec3(i), 1);
}