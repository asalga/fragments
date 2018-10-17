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
const int MaxShadowStep = 100;
const float TIME_SCALE = .9;

float samplePolarChecker(vec2 c){
  float t = u_time*2.;

  float r = pow( length(c), 2.) + t;
  float sz = 3.;
  float rLen = step(mod(r, sz), sz/2.);
  float angle = step(mod(atan(c.x,c.y)+PI, PI/5.), .3);

  float fog = pow(length(c)/2., 4.);
  if(rLen == angle){return 0.8* fog;}
  return rLen*angle * fog;
}

float sampleChecker(vec2 c) {
  float col;
  float sz = .25;

  float x = step(mod(c.x,sz), sz/2.);
  float y = step(mod(c.y,sz), sz/2.);

  if(x == y){return 1.;}
  return x*y;
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
  float gloss = 200.;
  vec3 R = normalize(reflect(-lightRayDir, n));
  float dotRV = max(dot(R, V), 0.);
  float spec = pow(dotRV, gloss);
  float ks = 1.;

  return ambient + diffuse*kd + spec *ks;
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

float sdScene(vec3 p, out float col){
  float t = u_time*TIME_SCALE;
  vec3 c = vec3(0., 1.0, 0.);

  float id = floor(p.y);

  col = sampleChecker(p.xz);
  p = mod(p,c)-c*0.5;

  float sz = (t-id+.5) * 0.5+0.5;
  float szx = (t-id+1.) * 0.5+0.5;

  sz = abs( floor(pow(sz, .0125)*4.)/4. );

  szx = abs( floor(pow(szx, 8. )*4.)/4. );

  sz = clamp(sz, 0. , 1.);
  szx = clamp(szx, 0. , 2.);

  p = (vec4(p, 1.) * r3dY(  id *PI/2.  )).xyz;

  return sdBox(p, vec3(sz *.5, .01, szx*.5));
}
float shadowMarch(vec3 point, vec3 lightPos){

  vec3 pToLight = lightPos-point;
  vec3 rd = normalize(pToLight);
  vec3 ro = point;

  float s = 0.;
  for(int i = 0; i < MaxShadowStep; ++i){
    vec3 v = ro + (rd*s);

    float dum;
    float dist;
    dist = sdScene(v, dum);

    if(dist < Epsilon){
      return 0.;
    }
    s += dist/1.;

    if(s >= MaxDist){
      return 1.;
    }
  }
  return 1.;
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
  float t = u_time*TIME_SCALE;
  vec2 fc = gl_FragCoord.xy;

  float dist = 2.;
  vec3 eye = vec3(cos(t/2.)*dist, dist+t , sin(t/2.)*dist);
  vec3 center = vec3(0, t, 0.);
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
    float lights = lighting(v, n, lightPos,eye);

    // float visibleToLight = shadowMarch(eye+worldDir*(d-0.001), vec3(0., t, 0.));

    i = lights * col.x;

    // if(visibleToLight <= 0.){
      // i *= 0.25;
    // }
  }

  gl_FragColor = vec4(vec3(i), 1);
}