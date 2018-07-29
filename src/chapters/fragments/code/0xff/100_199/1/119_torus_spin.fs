// 116
precision highp float;
uniform vec2 u_res;
uniform float u_time;

const float Epsilon = 0.01;
const float MaxDist = 400.;
const int MaxSteps = 128;
const float PI = 3.141592658;
const float TAU = PI*2.;
const float HALF_PI = PI*0.5;

const vec3 lightGrey = vec3(1.);
const vec3 darkGrey = vec3(0.4);

float sampleCheckerboard(vec2 c) {
  vec2 sz = vec2(0.008);
  vec2 co = ((mod(c,sz*2.)-sz*1.)+1.)/2.;

  vec2 s = step(co, vec2(.5) );
  return s.x;
  //if(s.x == 1.){return 0.;}
  //return 1.;
}

float rectSDF(vec2 p, vec2 size) {//book of shaders
  vec2 d = abs(p) - size;
  return min(max(d.x,d.y),.0)+length(max(d,.0));
}

float sample(vec2 c) {
  vec2 sz = vec2(0.005* PI);
  vec2 co = mod(c,sz)-sz*0.5;
  return rectSDF(co, vec2(0.0125)) * 8.;
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

mat3 viewMatrix(vec3 eye, vec3 center, vec3 up) {
  vec3 f = normalize(center - eye);
  vec3 s = normalize(cross(f, up));
  vec3 u = cross(s, f);
  return mat3(s, u, -f);
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

float sdScene(vec3 p, out float col){
  float t = u_time/1.;

  vec3 n = p;
  n = normalize(n);

  vec2 uv = vec2(atan(n.x, n.z)/PI + .5,
                  asin(n.y)/PI + 0.5);
  // if(n.x < 0.){
  //   uv.y *- -1.;
  // }
  // uv.x += t * .25;
  uv *= 0.25;
 
  // col = sampleCheckerboard(uv);

  float t1 = sdTorus( (vec4(p,1) * r2dX(t*0.00)).xyz + vec3(1,0,0), vec2(1.5, 0.85));
  // float t1 = sdTorus( p, vec2(1.5, 0.85));
  // float t2 = sdTorus(p+vec3(1.75,0,0), vec2(1., 0.5));
  // float t3 = sdTorus(p+vec3(-1.75,0,0), vec2(1., 0.5));
  // float res = min(t1, t2);
  // res = min(res, t3);

  return t1;
  // return res;
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

float getColor(vec3 n){
  float t = u_time * 0.0;

  // float u = acos(n.y/1.)*TAU;
  // float v = acos(n.x/(1.+ 0.5 *cos(2.*PI) ))*TAU;
  // vec2 uv = vec2(u,v);
  
  float a = atan(n.x, n.z)/PI + .5;
  vec2 uv = vec2(a+t, 0.);


  vec3 up = vec3(0,1,0);
  float an = acos( dot(up,n));

  if(an > PI/2.){
    // uv.x = 0.;
  }

  // if(n.x < 0. && n.z < 0.){
  //   uv.x *= 0.1;
  //   // uv.x = 0.;
  // }
  

  // uv.x += t * .25;
  uv *= 0.25 / PI;

  return sampleCheckerboard(uv);
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

    float intensity = getColor(n);

    float lights = lighting(v, n, lightPos);
    // i = lights/1.3 * col.x;
    // i = col.x;
    i = intensity;
  }

  gl_FragColor = vec4(vec3(i), 1);
}