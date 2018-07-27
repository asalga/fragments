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

float sampleCheckerboard(vec2 c) {
  vec2 sz = vec2(0.005 * PI);
  vec2 co = ((mod(c,sz*2.)-sz*1.)+1.)/2.;

  vec2 s = step(co, vec2(.5) );
  if(s.x == 1.){return 0.;}
  return 1.;
}

float rectSDF(vec2 p, vec2 size) {//book of shaders
  vec2 d = abs(p) - size;
  return min(max(d.x,d.y),.0)+length(max(d,.0));
}

float sample(vec2 c) {
  vec2 sz = vec2(0.005* PI);
  // vec2 col = step(mod(c, sz*2.), sz);
  // vec2 co = (mod(c, sz)*4.)-.125;
  vec2 co = mod(c,sz)-sz*0.5;

  return rectSDF(co, vec2(0.0125)) * 8.;
  // return 1.-length(col);


  // return pow((length(col) - .001), 0.1);
  // return pow(1.-(length(col) + 1.), .05);

  // return smoothstep( 0., 1. , length(col)/10.);
  //pow(1.-(length(col) + 1.), .08);

  // return (col.x+col.y == 1.) ? .9 : .1;
  // float col;
  // col = mod(c.y, .5);
  // return col;
  // return length(c);
  // vec2 col = (mod(c, sz*2.)+.0)/2.;
  // return 1.*col.x;// + col.y;
  
  // return length(c);
}

float sdEllipsoid( in vec3 p, in vec3 r ){
    return (length( p/r ) - 1.0) * min(min(r.x,r.y),r.z);
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
  float power = 24.;
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

  return ambient + diffuse;// + spec;
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
  // float res;
  float t = u_time/1.;

  vec3 n = p;
  n = normalize(n);

  vec2 uv = vec2(atan(n.x, n.z) /(PI) + .5,
                  asin(n.y)*2./PI);
                 // asin(n.y)/(PI) + .5);


  uv.x += t * .25;
  // float distend = 1. + sin(t);
  // + sin(t);
  //

  uv *= 0.25;

  col = sampleCheckerboard(uv);
  // col = 1.;
  // float v = sample(uv);
  // res = (v*distend) + sdSphere(p, 1.);
  float t1 = sdTorus(p, vec2(1., 0.5));
  float t2 = sdTorus(p+vec3(1.75,0,0), vec2(1., 0.5));
  float t3 = sdTorus(p+vec3(-1.75,0,0), vec2(1., 0.5));

  float res = min(t1, t2);
  res = min(res, t3);

  return res;
}
  // res = sdTorus(p, vec2(1., .25));
  // float s = sdSphere(p+ vec3(1, 0, 0), .27);
  // float c = cubeSDF(p - vec3(0, 0, 1), vec3(1.5, 1, 1.));
  // // res = min(res, s);

  // res = max(res, -c);
  // return res;

  // vec3 np = mod(p, vec3(c))-c *0.5;
  // np.y = p.y;
  
  // // float xIdx = abs(sin( floor(p.x)/10. + t )) *3.;
  // float xIdx = floor(p.x)/10.;
  // float zIdx = floor(p.z)/10.;

  // float cnt = 9.;
  // if( abs(p.x) >= cnt || abs(p.z) > cnt ){
  //  col =  0.;
  //  return MaxDist;
  // }

  // float len = sin(length( vec3(9.) * vec3(xIdx, 0, zIdx)) - t) / 8.;
  // vec3 off = vec3(0, -len ,0);
  // d = cubeSDF( (np + off) , vec3(0.35, 2. + len*1., .35));

  // return d;


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
  vec3 eye = vec3(0, 7, 4);
  vec3 center = vec3(0, 0, 0.);
  vec3 lightPos =  vec3(0., 0., 0) + eye;
  vec3 up = vec3(0,1,0);

  mat3 viewWorld = viewMatrix(eye, center, up);
  vec3 ray = rayDirection(70., u_res, fc);

  vec3 worldDir = viewWorld * ray;
  vec3 col;
  float d = rayMarch(eye, worldDir, col);
  vec3 v = eye + worldDir*d;

  if(d < MaxDist){
    vec3 n = estimateNormal(v);
    float lights = lighting(v, n, lightPos);
    // i = lights/1.3 * col.x;
    i = col.x;
  }


  gl_FragColor = vec4(vec3(i), 1);
}