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

float sdCylinder(vec3 p, vec2 sz){
  vec2 d = abs(vec2(length(p.xz),p.y)) - sz;
  float _out = length(max(vec2(d.x,d.y), 0.));
  float _in = min(max(d.x,d.y), 0.);
  return _in + _out;
}

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

float lighting(vec3 p, vec3 n, vec3 lightPos, vec3 eye){
  float ambient = 0.;

  // ---
  vec3 pToLight = vec3(lightPos - p);
  float power = 112.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  float nDotL = max(dot(n,lightRayDir), 0.0);
  float diffuse = (nDotL*power) /d;
  float kd = .38;

  vec3 V = normalize(eye-p);
  float gloss = 4.;
  vec3 R = normalize(reflect(-lightRayDir, n));
  float dotRV = max(dot(R, V), 0.);
  float spec = pow(dotRV, gloss);
  float ks = 12.;

  return ambient + diffuse*kd + spec *ks;
}
// float lighting(vec3 p, vec3 n, vec3 lightPos){
//   float ambient = 0.0;
//   // ---
//   vec3 pToLight = vec3(lightPos - p);
//   float power = 1.;
//   vec3 lightRayDir = normalize(pToLight);
//   float d = length(pToLight);
//   d *= d;
//   float nDotL = max(dot(n,lightRayDir), 0.);
//   float diffuse = (nDotL*power);// / d;
//   float kd = 1.;

//   // ---
//   // float gloss = 100.;
//   // vec3 H = normalize(lightRayDir + p);
//   // float NdotH = max(dot(n, H), .0);
//   // // vec3 r = reflect(lightRayDir, n);
//   // // float RdotV = max( 0., dot(r, normalize(p)) );
//   // float spec = pow( NdotH , gloss )/d;
//   // // float spec = pow(RdotV, gloss);
//   // float ks = 0.;
//   // ---

//   return ambient + diffuse;// + spec;
// }

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

float sdScene(vec3 p, out float col){
  float t = u_time;

  vec3 c = vec3(0.,1.,0.);
  vec3 rc = mod(p, c)-c*0.5;
  float id = floor(p.y);

  col = 1.;
  float maincylinder = sdCylinder(p, vec2(.1, 11100.));

  rc = (vec4(rc, 1.) * r3dZ(PI/2.)).xyz;
  // rc = (vec4(rc, 1.) * r3dX(id/4.)).xyz;

  vec3 rc2 = rc;
  rc2 = (vec4(rc2, 1.) * r3dX(PI/2.)).xyz;

  float cylinders1 = sdCylinder(rc, vec2(.1, sin(t*1.+id/2.)+1.  ));
  float cylinders2 = sdCylinder(rc2, vec2(.1, sin(t*1.+id/2.)+1.  ));

  //+ 1.+(sin(id/5.)+1.)/2. * 10.
  // return cylinders1;
  float cys =  min(cylinders1, cylinders2);
  return cys;
  // return min(maincylinder, cys);
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
  float t = u_time * 5.;
  vec2 fc = gl_FragCoord.xy;

  float dist = 2.;
  vec3 eye = vec3(dist);
  eye.y += t;

  vec3 center = vec3(0, t, 0.);
  vec3 lightPos =  vec3(0., 4., 2) + eye;
  vec3 up = vec3(0,1,0);

  mat3 viewWorld = viewMatrix(eye, center, up);
  vec3 ray = rayDirection(110., u_res, fc);

  vec3 worldDir = viewWorld * ray;
  vec3 col;
  float d = rayMarch(eye, worldDir, col);
  vec3 v = eye + worldDir*d;

  if(d < MaxDist){
    vec3 n = estimateNormal(v);
    float lights = lighting(v, n, lightPos,eye);
    i = lights * col.x;
  }

  gl_FragColor = vec4(vec3(i), 1);
}