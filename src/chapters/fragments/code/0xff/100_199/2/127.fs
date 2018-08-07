// 119 - "Torus Spin"
precision highp float;
uniform vec2 u_res;
uniform float u_time;

const float Epsilon = 0.01;
const float MaxDist = 400.;
const int MaxSteps = 128;
const float PI = 3.141592658;
const float TAU = PI*2.;
const float HALF_PI = PI*0.5;

float length2( vec2 p ){
  return sqrt( p.x*p.x + p.y*p.y );
}
float length8( vec2 p )
{
  p = p*p; p = p*p; p = p*p;
  return pow( p.x + p.y, 1.0/8.0 );
}

float sdTorus(vec3 p, vec2 t){  
  vec2 q = vec2(length(p.xy)-t.x, p.z);
  return length(q) - t.y;
}

float sdBox(vec3 p, vec3 sz) {
  vec3 d = abs(p) - sz;
  float _in = min(max(d.x, max(d.y, d.z)), 0.);
  float _out = length(max(d, 0.));
  return _in + _out;
}

float sdTorus82( vec3 p, vec2 t )
{
  vec2 q = vec2(length2(p.xy)-t.x,p.z);
  return length8(q)-t.y;
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

float lighting(vec3 p, vec3 n, vec3 lightPos, vec3 eye){
  float t = u_time;

  float ambient = 0.1 + 1.-abs(sin(t));
  
  vec3 pToLight = vec3(lightPos - p);
  float power = 20.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  float nDotL = max(dot(n,lightRayDir), 0.);
  float diffuse = (nDotL*power) / d;
  float kd = 1.;// * abs(sin(t));

  float gloss = 100.;    
  vec3 V = normalize(eye - p);
  vec3 R = normalize(reflect(-lightPos, n)); 
  float dotRV = dot(R, V);
  float spec = pow(dotRV, gloss);
  float ks = 1.0;

  return ambient + 
        kd * diffuse + 
        ks * spec;
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
  float t = u_time * 1.;
  float _1 = PI/4.;
  float c = 0.;
  c = smoothstep( 0., 1., sin(t*2. -2.)) * 10.;

  vec3 np = mod(p, vec3(0,0,c)) - vec3(0, 0, 0.5*c);
  float cellIdx = floor(p.z/10.)*10.;

  float tt = t;
  if(c == 0.){
    cellIdx = 0.;
    tt = 0.;
  }

  vec3 rot_t1 = (vec4(np + vec3(0, sin( (cellIdx * 10.) + tt*4.5), 0), 1) * r2dZ(_1)).xyz;

  vec2 dims = vec2(1.5, 0.3);
  float depth = abs(sin(t)) * 0.15;
  float t1 = mix(sdTorus82(rot_t1, vec2(1.5, 0.25) ), sdTorus(rot_t1, dims), abs(sin(t)) );
  float b1 = sdBox( (vec4(rot_t1,1) * r2dY(t)).xyz, vec3(0.8, 0.15, depth));

  float b2 = sdBox(rot_t1, vec3(0.15, 0.85, depth));
  // float t2 = sdTorus(rot_t2, dims);

  if(t1 < Epsilon){
    rot = vec3(1.,0,0);
  }

  // if(t2 < Epsilon){
  //   rot = vec3(_2, 0,0);
  // }
 
  float final = b1;
  final = min(final, b2);
  final = min(final, t1);

  return final;
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

float getColor(vec3 n, vec3 p){
  float t = u_time * 0.4;
  // float a = atan(n.x, n.z);
  float u = ((atan(n.y/n.x))*2.+PI)/ TAU;
  float sz = 0.25;
  return step(mod(u+t, sz), sz/2.) + 0.25;
}

void main(){
  float i;
  float t = u_time;
  float dist = 4.;
  // vec3 eye = vec3(dist * cos(t), sin(t) * 3., dist * sin(t));
  vec3 eye = vec3(dist * (sin(t)), dist * (sin(0.)), 8);
  vec3 center = vec3(0, 0, -5);// 0 , 0, 0
  vec3 lightPos = vec3(2,2,2);
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
    float lights = lighting(v, nt, lightPos, eye);
    i3 = vec3(lights);
    i3 = pow(i3, 1./vec3(2.2));
  }

  gl_FragColor = vec4(vec3(i3), 1);
}