// 131
precision highp float;
uniform vec2 u_res;
uniform float u_time;

const float Epsilon = 0.0001;
const float E = Epsilon;
const float MaxDist = 300.;
const int MaxSteps = 328;
const float PI = 3.141592658;
const float HALF_PI = PI*0.5;
const int MaxShadowStep = 100;

const vec3 lightGrey = vec3(1.);
const vec3 darkGrey = vec3(0.4);
const float X_SCALE= 13443.;
const float Y_SCALE = 389492.;

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



float lighting(vec3 p, vec3 n, vec3 lightPos, vec3 eye){
  float ambient = 0.0;

  // ---
  vec3 pToLight = vec3(eye - p);
  float power = 40.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  float nDotL = max(dot(n,lightRayDir), 0.);
  float diffuse = (nDotL*power) / d;
  float kd = .5;

  float gloss = 300.;
  vec3 V = normalize(eye - p);
  vec3 R = normalize(reflect(-lightPos, n));
  float dotRV = dot(R, V);
  float spec = pow(dotRV, gloss);
  float ks = .0;

  return  ambient +
          kd * diffuse +
          ks * spec;
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


float sdScene(vec3 p, out float col){
  float t = u_time * 5.0;
  col = .4;

  float c = 1.;
  vec3 np = mod(p, vec3(c))-c *0.5;
  np.y = p.y;

  float xIdx = floor(p.x*1.)/10.;
  float zIdx = floor(p.z*1.)/10.;

  if(abs(xIdx) > 2. || abs(zIdx) > 2.){
    col = 0.;
  }

  float d = sqrt(xIdx*xIdx+zIdx*zIdx);
  float y = sin(9. * d - t) * 3.;
  y /= 1. + d * 7.;
  vec3 off = vec3(0, y ,0);

  float cube =  cubeSDF( np + off, vec3(0.25, .1, .25));
  float sphere =  sdSphere(np+off, 0.25 + (sin( c/4.- (t*0.125) ) +1.)/2.);

  return mix(cube, sphere, y + (1.+sin(y + t*0.25))/2. );
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
    s += d/4.;

    if(d > MaxDist){
      return MaxDist;
    }
  }
  return MaxDist;
}

float ao(vec3 p, vec3 n)
{
  float stepSize = .02;
  float t = stepSize;
  float oc = 0.;
  float dum;
  for(int i = 0; i < 4; ++i)
  {
    float d = sdScene(p+n*t, dum);
    oc += t - d;
    t += stepSize;
  }

  return clamp(oc, 0., 1.);
}


void main(){
  // vec2 p = (gl_FragCoord.xy/u_res)*2. -1.;
  float i;
  float t = u_time * .14;
  vec2 fc = gl_FragCoord.xy;

  // glitch
  // float vn = valueNoise(0., vec2(t));
  // if(vn > .02){
  //  fc.x  += 40.;
  // }
  // if(vn > 0.1){
  //  fc.y -= 35.;
  // }

  float x =  sin(t) * 10.;
  float z =  cos(t) * 10.;
  z = 4.9;
  x = 4.9;

  vec3 eye = vec3( cos(t) * 22.0  , 10. , sin(t)*22.);
  vec3 center = vec3(0,0,0);
  vec3 lightPos =   vec3(-1,5,-1) + eye;
  vec3 up = vec3(0,1,0);

  mat3 viewWorld = viewMatrix(eye, center, up);
  vec3 ray = rayDirection(80.0, u_res, fc);

  vec3 worldDir = viewWorld * ray;
  vec3 col;
  float d = rayMarch(eye, worldDir, col);


  // i = 0.1;
  vec3 v = eye + worldDir*d;

  if(d < MaxDist){
    // float visibleToLight = shadowMarch(eye + ray * d, lightPos);
    // float visibleToLight = shadowMarch(eye+worldDir*(d-0.001), lightPos);

    vec3 n = estimateNormal(v);

    float lights = lighting(v, n, lightPos, eye);

    i += (d * lights) * col.x;
    // i = col.x;
  }

  // float fog = 5./ pow( d, 2.);
  // i *= fog;

  // float vignette = 1.-smoothstep(0.9, 1., abs(p.x)) *
                   // 1.-smoothstep(0.9, 1., abs(p.y));
  // i *= vignette;

  gl_FragColor = vec4(vec3(i), 1);
}
