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


float valueNoise(vec2 p){
  #define Y_SCALE 45343.
  #define X_SCALE 37738.
  float x = p.x * X_SCALE;
  float y = p.y * Y_SCALE;
  return fract( sin(x+y) * 23454.);
}

float sdSphere(in vec3 p, in float r){
  return length(p)-r;
}

float sdTorus(in vec3 p, in vec2 t){
  vec2 q = vec2(length(p.xz)-t.x, p.y);
  return length(q) - t.y;
}

float sdBox(vec3 p, vec3 sz) {
  vec3 d = abs(p) - sz;
  float insideDistance = min(max(d.x, max(d.y, d.z)), 0.0);
  float outsideDistance = length(max(d, 0.0));
  return insideDistance + outsideDistance;
}

float sdQuarterRing(in vec3 p, in vec2 dim){
  float sz = 0.25;

  // total hack
  p *= .9;
  p.x -= 0.5-dim.y;
  p.z -= 0.5-dim.y;

  float torus = sdTorus(p, dim);

  float box1 = sdBox(p + vec3(sz, 0., sz), vec3(0.25, .25, 0.25));

  // float box2 = sdBox(p + vec3(0.0, 0., 0.24), vec3(0.5, 0.1, 0.23));
  // float qr = max(torus, -box1);
  // qr = max(qr, -box2);
  // float qr = min(torus, box1);
  // qr = min(qr, box2);
  // return qr;
  // float _ = min(box1, torus);
  // return min(_, box2);
  // return min(box1, torus);
  // return max(torus, -box1);

  // return min(box1, qr);
  // return max(box1, -torus);
  return max(torus, box1);

  // return box1;
  // return qr;
}

mat3 viewMatrix(vec3 eye, vec3 center, vec3 up) {
  vec3 f = normalize(center - eye);
  vec3 s = normalize(cross(f, up));
  vec3 u = cross(s, f);
  return mat3(s, u, -f);
}

float lighting(vec3 p, vec3 n, vec3 lightPos, vec3 eye){
  float ambient = 0.0;
  // ---
  vec3 pToLight = vec3(lightPos - p);
  float power = 30.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  float nDotL = max(dot(n,lightRayDir), 0.);
  float diffuse = (nDotL*power) / d;
  float kd = 1.;

  // ---

  // vec3 H = normalize(lightRayDir + p);
  // float NdotH = dot(n, H);
  // vec3 r = reflect(lightRayDir, n);
  // float RdotV = dot(r, normalize(p));
  // float spec = pow( NdotH , gloss );
  // float spec = pow(RdotV, gloss) / d;
  // ---

  vec3 V = normalize(eye-p);
  float gloss = 10.;
  vec3 R = normalize(reflect(-lightRayDir, n));
  float dotRV = dot(R, V);
  float spec = pow(dotRV, gloss)/d;

  return ambient + diffuse + spec;
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

void rot3dY(inout vec3 p, float a){
  vec4 p4 = vec4(p, 1.);

  float c = cos(a);
  float s = sin(a);

  mat4 r =    mat4(-c, 0, s,  0,
              0,  1, 0,  0,
              s,  0, c,  0,
              0,  0, 0,  1);

  p = (p4 * r).xyz;
  // return p4*r).xyz;
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

float sdScene(vec3 p, out float col){
  float t = u_time;
  col = 1.;
  float rad = 0.5;
  float th = 0.05;

  vec2 id = floor(p.xz);

  float r = valueNoise(id);

  if(r < 0.5){
    rot3dY(p, PI);
  }

  vec3 c = vec3(1., 0. ,1.);
  p = mod(p, c) - c*0.5;

  // float q = qr(p + vec3(0.5, 0., 0.5));

  float q = sdQuarterRing(p, vec2(rad-th, th));

  rot3dY(p, PI + PI/2.);
  float q1 = sdQuarterRing(p, vec2(rad-th, th));


  // float sph = sdSphere(p, 0.125);
  float res = min(q1, q);

  // res = min(res, sph);
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
    s += d/2.;

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

  float dist = 13.;
  vec3 eye = vec3(t, 2.5, 2.+t);
  // vec3 eye = vec3(-0, 5, 0);

  vec3 center = vec3(t+4., -1.3, .25+t);
  vec3 lightPos =  vec3(0., 4., 4) + eye;
  vec3 up = vec3(0,1,0);

  mat3 viewWorld = viewMatrix(eye, center, up);
  vec3 ray = rayDirection(90., u_res, fc);

  vec3 worldDir = viewWorld * ray;
  vec3 col;
  float d = rayMarch(eye, worldDir, col);
  vec3 v = eye + worldDir*d;

  if(d < MaxDist){
    vec3 n = estimateNormal(v);
    float lights = lighting(v, n, lightPos, eye);
    i = lights * col.x;

    float fog = pow(1./d, 1.5) * 10.;
    i *= fog;
  }

  gl_FragColor = vec4(vec3(i), 1);
}