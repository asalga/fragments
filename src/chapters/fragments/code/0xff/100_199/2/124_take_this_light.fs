// 124 - "Take this Light"
precision highp float;
uniform vec2 u_res;
uniform float u_time;

const float Epsilon = 0.0001;
const float E = Epsilon;
const float MaxDist = 100.;
const int MaxSteps = 228;

const float PI = 3.141592658;
const float TAU = PI*2.;
const float HALF_PI = PI*0.5;

const vec3 lightGrey = vec3(1.);
const vec3 darkGrey = vec3(0.4);

const int MaxShadowStep = 100;


float valueNoise(vec2 p){
  #define Y_SCALE 95343.
  #define X_SCALE 137738.  
  float x = p.x * X_SCALE;
  float y = p.y * Y_SCALE;  
  return fract( sin(x+y) * 329354.);
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

float sdBox(vec3 p, vec3 sz) {
  vec3 d = abs(p) - sz;
  float _in = min(max(d.x, max(d.y, d.z)), 0.);
  float _out = length(max(d, 0.));
  return _in + _out;
}

float getHeight(vec2 c){
 float n;
  n += smoothValueNoise(c*2. + vec2(0,0)) * 0.5;
  // n += smoothValueNoise(c*4.) * 0.25;
  // n += smoothValueNoise(c*6.) * 0.125;
  n += smoothValueNoise(c*2. + vec2(0,0)) * 0.0625;
  n /= .5;
  return n;
}

mat3 viewMatrix(vec3 eye, vec3 center, vec3 up) {
  vec3 f = normalize(center - eye);
  vec3 s = normalize(cross(f, up));
  vec3 u = cross(s, f);
  return mat3(s, u, -f);
}

float lighting(vec3 p, vec3 n, vec3 lightPos, vec3 eye){
  float ambient = 0.01;
  // ---
  vec3 pToLight = vec3(lightPos - p);
  float power = 100.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  float nDotL = max(dot(n,lightRayDir), 0.);
  float diffuse = (nDotL*power) / d;
  float kd = 1.;

  float gloss = 1.;    
  vec3 V = normalize(eye - p);
  vec3 R = normalize(reflect(-lightPos, n)); 
  float dotRV = dot(R, V);
  float spec = pow(dotRV, gloss);

  return ambient + 0.38*diffuse;// + spec;
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
  float t = u_time*0.;
  vec3 c = vec3(3.5, 0, 4);
  vec3 np = mod(p, c)-0.5*c;

  float vert = sdBox(np, vec3(.1, 0.8, .1));
  float horz = sdBox(np- vec3(0,0.3, 0), vec3(.55, 0.1, .1));

  float h = getHeight(p.xz)/15.;
  float ground = sdBox(p - vec3(0, h-0.95, 0.), vec3(10., 0.25, 50.));
  if(ground < 0.001){
    rot.x = 1.;
  }

  float final = min(vert, horz);
  final = min(final, ground);

  return final;
}




float shadowMarch(vec3 point, vec3 lightPos){

  vec3 pToLight = lightPos-point;
  vec3 rd = normalize(pToLight);
  vec3 ro = point;

  float s = 0.;
  for(int i = 0; i < MaxShadowStep; ++i){
    vec3 v = ro + (rd*s);
    
    vec3 dum;
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

void main(){
  float i;
  float t = u_time * 1.0;
  float dist = 4.;

  vec3 eye = vec3(0, 1, t);
  vec3 center = vec3(0, 1, 15. +t);

  vec3 lightPos = vec3(cos(u_time), 2, -2. + sin(u_time)) + eye;
  vec3 up = vec3(0,1,0);
  vec3 ray = rayDirection(80.0, u_res, gl_FragCoord.xy);

  vec3 worldDir = viewMatrix(eye, center, up) * ray;
  vec3 rot = vec3(0);
  float d = rayMarch(eye, worldDir, rot);
  vec3 i3 = vec3(0);

  if(d < MaxDist){
    vec3 v = eye + worldDir*d;
    vec3 n = estimateNormal(v);
    float lights = lighting(v, n, lightPos, eye);


    vec3 shadowLight = vec3(sin(u_time*1.), 6,   -3. + eye.z + cos(u_time*1.));  
    float visibleToLight = shadowMarch(eye+worldDir*(d-0.001),   shadowLight);
    
    if(rot.x > 0.){
      i3 = vec3(d * lights*0.1);
    }
    else{
      i3 = vec3(d * lights * 1.);
    }
    
    if(visibleToLight == 0. ){
      i3 *= 0.4;
    }

    float fog = (20./pow(d*d,1.));
    i3 *= fog * 1.;

    // i3 = pow(i3, 1./vec3(2.2));
  }

  // float test = valueNoise( gl_FragCoord.xy/u_res + vec2(t, t));
  // i3 *= vec3(test*0.93);

  gl_FragColor = vec4(vec3(i3), 1);
}