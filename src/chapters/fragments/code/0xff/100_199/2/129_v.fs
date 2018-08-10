//
precision highp float;
uniform vec2 u_res;
uniform float u_time;
uniform vec3 u_mouse;

const float Epsilon = 0.0001;
const float MaxDist = 400.;
const int MaxSteps = 128;
const int MaxShadowStep = 50;
const float PI = 3.141592658;
const float TAU = PI*2.;
const float HALF_PI = PI*0.5;
#define CNT 10

const float lightGrey = .9;
const float darkGrey = .0;

float random (vec2 st) {
  return fract(sin(dot(st.xy,vec2(12.9898,78.233)))*43758.5453123);
}

////////////////////////////////////////////////

float valueNoise(vec2 p){
  #define Y_SCALE 45343.
  #define X_SCALE 37738.
  float x = p.x * X_SCALE;
  float y = p.y * Y_SCALE;
  return fract( sin(x+y) * 23454.);
}

float smoothValueNoise(vec2 p){
  vec2 lv = fract(p);
  lv = smoothstep(0.,1.,lv);
  vec2 id = floor(p);
  float bl = valueNoise(vec2(id));
  float br = valueNoise(vec2(id)+vec2(1,0));
  float b = mix(bl,br,lv.x);

  float tl = valueNoise(vec2(id)+vec2(0,1));
  float tr = valueNoise(vec2(id)+vec2(1,1));
  float t = mix(tl,tr,lv.x);

  return mix(b,t,lv.y);
}

float cSDF(vec2 p, float r){
  return length(p) - r;
}


float sdSphere(vec3 p, float r){
  return length(p)-r;
}


float sampleChecker(vec2 c) {
  float col;
  vec2 sz = vec2(1.);

  vec2 ch = step(mod(c,sz), sz/vec2(2.));

  if(ch.x == ch.y){return lightGrey;}
  return darkGrey;
}


float sdBox(vec3 p, vec3 sz) {
  vec3 d = abs(p) - sz;
  float _in = min(max(d.x, max(d.y, d.z)), 0.0);
  float _out = length(max(d, 0.0));
  return _in + _out;
}

mat3 viewMatrix(vec3 eye, vec3 center, vec3 up) {
  vec3 f = normalize(center - eye);
  vec3 s = normalize(cross(f, up));
  vec3 u = cross(s, f);
  return mat3(s, u, -f);
}


float lighting(vec3 p, vec3 n, vec3 lightPos, vec3 eye){
  float ambient = 0.4;

  vec3 pToLight = vec3(lightPos - p);
  float power = 50.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  float nDotL = max(dot(n,lightRayDir), 0.);
  float diffuse = (nDotL*power) / (d*d);
  float kd = 0.75;

  float gloss = 500.;
  vec3 V = normalize(eye - p);
  vec3 R = normalize(reflect(-lightPos, n));
  float dotRV = dot(R, V);
  float spec = pow(dotRV, gloss);
  float ks = 1.;//abs(sin(u_time*2.));


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




float tex2(vec2 p){
  vec2 points[CNT];

  // populate with values
  for(int i = 0; i < CNT; i++){
    points[i] = vec2( 2.*fract(cos(float(i)*1000.0))-1.,
                      2.*fract(sin(float(i)*20000.0))-1.);

    points[i].x += smoothValueNoise(vec2(u_time + float(i+1)*100. ));
  }




  float dist = length(p - points[0]);

  for(int i = 1; i < CNT; i++){
    float testLength = length(p-points[i]);
    if(testLength < dist){
      dist = testLength;
    }
  }

  dist = clamp(dist, 0. , 1.);
  return dist;
}


float sdScene(vec3 p, out float col){
  // float h = getHeight(p.xz)*0.01;
  float t = u_time;
  float h = 0.;
  col = .1 + .3*sampleChecker(p.xz*2.5);
  h = tex2(p.xz);
  col = 1.-h;

  // h = pow(h,5.3);


  h = max(h, 0.95);


  // h = clamp(h, 0.3, .9)/10.;
  // float testH = smoothstep(0.0, .9, h);
  float testH = h;

  //clamp((1.-h)/5., 0. , 1.);
  // testH = max(testH, 0.2);
  // testH =
  //smoothstep(0.,1., h*1.)/12.;
  // h = max(h,0.5);


  float box = sdBox(p + vec3(0,testH,0), vec3(2., 0.01, 2.));
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
    s += dist;

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
  float t = u_time * .15;
  vec2 fc = gl_FragCoord.xy;

  float dist = 4.;

  vec3 eye = vec3(dist * cos(t), 1.0  , dist * sin(t));
  // vec3 eye = vec3(4., 3., 2.);
  vec3 center = vec3(0, 0, 0.);
  vec3 lightPos =  vec3(0., 0., 5);
  vec3 up = vec3(0,1,0);

  mat3 viewWorld = viewMatrix(eye, center, up);
  vec3 ray = rayDirection(90., u_res, fc);

  vec3 worldDir = viewWorld * ray;
  vec3 col;
  float d = rayMarch(eye, worldDir, col);
  vec3 v = eye + worldDir*d;

  if(d < MaxDist){
    vec3 n = estimateNormal(v);
    float lights = lighting(v, n, lightPos, eye);
    // float lights2 = lighting(v, n, -lightPos, eye);
    i = lights * col.x;
    // i = col.x;
  }

  i = pow(i, 1./2.2);
  gl_FragColor = vec4(vec3(i), 1);
}