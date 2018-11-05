// 121 - "Friends in the Amazon" / "Slice of Noise"
precision highp float;
uniform vec2 u_res;
uniform float u_time;

const float Epsilon = 0.00001;
const float MaxDist = 200.;
const int MaxSteps = 300;
const int MaxShadowStep = 50;
const float PI = 3.141592658;
const float TAU = PI*2.;
const float HALF_PI = PI*0.5;

const float lightGrey = 1.;
const float darkGrey = .0;

float easeInOutBack(float t, float b, float c, float d){
  float s = .0;//70158;
  if ((t /= d/2.) < 1.){
    return c/2.*(t*t*(((s*=(1.525))+1.)*t - s)) + b;
  }
  return c/2.*((t-=2.)*t*(((s*=(1.525))+1.)*t + s) + 2.) + b;
}

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

float getHeight(vec2 c){
  float t = -u_time*2.;

  float n;
  n += smoothValueNoise(c*2. + t/10.) * 0.5;
  n += smoothValueNoise(c*4.) * 0.25;
  n += smoothValueNoise(c*6.) * 0.125;
  n += smoothValueNoise(c*8.+t*3.) * 0.0625;
  n /= .89;

  // c.x += sin(t/2.);
  // c.x += smoothValueNoise( vec2(t/2.) ) * 1.5;
  // c.y += smoothValueNoise( vec2((t+10.)/2.) ) * 1.5;

  // c.x += sin(t);
  // c.y += cos(t);

  float porab = pow( sqrt(dot(c,c)/2. ) , 3.1)-2.;

  ///float modt = step(1., mod(t, 2.));
  float e = easeInOutBack( fract(t) , 1.1, .0, .01);
  return (n*2. - porab) * e;
  //abs(sin(t*0.2));

  return smoothValueNoise( (c + vec2(t * .1)) *1.);

  float r = length(c)*4. + t;
  float rLen = sin(r) + cos(r*2.);
  return rLen;
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

// sss - just a marker
float sdScene(vec3 p, out float col){
  float h = getHeight(p.xz);
  h = max(h, .25);
  float y = sin(p.x+u_time);

  // p.x += floor(u_time*20.)/20.;
  float ch = sampleChecker(p.xz*2.0 - vec2(u_time*5., 0.));

  float c_ = 1.0;
  vec2 uv = mod(p.xz, vec2(c_))*0.5*c_ - 0.25;
  uv *= 5.;
  uv.x += 0.5 + u_time*10.; // + sin(u_time);
  // uv.y -= 0.5 + u_time;

  float th = (sin(p.x)+1.)/2.;

  // float c = ( uv);
  float c = 0.;

  col = ch * 0.2;
  col *= 10.;

  float box = sdBox(p - vec3(0, y + sin(u_time), 0), vec3(120., th*0.5, 1.));
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

  // vec3 eye = vec3(dist * cos(t), 2.  , dist * sin(t));
  vec3 eye = vec3(4., 3., 2.);
  vec3 center = vec3(0, 0, 0.);
  vec3 lightPos =  vec3(0., 0., 5);
  vec3 up = vec3(0,1,0);

  mat3 viewWorld = viewMatrix(eye, center, up);
  vec3 ray = rayDirection(150., u_res, fc);

  vec3 worldDir = viewWorld * ray;
  vec3 col;
  float d = rayMarch(eye, worldDir, col);
  vec3 v = eye + worldDir*d;

  if(d < MaxDist){
    vec3 n = estimateNormal(v);
    float lights = lighting(v, n, lightPos, eye);
    float lights2 = lighting(v, n, -lightPos, eye);

    // i = (lights2+lights) * col.x;
    i = col.x;
  }

  // i = pow(i, 1./2.2);

  gl_FragColor = vec4(vec3(i), 1);
}