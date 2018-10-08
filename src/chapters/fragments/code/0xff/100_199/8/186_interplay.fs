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
  float sz = 0.25;

  float x = step(mod(c.x,sz), sz/2.);
  float y = step(mod(c.y,sz), sz/2.);

  if(x == y){return 0.8;}
  return x*y;
}






float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

mat2 r2d(float a){
  return mat2(cos(a),sin(a),-sin(a),cos(a));
}

float diaid(vec2 p){
  // vec2 p = gl_FragCoord.xy/u_res;
  float i;
  float t = u_time*4.;

  float cntPerSide = 1. + mod( (1. + floor(t/PI*2.)), 10.);

  vec2 c = vec2(1./cntPerSide);
  vec2 rp = mod(p, c) - c * 0.5;

  // alternate the rotation each cell
  vec2 id = floor(p*cntPerSide);
  if(mod(id.x, 2.) > .0){
    t = -t;
  }
  if(mod(id.y, 2.) > .0){
    t = -t;
  }

  rp *= r2d(t);

  i += step(sdRect(rp+vec2(-c.x/2.,   c.y/2.), c*.5), 0.) * 0.1;
  i += step(sdRect(rp+vec2(-c.x/2.,  -c.y/2.), c*.5), 0.) * 0.25;
  i += step(sdRect(rp+vec2(c.x/2.,   -c.y/2.), c*.5), 0.) * 0.75;
  i += step(sdRect(rp+vec2(c.x/2.,    c.y/2.), c*.5), 0.) * 1.0;

  // gl_FragColor = vec4(vec3(i),1.);
  return i;
}

float diaid1(vec2 p){
  // vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  p = (mod(p, vec2(0.5))-vec2(.25)) * r2d(PI/4.);
  vec2 shape = mod(abs(p*4.) - u_time * 0.5, vec2(0.5));
  shape = step(shape, vec2(0.25)) * step(abs(p.yx), abs(p.xy));
  // gl_FragColor = vec4(vec3(shape.x + shape.y),1);
  return shape.x +shape.y;
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
float sdSphere(in vec3 p, in float r){
  return length(p)-r;
}
float lighting(vec3 p, vec3 n, vec3 lightPos){
  float ambient = 0.0;
  // ---
  vec3 pToLight = vec3(lightPos - p);
  float power = 50.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  float nDotL = max(dot(n,lightRayDir), 0.);
  float diffuse = (nDotL*power) / d;
  float kd = 1.;

  // ---
  // float gloss = 100.;
  // vec3 H = normalize(lightRayDir + p);
  // float NdotH = max(dot(n, H), .0);
  // // vec3 r = reflect(lightRayDir, n);
  // // float RdotV = max( 0., dot(r, normalize(p)) );
  // float spec = pow( NdotH , gloss )/d;
  // // float spec = pow(RdotV, gloss);
  // float ks = 0.;
  // ---

  return ambient + diffuse;// + spec;
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
  float t = u_time;
  col = 1.;

  float mv = t-1.;
  float visibleBox = sdBox(p-vec3(0.,mv,0.), vec3(1., 0.08, 1.));

  float sphereSub = sdSphere(p-vec3(0., mv, 0.), (sin(t*PI + PI/2.)+1.)/2. * .8  );
  vec2 uv = p.xy;// + vec2(t/10.);

  vec3 c = vec3(0., 2., 0. );
  vec3 rp = mod(p, c)-c*0.5;
  // rp.y += t;
  float sphereArmy = sdSphere(rp, .5);

  col = 1.;//

  // if(sphereArmy < 0.001){col = samplePolarChecker(uv);}
  // col = samplePolarChecker(uv);

  // return
  //visibleBox;res
  // float res = max(visibleBox, -subtractbox1);
   // float res = max(visibleBox, -subtractbox2);
  // float _2 = min(_1, -subtractbox2);
  float res = max(visibleBox, -sphereSub);
  res = min(res, sphereArmy);
  // return visibleBox;
  return res;
}

float sdSceneFront(vec3 p, out float col){
  float t = u_time;
  col = 1.;

  float visibleBox = sdBox(p, vec3(1));
  // fl//oat subtractbox = sdBox(p - vec3(.05), vec3(1));

  return visibleBox;
  //max(visibleBox, -subtractbox);
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
  float t = u_time;
  vec2 fc = gl_FragCoord.xy;

  float dist = 2.7;
  vec3 eye = vec3( dist, t, dist);
  // vec3 eye = vec3(cos(.4)*dist, t, sin(.4)*dist);
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
    float lights = lighting(v, n, lightPos);
    i = lights * col.x;
  }

  gl_FragColor = vec4(vec3(i), 1);
}