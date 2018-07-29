// Torus SDF test
precision highp float;
uniform vec2 u_res;
uniform float u_time;

const float Epsilon = 0.0001;
const float E = Epsilon;
const float MaxDist = 400.;
const int MaxSteps = 128;
const float PI = 3.141592658;
const float TAU = PI*2.;
const float HALF_PI = PI*0.5;
const int MaxShadowStep = 100;

const vec3 lightGrey = vec3(1.);
const vec3 darkGrey = vec3(0.4);
const float X_SCALE= 13443.;
const float Y_SCALE = 389492.;

float sdCircle(vec2 p, float r){
  float d = length(p) - r;
  return d;
}

float sdRect(vec2 p, vec2 size) {//book of shaders
  vec2 d = abs(p) - size;
  return min(max(d.x,d.y),.0)+length(max(d,.0));
}

float sample(vec2 c, vec3 n) {
  float t = u_time*0.3;

  vec2 sz = vec2(1.);
  vec2 co = ((mod(c,sz*2.)-sz)+1.)/2.;
  co -= vec2(0.5);

  float d;
  float outer = step(sdCircle(co, .25), 0.);
  float inner = step(sdCircle(co, .22), 0.);
  d += outer - inner;

  float _ = acos(dot(vec3(0,1,0), n))+t*2.;// +  acos(dot(vec3(1,0,0), n))*10./PI   + t;
  co *= mat2( -cos(_), sin(_),
              sin(_), cos(_));

  float w = 0.3;
  d += step(sdRect(co, vec2(w) ), 0.) - step(sdRect(co, vec2(w*0.95) ), 0.);

  return d;
  vec2 s = step(co, vec2(.5) );
}



float rectSDF(vec2 p, vec2 size) {//book of shaders
  vec2 d = abs(p) - size;
  return min(max(d.x,d.y),.0)+length(max(d,.0));
}



float sdEllipsoid( in vec3 p, in vec3 r ){
    return (length( p/r ) - 1.0) * min(min(r.x,r.y),r.z);
}

mat4 scale(vec3 v){
    return mat4(v.x,  0,  0,  0,
                0,  v.y, 0,  0,
                0,  0,  v.z,  0,
                0,  0,  0,  1);
}
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

float valueNoise(float seed, vec2 p){  
  float x = p.x * X_SCALE;
  float y = p.y * Y_SCALE;
  return fract( sin(x+y) * (23454. + seed));
}


mat3 viewMatrix(vec3 eye, vec3 center, vec3 up) {
  vec3 f = normalize(center - eye);
  vec3 s = normalize(cross(f, up));
  vec3 u = cross(s, f);
  return mat3(s, u, -f);
}

float sdCylinder(vec3 p, vec2 sz ){
  vec2 d = abs(vec2(length(p.xy),p.z)) - sz;
  float _out = length(max(vec2(d.x,d.y), 0.));
  float _in = min(max(d.x,d.y), 0.);
  return  _in + _out;
}



float lighting(vec3 p, vec3 n, vec3 lightPos){
  float ambient = 0.3;
  // ---
  vec3 pToLight = vec3(lightPos - p);
  float power = 44.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  float nDotL = max(dot(n,lightRayDir), 0.);
  float diffuse = (nDotL*power) / d;
  float kd = 1.;

  // ---
  float gloss = 100.;
  vec3 H = normalize(lightRayDir + p);
  float NdotH = max(dot(n, H), .0);
  // vec3 r = reflect(lightRayDir, n);
  // float RdotV = max( 0., dot(r, normalize(p)) );
  float spec = pow( NdotH , gloss )/d;
  // float spec = pow(RdotV, gloss);
  float ks = 0.;
  // ---

  return ambient + diffuse + spec;
  // return spec;
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
  float res;
  float t = u_time/1.;

  vec3 n = p;
  n = normalize(n);

  // vec2 uv = vec2(atan(n.x, n.z) /(PI) + .5,
                 // asin(n.y)/(PI) + .5);
  // uv *= 80.;


  // n.x+=0.1 * t;
  float u = acos(n.y/1.)*TAU;
  float v = acos(n.x/(1.+ .25 * cos(2.*PI) ))*TAU;
  v += t*1.5;
  u -= t*2.;
  vec2 uv = vec2(u*2.,v);

  col = sample(uv,n);
  // col = test(uv);

  // res = sdSphere(p, 1.);
  res = -sdTorus( (vec4(p,1)*scale(vec3(1, 1., 1) )).xyz , vec2(2.2, 1.7));
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
  float t = -u_time*.5;
  vec2 fc = gl_FragCoord.xy;

  float dist = 4.;
  // vec3 eye = vec3(dist * cos(t), sin(t) * 3., dist * sin(t));
  vec3 eye = vec3(0, 0, 3.69);
  vec3 center = vec3(0, 0, 0.);
  vec3 lightPos =  vec3(0., 0., 4) + eye;
  vec3 up = vec3(0,1,0);

  mat3 viewWorld = viewMatrix(eye, center, up);
  vec3 ray = rayDirection(90. - sin(t)*30. , u_res, fc);

  vec3 worldDir = viewWorld * ray;
  vec3 col;
  float d = rayMarch(eye, worldDir, col);
  vec3 v = eye + worldDir*d;


  if(d < MaxDist){
    vec3 n = estimateNormal(v);
    float lights = lighting(v, n, lightPos);
    i = lights * col.x;
    // i = col.x;
  }

  gl_FragColor = vec4(vec3(i), 1);
}