// 120 - "box texture"
precision highp float;
uniform vec2 u_res;
uniform float u_time;

const float Epsilon = 0.0001;
const float MaxDist = 400.;
const int MaxSteps = 128;
const int MaxShadowStep = 100;
const float PI = 3.141592658;
const float TAU = PI*2.;
const float HALF_PI = PI*0.5;

const vec3 lightGrey = vec3(1.);
const vec3 darkGrey = vec3(0.4);

float cSDF(vec2 p, float r){
  return length(p) - r;
}

float sdSphere(vec3 p, float r){
  return length(p)-r;
}

float samplePac(vec2 p){
  float t = u_time*4.;
  float theta = abs(atan(p.y,p.x))/PI;
  float i = smoothstep(0.01,0.001,cSDF(p,.45)) * 
        step(.25,theta+(sin(t*PI*2.)+1.)/2.*.25);
  i += step(cSDF(p+vec2(mod(t,1.)-1.,0.),.08),0.);
  return i;
}

float sampleChecker(vec2 c) {
  float col;
  float sz = 0.85 + 0.15;//*sin(u_time/5.);

  float x = step(mod(c.x,sz), sz/2.);
  float y = step(mod(c.y,sz), sz/2.);

  if(x == y){return 0.8;}
  return x*y;
}

float getHeight(vec2 c){
  float t = -u_time*2.;
  float r = length(c)*4. + t;
  float rLen = sin(r) + cos(r*2.);
  return rLen;
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
  float ambient = 0.1;
  // ---
  vec3 pToLight = vec3(lightPos - p);
  float power = 50.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  float nDotL = max(dot(n,lightRayDir), 0.);
  float diffuse = (nDotL*power) / (d*d);
  float kd = 0.85;

  float gloss = 200.;    
  vec3 V = normalize(eye - p);
  vec3 R = normalize(reflect(-lightPos, n)); 
  float dotRV = dot(R, V);
  float spec = pow(dotRV, gloss);
  float ks = 1.;


  return ambient + kd*diffuse + ks*spec;
}

vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
  vec2 xy = fragCoord - size / 2.0;
  float z = size.y / tan(radians(fieldOfView) / 2.0);
  return normalize(vec3(xy, -z));
}

float cubeSDF(vec3 p, vec3 sz) {
  vec3 d = abs(p) - sz;
  float insideDistance = min(max(d.x, max(d.y, d.z)), 0.0);
  float outsideDistance = length(max(d, 0.0));    
  return insideDistance + outsideDistance;
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

// sss - just a marker
float sdScene(vec3 p, out float col){
  float h = getHeight(p.xz);
  h = max(h, .25);
  float y = h/4.;
  
  float ch = sampleChecker(p.xz);

  float c_ = 1.0;
  vec2 uv = mod(p.xz, vec2(c_))*0.5*c_ - 0.25; 
  uv *= 4.;
  uv.x += 0.5;// + sin(u_time);
  uv.y -= 0.5;

  // float c = samplePac( uv);
  float c = 0.;
  
  col = h * c + 0.1 + (ch*0.3);

  float box = sdBox(p - vec3(0, y*1.3, 0), vec3(2, 0.1, 2.));
  return box;
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
    s += d/3.5;
    


    if(d > MaxDist){
      return MaxDist;
    }
  }
  return MaxDist;
}



void main(){
  float i;
  float t = u_time * .15;
  vec2 fc = gl_FragCoord.xy;

  float dist = 4.;
  vec3 eye = vec3(dist * cos(t), 3.  , dist * sin(t));
  // vec3 eye = vec3(4., 3., 2.);
  vec3 center = vec3(0, 0, 0.);
  vec3 lightPos =  vec3(0., 2., 4);
  vec3 up = vec3(0,1,0);

  mat3 viewWorld = viewMatrix(eye, center, up);
  vec3 ray = rayDirection(90., u_res, fc);

  vec3 worldDir = viewWorld * ray;
  vec3 col;
  float d = rayMarch(eye, worldDir, col);
  vec3 v = eye + worldDir*d;

  if(d < MaxDist){

    // float visibleToLight = shadowMarch(eye+ray*(d), lightPos);

    vec3 n = estimateNormal(v);
    float lights = lighting(v, n, lightPos, eye);
    i = lights * col.x;

    // if(visibleToLight == 0.){
      // i -= .35;
    // }
  }
  
  i = pow(i, 1./2.2);

  gl_FragColor = vec4(vec3(i), 1);
}