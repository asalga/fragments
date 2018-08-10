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

// x => overall size/radius
// y => thickness
float sdTorus(vec3 p, vec2 t){
  // first component is the difference between
  // the sample point straight line distance and the
  // torus overall size
  // second component is the
  vec2 q = vec2(length(p.xz)-t.x, p.y);
  return length(q) - t.y;
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
  float ambient = 0.1;
  // ---
  vec3 pToLight = vec3(lightPos - p);
  float power = 14.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  float nDotL = max(dot(n,lightRayDir), 0.);
  float diffuse = (nDotL*power) / d;
  float kd = 1.;

  float gloss = 10.;
  vec3 V = normalize(eye - p);
  vec3 R = normalize(reflect(-lightPos, n));
  float dotRV = dot(R, V);
  float spec = pow(dotRV, gloss);

  return ambient + diffuse + spec;
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
  float t = u_time * 0.0;

  float _1 =       - t;
  float _2 = PI/2. - t;

  vec2 dims = vec2(1.4, 0.7);

  vec3 rot_t1 = (vec4(p - vec3(dims.x/2. , 0,0),1) * r2dX(_1)).xyz;
  vec3 rot_t2 = (vec4(p - vec3(-dims.x/2. ,0,0),1) * r2dX(_2)).xyz;

  float t1 = sdTorus(rot_t1, dims);
  float t2 = sdTorus(rot_t2, dims);

  if(t1 < Epsilon){
    rot = vec3(_1,0,0);
  }
  if(t2 < Epsilon){
    rot = vec3(_2, 0,0);
  }

  return min(t2,t1);
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
  float t = u_time * 0.0;
  float a = atan(n.x, n.z);

  float u = ((atan(n.z/n.x))*2.+PI)/ TAU;

  float sz = 0.25;
  return step(mod(u+t, sz), sz/2.) + 0.25;
}

void main(){
  float i;
  // float t = u_time * 0.0;

  float dist = 4.;
  // vec3 eye = vec3(dist * cos(t), sin(t) * 3., dist * sin(t));
  vec3 eye = vec3(0, 10, 5);
  vec3 center = vec3(0);
  vec3 lightPos =  vec3(0,5,2) + eye;
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
    i3 = vec3(getColor(nt,v)) * lights;

    i3 = pow(i3, 1./vec3(2.2));
  }

  // i3 = vec3(gl_FragCoord.x/u_res.x);

  gl_FragColor = vec4(vec3(i3), 1);
}