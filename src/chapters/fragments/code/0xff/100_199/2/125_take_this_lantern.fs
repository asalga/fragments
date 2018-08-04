// 125
precision highp float;
uniform vec2 u_res;
uniform float u_time;

const float Epsilon = 0.01;
const float MaxDist = 400.;
const int MaxSteps = 128;
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

float lighting(vec3 p, vec3 n, vec3 lightPos, vec3 eye){
  float ambient = 0.01;
  // ---
  vec3 pToLight = vec3(lightPos - p);
  float power = 10.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  float nDotL = max(dot(n,lightRayDir), 0.);
  float diffuse = (nDotL*power) / d;
  float kd = 1.;

  float gloss = 1.;    
  vec3 V = normalize(eye - p);
  vec3 R = normalize(reflect(-lightPos, n)); 
  float dotRV = dot(R, V);
  float spec = pow(dotRV, gloss);

  return ambient + diffuse;// + spec;
}

vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
  vec2 xy = fragCoord - size / 2.0;
  float z = size.y / tan(radians(fieldOfView) / 2.0);
  return normalize(vec3(xy, -z));
}

mat4 r2dY(float a){
  float c = cos(a);
  float s = sin(a);

  return mat4(-c, 0, s,  0,
              0,  1, 0,  0,
              s,  0, c,  0,
              0,  0, 0,  1);
}
mat4 r2dX(float a){
  float c = cos(a);
  float s = sin(a);

  return mat4(1,  0,  0,  0,
              0,  -c, s,  0,
              0,  s,  c,  0,
              0,  0,  0,  1);
}
mat4 r2dZ(float a){
  float c = cos(a);
  float s = sin(a);

  return mat4(c, -s, 0,  0,
              s,  c, 0,  0,
              0,  0, 1,  0,
              0,  0, 0,  1);
}
mat4 scale(float x, float y, float z){
  return mat4(x, 0, 0, 0,
              0, y, 0, 0,
              0, 0, z, 0,
              0, 0, 0, 1);
}

float sdScene(vec3 p, out vec3 rot){

  vec3 c = vec3(4, 0, 8);
  p = mod(p, c)-0.5*c;

  float vert = sdBox(p, vec3(.1, 0.8, .1));
  float horz = sdBox(p- vec3(0,0.3, 0), vec3(.55, 0.1, .1));
  float cross = min(vert, horz);

  return cross;
}

vec3 estimateNormal(vec3 v){
  vec3 n;
  vec3 dum;
  n.x = sdScene(vec3(v.x + Epsilon, v.y, v.z),dum) - sdScene( vec3(v.x - Epsilon, v.y, v.z),dum);
  n.y = sdScene(vec3(v.x, v.y + Epsilon, v.z),dum) - sdScene( vec3(v.x, v.y - Epsilon, v.z),dum);
  n.z = sdScene(vec3(v.x, v.y, v.z + Epsilon),dum) - sdScene( vec3(v.x, v.y, v.z - Epsilon),dum);
  return normalize(n);
}

float rayMarch(vec3 ro, vec3 rd, out vec3 rot){
  float s = 0.;
  for(int i = 0; i < MaxSteps; i++){
    vec3 p = ro + rd * s;
    
    float d = sdScene(p, rot);
    
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
  float t = -u_time * 2.0;

  float dist = 4.;

  // vec3 eye = vec3(dist * cos(t), sin(t) * 3., dist * sin(t));
  vec3 eye = vec3(0, 2, 5.+t);
  vec3 center = vec3(0,2,0. +t);
  vec3 lightPos =  vec3(0,8,8) + eye;
  vec3 up = vec3(0,1,0);
  vec3 ray = rayDirection(70., u_res, gl_FragCoord.xy);

  vec3 worldDir = viewMatrix(eye, center, up) * ray;
  vec3 rot;
  float d = rayMarch(eye, worldDir, rot);
  vec3 i3;

  if(d < MaxDist){
    vec3 v = eye + worldDir*d;
    vec3 n = estimateNormal(v);

    vec3 nt = (vec4(n,1) * r2dX(rot.x)).xyz;

    float lights = lighting(v, n, lightPos, eye);
    // i3 = vec3(getColor(nt,v)) * lights;
    i3 = vec3(d * lights);

    float fog = (1./ pow( (d-3.), 2.4));
    i3 *= fog * 5.;

    i3 = pow(i3, 1./vec3(2.2));
  }

  gl_FragColor = vec4(vec3(i3), 1);
}